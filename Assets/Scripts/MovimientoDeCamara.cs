using UnityEngine;

public class MovimientoDeCamara : MonoBehaviour
{
    public float fixedYPosition;
    public float fixedZPosition;

    private Transform targetObject; // El objeto que la cámara sigue dinámicamente
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
    }

    private void Update()
    {
        if (targetObject != null)
        {
            Vector3 newPosition = new Vector3(targetObject.position.x, fixedYPosition, fixedZPosition);
            transform.position = newPosition;
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
