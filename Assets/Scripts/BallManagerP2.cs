using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BallManagerP2 : MonoBehaviour
{
    public Transform puntoLanzamientoP2; // Punto de lanzamiento para el segundo jugador
    public Vector3 cestaP2;               // Posición de la cesta para el segundo jugador
    public float alturaMaximaP2 = 5f;     // Altura máxima que debe alcanzar el balón
    private GameObject balonCancha;       // Referencia al balón de la cancha
    public bool tieneBalonP2 = false;    // Verifica si el segundo jugador ha recogido la pelota del centro
    private ControlCodeP2 controlCode;    // Referencia al script del personaje
    private MovimientoDeCamara camara;    // Referencia al script de la cámara

    void Start()
    {
        // Inicializar puntoLanzamientoP2 si es null
        if (puntoLanzamientoP2 == null)
        {
            puntoLanzamientoP2 = this.transform;
        }

        // Buscar el BalonCancha por su tag
        balonCancha = GameObject.FindGameObjectWithTag("BalonCancha");

        // Asegúrate de que el balonCancha esté activo al inicio
        if (balonCancha != null)
        {
            balonCancha.SetActive(true);
        }
        else
        {
            Debug.LogWarning("No se encontró el BalonCancha con el tag especificado.");
        }

        controlCode = GetComponent<ControlCodeP2>();

        // Obtener la referencia al script de la cámara
        camara = Camera.main.GetComponent<MovimientoDeCamara>();
    }

    void Update()
    {
        // Recoger balón con el botón "O" o "buttonEast"
        if (Gamepad.all.Count > 1 && Gamepad.all[1].buttonEast.wasPressedThisFrame && !tieneBalonP2)
        {
            Debug.Log("Jugador 2 intentando recoger la pelota...");
            RecogerBalonP2();
        }

        // Lanzar balón con el botón "X" o "buttonSouth"
        if (Gamepad.all.Count > 1 && Gamepad.all[1].buttonSouth.wasPressedThisFrame && tieneBalonP2)
        {
            LanzarBalonAwait();
        }

        // Robo de balón con el botón "Y" o "buttonNorth" para el Jugador 2
        if (Gamepad.all.Count > 1 && Gamepad.all[1].buttonNorth.wasPressedThisFrame && !tieneBalonP2)
        {
            // Detectar colisiones cercanas
            Collider[] colliders = Physics.OverlapSphere(transform.position, 1.5f);
            foreach (Collider col in colliders)
            {
                // Si el objeto tiene el componente BallManagerP1 (para el Jugador 1)
                BallManagerP1 jugador1 = col.GetComponent<BallManagerP1>();
                if (jugador1 != null && !tieneBalonP2)
                {
                    StartCoroutine(QuitarBalonAwait(jugador1.gameObject, 0.85f));
                }
            }
        }
    }

    void RecogerBalonP2()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 1.5f); // Rango de recogida
        foreach (Collider col in colliders)
        {
            if (col.gameObject == balonCancha)
            {
                balonCancha.SetActive(false); // Desactivar el balón de la cancha
                tieneBalonP2 = true; // Indicar que el jugador ahora tiene el balón

                // Cambiar objetivo de la cámara al jugador con tag Player2
                camara.CambiarObjetivoAPlayer2();

                controlCode.CambiarEstadoBalon(tieneBalonP2);
            }
        }
    }

    void LanzarBalonAwait()
    {
        controlCode.LanzarBalonP2();
        Invoke("LanzarBalon", 2f);
    }

    void LanzarBalon()
    {
        // Colocar el balón de la cancha 2 unidades más lejos en la dirección de frente del jugador
        Vector3 lanzamientoPosicion = puntoLanzamientoP2.position + puntoLanzamientoP2.forward * 2; // Aumentar 2 unidades
        balonCancha.SetActive(true);
        balonCancha.transform.position = lanzamientoPosicion;

        Rigidbody rb = balonCancha.GetComponent<Rigidbody>();

        // Calcular la trayectoria del lanzamiento
        Vector3 toCesta = cestaP2 - lanzamientoPosicion; // Usar la nueva posición de lanzamiento
        Vector3 toCestaXZ = new Vector3(toCesta.x, 0, toCesta.z);
        float tiempo = Mathf.Sqrt(-2 * alturaMaximaP2 / Physics.gravity.y) +
                       Mathf.Sqrt(2 * (toCesta.y - alturaMaximaP2) / Physics.gravity.y);
        Vector3 velocidadXZ = toCestaXZ / tiempo;
        float velocidadY = Mathf.Sqrt(-2 * Physics.gravity.y * alturaMaximaP2);
        Vector3 velocidadInicial = velocidadXZ + Vector3.up * velocidadY;
        rb.velocity = velocidadInicial;

        tieneBalonP2 = false; // Resetear el estado de tener balón

        // Cambiar objetivo de la cámara al balón
        camara.CambiarObjetivoAlBalon(balonCancha.transform);

        controlCode.CambiarEstadoBalon(tieneBalonP2);
    }

    IEnumerator QuitarBalonAwait(GameObject oponente, float delay)
    {
        controlCode.QuitarBalonP2();
        yield return new WaitForSeconds(delay);
        RobarBalonP2(oponente);
    }

    public void RobarBalonP2(GameObject oponente)
    {
        // Verificar si el oponente (Jugador 1) tiene el balón
        BallManagerP1 managerOponente = oponente.GetComponent<BallManagerP1>();
        ControlCodeP1 controlCodeOponente = oponente.GetComponent<ControlCodeP1>();

        if (managerOponente != null && managerOponente.tieneBalon)
        {
            managerOponente.tieneBalon = false;
            tieneBalonP2 = true;

            // Cambiar el objetivo de la cámara al jugador que robó el balón (Player2)
            camara.CambiarObjetivoAPlayer2();

            if (controlCodeOponente != null)
            {
                controlCodeOponente.BalonRobadoP1();
                controlCodeOponente.CambiarEstadoBalon(false);
            }

            controlCode.CambiarEstadoBalon(tieneBalonP2);
            Debug.Log("Jugador 2 ha robado el balón del Jugador 1.");
        }
    }
}
