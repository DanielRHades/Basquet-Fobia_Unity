using UnityEngine;

public class MovimientoDeCamara : MonoBehaviour
{
    public float fixedYPosition;      // Posición fija en el eje Y
    public float fixedZPosition;      // Posición fija en el eje Z
    public float smoothSpeed = 0.125f; // Velocidad de suavizado

    // Offset para la distancia de la cámara respecto al objetivo
    public Vector3 farOffset = new Vector3(0, 5, -10); // Vista general
    public Vector3 closeOffset = new Vector3(0, 4, -8); // Vista cercana

    private Vector3 currentOffset;    // Offset actual de la cámara
    private Transform targetObject;   // El objeto que la cámara sigue dinámicamente
    private string balonTag = "BalonCancha";
    private string player1Tag = "Player1";
    private string player2Tag = "Player2";

    private void Start()
    {
        GameObject balon = GameObject.FindWithTag(balonTag);
        if (balon != null)
        {
            targetObject = balon.transform;
        }
        else
        {
            Debug.LogError("No se encontró un objeto con el tag 'BalonCancha'. Asegúrate de que existe en la escena.");
        }

        Vector3 initialPosition = transform.position;
        fixedYPosition = initialPosition.y;
        fixedZPosition = initialPosition.z;

        // Establecer el offset inicial como la vista general
        currentOffset = farOffset;
    }

    private void LateUpdate()
    {
        if (targetObject != null)
        {
            // Determinar si el objetivo se está moviendo
            bool isMoving = targetObject.GetComponent<Rigidbody>() != null && targetObject.GetComponent<Rigidbody>().velocity.magnitude > 0.1f;

            // Cambiar el offset según el movimiento del objetivo
            currentOffset = isMoving ? closeOffset : farOffset;

            // Calcula la posición deseada con el offset actual
            Vector3 desiredPosition = targetObject.position + currentOffset;
            desiredPosition.y = fixedYPosition; // Mantener la altura fija

            // Suavizar el movimiento de la cámara hacia la posición deseada
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;

            // Rotación opcional para mirar al objetivo
            transform.LookAt(targetObject.position);
        }
    }

    // Cambiar objetivo a "Player1"
    public void CambiarObjetivoAPlayer1()
    {
        GameObject player1 = GameObject.FindWithTag(player1Tag);
        if (player1 != null)
        {
            targetObject = player1.transform;
            Debug.Log("La cámara ahora sigue al jugador con tag Player1");
        }
        else
        {
            Debug.LogWarning("No se encontró un objeto con el tag 'Player1'.");
        }
    }

    // Cambiar objetivo a "Player2"
    public void CambiarObjetivoAPlayer2()
    {
        GameObject player2 = GameObject.FindWithTag(player2Tag);
        if (player2 != null)
        {
            targetObject = player2.transform;
            Debug.Log("La cámara ahora sigue al jugador con tag Player2");
        }
        else
        {
            Debug.LogWarning("No se encontró un objeto con el tag 'Player2'.");
        }
    }

    // Cambiar objetivo al balón
    public void CambiarObjetivoAlBalon(Transform balon)
    {
        targetObject = balon;
        Debug.Log("La cámara ahora sigue al balón: " + balon.name);
    }
}
