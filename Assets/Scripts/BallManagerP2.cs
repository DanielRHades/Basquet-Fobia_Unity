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
    public bool tieneBalonP2 = false;  
    public bool bloqueandoInputsP2 = false;
    public bool lanzandoBalonP2 = false;  // Verifica si el segundo jugador ha recogido la pelota del centro
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
        // Verificar si los inputs están bloqueados
        if (bloqueandoInputsP2)
        {
            return;
        }

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
        if (!lanzandoBalonP2)
        {
            lanzandoBalonP2 = true; 
            bloqueandoInputsP2 = true; 
            controlCode.LanzarBalonP2();
            Invoke("LanzarBalon", 1.6f); 
        }
    }

    void LanzarBalon()
    {
    // Colocar el balón de la cancha 2 unidades más lejos en la dirección de frente del jugador
    Vector3 lanzamientoPosicion = puntoLanzamientoP2.position + puntoLanzamientoP2.forward * 2;
    balonCancha.SetActive(true);
    balonCancha.transform.position = lanzamientoPosicion;

    Rigidbody rb = balonCancha.GetComponent<Rigidbody>();

    // Calcular la trayectoria del lanzamiento
    Vector3 toCesta = cestaP2 - lanzamientoPosicion;

    // Distancia en X entre el jugador y la cesta (en valor absoluto para manejar canchas opuestas)
    float distanciaX = Mathf.Abs(toCesta.x);

    // Definir la probabilidad de acierto o fallo
    float probabilidadAcierto = (distanciaX > 12.8f) ? 0.2f : (distanciaX > 6.4f ? 0.6f : 1.0f);

    // Decidir si se modifica la posición en Z para fallar el tiro
    if (distanciaX > 6.4f && Random.value > probabilidadAcierto)
    {
        // Elegir aleatoriamente si desviamos la Z en 0.5 o -0.5
        float desviacionZ = Random.Range(0, 2) == 0 ? 0.5f : -0.5f;
        toCesta.z += desviacionZ;

        // Mostrar en la consola la probabilidad y la desviación en Z
        Debug.Log("Probabilidad: " + (1 - probabilidadAcierto) + " - Fallo en Z modificado a: " + toCesta.z);
    }
    else
    {
        // Mostrar en la consola la probabilidad y que el tiro es acertado
        Debug.Log("Probabilidad: " + probabilidadAcierto + " - Tiro sin modificación en Z");
    }

    // Calcular el lanzamiento usando la posición modificada en Z
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

    lanzandoBalonP2 = false;

    bloqueandoInputsP2 = false;

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

        if (managerOponente != null && managerOponente.tieneBalon && !managerOponente.lanzandoBalon)
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
