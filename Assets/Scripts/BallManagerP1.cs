using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BallManagerP1 : MonoBehaviour
{
    public Transform puntoLanzamiento; // Punto de lanzamiento
    public Vector3 cesta;               // Posición de la cesta
    public float alturaMaxima = 5f;     // Altura máxima
    private GameObject balonCancha;     // Referencia al balón de la cancha
    public bool tieneBalon = false;  
    public bool bloqueandoInputs = false;
    public bool lanzandoBalon = false;  // Si el jugador tiene un balón en la mano
    private ControlCodeP1 controlCode;    // Referencia al script del personaje
    private MovimientoDeCamara camara;   // Referencia al script de la cámara

    void Start()
    {
        if (puntoLanzamiento == null)
        {
            puntoLanzamiento = this.transform; // Asignar punto de lanzamiento si es null
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

        // Obtener la referencia al ControlCode en el mismo GameObject
        controlCode = GetComponent<ControlCodeP1>();

        // Obtener la referencia al script de la cámara
        camara = Camera.main.GetComponent<MovimientoDeCamara>();
    }

    void Update()
    {   
         // Verificar si los inputs están bloqueados
        if (bloqueandoInputs)
        {
            return;
        }
            
        // Recoger balón con el botón "O" o "buttonEast"
        if (Gamepad.all.Count > 0 && Gamepad.all[0].buttonNorth.wasPressedThisFrame && !tieneBalon)
        {
            Debug.Log("Intentando recoger la pelota...");
            RecogerBalon();
        }

        // Lanzar balón con el botón "X" o "buttonSouth"
        if (Gamepad.all.Count > 0 && Gamepad.all[0].buttonEast.wasPressedThisFrame && tieneBalon)
        {
            LanzarBalonAwait();
        }

        // Robo de balón con el botón "Y" o "buttonNorth" para el Jugador 2
        if (Gamepad.all.Count > 0 && Gamepad.all[0].buttonWest.wasPressedThisFrame && !tieneBalon)
        {
            // Detectar colisiones cercanas
            Collider[] colliders = Physics.OverlapSphere(transform.position, 1.5f);
            foreach (Collider col in colliders)
            {
                // Si el objeto tiene el componente BallManagerP2 (para el Jugador 2)
                BallManagerP2 jugador2 = col.GetComponent<BallManagerP2>();
                if (jugador2 != null && !tieneBalon)
                {
                    StartCoroutine(QuitarBalonAwait(jugador2.gameObject, 0.85f));
                }
            }
        }
    }

    void RecogerBalon()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 1.5f); // Rango de recogida
        foreach (Collider col in colliders)
        {
            if (col.gameObject == balonCancha)
            {
                balonCancha.SetActive(false); // Desactivar el balón de la cancha
                tieneBalon = true; // Indicar que el jugador ahora tiene el balón

                // Cambiar objetivo de la cámara al jugador con tag Player1
                camara.CambiarObjetivoAPlayer1();

                controlCode.CambiarEstadoBalon(tieneBalon);
            }
        }
    }

    void LanzarBalonAwait()
    {
        if (!lanzandoBalon)
        {
            lanzandoBalon = true; 
            bloqueandoInputs = true; 
            controlCode.LanzarBalonP1();
            Invoke("LanzarBalon", 1.6f); 
        }
    }

    void LanzarBalon()
    {
    // Posición de lanzamiento relativa al personaje
    Vector3 lanzamientoRelativo = new Vector3(0, 0.727999985f, 0.556999981f);
    Vector3 lanzamientoPosicion = puntoLanzamiento.position + puntoLanzamiento.TransformDirection(lanzamientoRelativo);
    balonCancha.SetActive(true);
    balonCancha.transform.position = lanzamientoPosicion;

    Rigidbody rb = balonCancha.GetComponent<Rigidbody>();

    // Calcular la trayectoria del lanzamiento
    Vector3 toCesta = cesta - lanzamientoPosicion; // Vector que apunta a la cesta

    // Distancia en X entre el jugador y la cesta
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
    float tiempo = Mathf.Sqrt(-2 * alturaMaxima / Physics.gravity.y) +
                   Mathf.Sqrt(2 * (toCesta.y - alturaMaxima) / Physics.gravity.y);
    Vector3 velocidadXZ = toCestaXZ / tiempo;
    float velocidadY = Mathf.Sqrt(-2 * Physics.gravity.y * alturaMaxima);
    Vector3 velocidadInicial = velocidadXZ + Vector3.up * velocidadY;

    rb.velocity = velocidadInicial;

    tieneBalon = false; // Resetear el estado de tener balón

    // Cambiar objetivo de la cámara al balón
    camara.CambiarObjetivoAlBalon(balonCancha.transform);

    controlCode.CambiarEstadoBalon(tieneBalon);

    lanzandoBalon = false;

    bloqueandoInputs = false;

    }

    IEnumerator QuitarBalonAwait(GameObject oponente, float delay)
    {
        controlCode.QuitarBalonP1();
        yield return new WaitForSeconds(delay);
        RobarBalon(oponente);
    }

    public void RobarBalon(GameObject oponente)
    {
        // Verificar si el oponente tiene el balón
        BallManagerP2 managerOponente = oponente.GetComponent<BallManagerP2>();
        ControlCodeP2 controlCodeOponente = oponente.GetComponent<ControlCodeP2>();

        if (managerOponente != null && managerOponente.tieneBalonP2 && !managerOponente.lanzandoBalonP2)
        {
            // Actualizar el estado de tieneBalon en Player1
            managerOponente.tieneBalonP2 = false;
            tieneBalon = true;

            // Cambiar el objetivo de la cámara al jugador que robó el balón
            camara.CambiarObjetivoAPlayer1();

            if (controlCodeOponente != null)
            {
                controlCodeOponente.BalonRobadoP2();
                controlCodeOponente.CambiarEstadoBalon(false);
            }

            controlCode.CambiarEstadoBalon(tieneBalon);
            Debug.Log("Jugador 1 ha robado el balón del Jugador 2.");
        }
    }
}
