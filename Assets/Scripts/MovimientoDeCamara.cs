using UnityEngine;

public class MovimientoDeCamara : MonoBehaviour
{
    public float smoothSpeed = 0.0000000005f; // Velocidad de suavizado más baja para un seguimiento más suave
    public Vector3 playerOffset = new Vector3(0, 5, -10); // Offset cuando sigue al jugador (más cerca)
    private Vector3 fixedBallPosition = new Vector3(-0.23f, 7.27f, -12.33f); // Posición inicial para la cámara cuando sigue la pelota

    private Transform targetObject;    // El objeto que la cámara sigue dinámicamente
    private string balonTag = "BalonCancha";
    private string player1Tag = "Player1";
    private string player2Tag = "Player2";
    private bool isFollowingBall = true; // Controla si está siguiendo al balón inicialmente

    private void Start()
    {
        // Configura la cámara en la posición fija para el balón al inicio
        transform.position = fixedBallPosition;

        GameObject balon = GameObject.FindWithTag(balonTag);
        if (balon != null)
        {
            targetObject = balon.transform;
            isFollowingBall = true;
        }
        else
        {
            Debug.LogError("No se encontró un objeto con el tag 'BalonCancha'. Asegúrate de que existe en la escena.");
        }
    }

    private void LateUpdate()
    {
        if (targetObject != null)
        {
            if (isFollowingBall)
            {
                // Actualiza solo la posición en X para seguir al balón horizontalmente, manteniendo Y y Z fijos
                Vector3 targetPosition = new Vector3(targetObject.position.x, fixedBallPosition.y, fixedBallPosition.z);
                Vector3 smoothedPosition = Vector3.Lerp(transform.position, targetPosition, smoothSpeed);
                transform.position = smoothedPosition;
            }
            else
            {
                // Si sigue a un jugador, usa un offset más cercano con suavizado
                Vector3 desiredPosition = targetObject.position + playerOffset;
                Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
                transform.position = smoothedPosition;
            }

            // Siempre mira al objetivo (ya sea el balón o el jugador)
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
            isFollowingBall = false;
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
            isFollowingBall = false;
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
        isFollowingBall = true;
        Debug.Log("La cámara ahora sigue al balón: " + balon.name);
    }
}

