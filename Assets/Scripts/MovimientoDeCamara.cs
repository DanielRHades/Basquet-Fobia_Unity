using UnityEngine;

public class FollowObjectOnXAxis : MonoBehaviour
{
    // Distancia fija en el eje Y y Z
    public float fixedYPosition;
    public float fixedZPosition;

    // Nombre del tag del objeto que la cámara debe seguir
    private string targetTag = "BalonCancha";
    private Transform targetObject;

    void Start()
    {
        // Encuentra el objeto con el tag especificado
        GameObject targetGameObject = GameObject.FindWithTag(targetTag);

        if (targetGameObject != null)
        {
            targetObject = targetGameObject.transform;

            // Configura la posición inicial de la cámara
            Vector3 initialPosition = transform.position;
            fixedYPosition = initialPosition.y;
            fixedZPosition = initialPosition.z;
        }
        else
        {
            Debug.LogError("No se encontró un objeto con el tag 'BalonCancha'. Asegúrate de que existe en la escena.");
        }
    }

    void Update()
    {
        if (targetObject != null)
        {
            // Actualiza la posición de la cámara en el eje X del objeto seguido
            Vector3 newPosition = new Vector3(targetObject.position.x, fixedYPosition, fixedZPosition);
            transform.position = newPosition;
        }
    }
}
