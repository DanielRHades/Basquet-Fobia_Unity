using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControlCodeP2 : MonoBehaviour
{
    private Vector3 ultimaPosicion;

    // Variables para las velocidades de trotar y correr
    public float velocidadTrotar = 100f;
    public float velocidadCorrer = 300f;

    // Variable para capturar el valor del gatillo (correr)
    float correrValor = 0f;

    // Variable para capturar el valor de movimiento del stick
    Vector2 direccionMovimiento;

    // Velocidad de suavización de rotación
    public float suavizadoRotacion = 5f;
    private BallManagerP2 ballManager;
    // Umbral para considerar movimiento
    public float umbralMovimiento = 0.1f;
    public Animator player2;
    public Animator balon;// Asegúrate de asignar esto desde el inspector

    void Start()
    {   
        ballManager = GetComponent<BallManagerP2>();

        ultimaPosicion = transform.position;

        if (player2 == null)
        {
            Debug.LogWarning("Animator no asignado en el inspector.");
        }
        else
        {
            // Listar todos los parámetros para verificar cuáles están disponibles
            foreach (var param in player2.parameters)
            {
                Debug.Log("Parámetro encontrado: " + param.name + " - Tipo: " + param.type);
            }
        }
    }

    void Update()
    {   
        if(ballManager.bloqueandoInputsP2)
        {
            return;
        }
        // Movimiento del personaje
        if (direccionMovimiento.magnitude > umbralMovimiento)
        {
            float velocidadActual = velocidadTrotar;
            if (correrValor > 0)
            {
                velocidadActual = Mathf.Lerp(velocidadTrotar, velocidadCorrer, correrValor);
            }

            Vector3 movimiento = new Vector3(direccionMovimiento.x, 0, direccionMovimiento.y).normalized;
            transform.Translate(movimiento * velocidadActual * Time.deltaTime, Space.World);

            if (movimiento != Vector3.zero)
            {
                Quaternion rotacion = Quaternion.LookRotation(movimiento);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotacion, Time.deltaTime * suavizadoRotacion);
            }

            AnimatotionPlay();
            ultimaPosicion = transform.position;
        }
        else
        {
            // Si no se mueve, actualizar el estado
            AnimatotionPlay();
        }
    }

    private bool HasMoved()
    {
        return Vector3.Distance(transform.position, ultimaPosicion) > 0.01f;
    }

    // Función para cuando se mueve el personaje con el stick
    public void EnMovimiento(InputAction.CallbackContext ctx)
    {
        direccionMovimiento = ctx.ReadValue<Vector2>();
    }

    // Función para cuando se presiona el gatillo (Correr)
    public void EnCorrer(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            correrValor = ctx.ReadValue<float>();
        }
        else if (ctx.canceled)
        {
            correrValor = 0f;
        }
    }

    public void AnimatotionPlay()
    {
        if (player2 != null)
        {
            // Cambia el parámetro según el movimiento
            player2.SetBool("IsMovingP2", direccionMovimiento.magnitude > umbralMovimiento);
            
        }

        if (balon != null)
        {
            // Cambia el parámetro según el movimiento
            balon.SetBool("IsMovingP2", direccionMovimiento.magnitude > umbralMovimiento);
            
        }
    }
    public void CambiarEstadoBalon(bool tieneBalon)
    {
        if (player2 != null)
        {
            player2.SetBool("TieneBalonP2", tieneBalon);
            Debug.Log("Estado de balón actualizado: " + tieneBalon);
        }

        if (balon != null)
        {
            balon.SetBool("TieneBalonP2", tieneBalon);
            Debug.Log("Estado de balón actualizado: " + tieneBalon);
        }
    }

    public void LanzarBalonP2()
    {
        if (player2 != null)
        {   
            player2.ResetTrigger("BalonRobadoP2");
            player2.ResetTrigger("QuitarBalonP2");
            player2.SetTrigger("LanzarP2");
            balon.SetTrigger("LanzarBalonP2"); // Asegúrate de que este trigger esté definido en tu Animator
        }
    }
   
    public void QuitarBalonP2()
    {
        if (player2 != null)
        {   
            balon.ResetTrigger("LanzarBalonP2");
            player2.ResetTrigger("LanzarP2");
            player2.ResetTrigger("BalonRobadoP2");
            player2.SetTrigger("QuitarBalonP2"); // Asegúrate de que este trigger esté definido en tu Animator
        }
    }

        public void BalonRobadoP2()
    {
        if (player2 != null)
        {   
            balon.ResetTrigger("LanzarBalonP2");
            player2.ResetTrigger("LanzarP2");
            player2.ResetTrigger("QuitarBalonP2");
            player2.SetTrigger("BalonRobadoP2"); // Asegúrate de que este trigger esté definido en tu Animator
        }
    }
}
