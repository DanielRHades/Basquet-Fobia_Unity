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
    public bool tieneBalon = false;    // Si el jugador tiene un balón en la mano
    private ControlCodeP1 controlCode;    // Referencia al script del personaje

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
    }

    void Update()
    {
        // Recoger balón con el botón "O" o "buttonEast"
        if (Gamepad.all.Count > 0 && Gamepad.all[0].buttonEast.wasPressedThisFrame && !tieneBalon)
        {
            Debug.Log("Intentando recoger la pelota...");
            RecogerBalon();
        }

        // Lanzar balón con el botón "X" o "buttonSouth"
        if (Gamepad.all.Count > 0 && Gamepad.all[0].buttonSouth.wasPressedThisFrame && tieneBalon)
        {
            LanzarBalon();
        }

        // Robo de balón con el botón "Y" o "buttonNorth" para el Jugador 2
        if (Gamepad.all.Count > 0 && Gamepad.all[0].buttonNorth.wasPressedThisFrame && !tieneBalon)
        {
            // Detectar colisiones cercanas
            Collider[] colliders = Physics.OverlapSphere(transform.position, 1.5f);
            foreach (Collider col in colliders)
            {
                // Si el objeto tiene el componente BallManagerP2 (para el Jugador 2)
                BallManagerP2 jugador2 = col.GetComponent<BallManagerP2>();
                if (jugador2 != null && !tieneBalon)
                {
                    RobarBalon(jugador2.gameObject);
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

                controlCode.CambiarEstadoBalon(tieneBalon);
            }
        }
    }

    void LanzarBalon()
    {
        // Colocar el balón de la cancha 2 unidades más lejos en la dirección de frente del jugador
        Vector3 lanzamientoPosicion = puntoLanzamiento.position + puntoLanzamiento.forward * 2; // Aumentar 2 unidades
        balonCancha.SetActive(true);
        balonCancha.transform.position = lanzamientoPosicion;

        Rigidbody rb = balonCancha.GetComponent<Rigidbody>();

        // Calcular la trayectoria del lanzamiento
        Vector3 toCesta = cesta - lanzamientoPosicion; // Usar la nueva posición de lanzamiento
        Vector3 toCestaXZ = new Vector3(toCesta.x, 0, toCesta.z);
        float tiempo = Mathf.Sqrt(-2 * alturaMaxima / Physics.gravity.y) +
                       Mathf.Sqrt(2 * (toCesta.y - alturaMaxima) / Physics.gravity.y);
        Vector3 velocidadXZ = toCestaXZ / tiempo;
        float velocidadY = Mathf.Sqrt(-2 * Physics.gravity.y * alturaMaxima);
        Vector3 velocidadInicial = velocidadXZ + Vector3.up * velocidadY;
        rb.velocity = velocidadInicial;

        tieneBalon = false; // Resetear el estado de tener balón

        // Actualizar el estado del balón en el personaje
        controlCode.CambiarEstadoBalon(tieneBalon);
    }

    public void RobarBalon(GameObject oponente)
    {
        // Verificar si el oponente tiene el balón
        BallManagerP2 managerOponente = oponente.GetComponent<BallManagerP2>();
        ControlCodeP2 controlCodeOponente = oponente.GetComponent<ControlCodeP2>(); // Obtener el script ControlCodeP1 de Player1

       if (managerOponente != null && managerOponente.tieneBalonP2)
        {
        // Actualizar el estado de tieneBalon en Player1
            managerOponente.tieneBalonP2 = false; 
            tieneBalon = true;

        // Cambiar el estado de balón en el animador de Player1 para desactivar dribleo
            if (controlCodeOponente != null)
            {
                controlCodeOponente.CambiarEstadoBalon(false);
            }

        // Activar el estado de balón en Player2
            controlCode.CambiarEstadoBalon(tieneBalon);

            Debug.Log("Jugador 1 ha robado el balón del Jugador 2.");
        }
    }

}
