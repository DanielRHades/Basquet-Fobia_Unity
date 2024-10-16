using UnityEngine;

public class FollowObjectOnXAxis : MonoBehaviour
{
    // Distancia fija en el eje Y y Z
    public float fixedYPosition;
    public float fixedZPosition;

    // Nombre del tag del objeto que la c�mara debe seguir
    private string targetTag = "BalonCancha";
    private Transform targetObject;

    void Start()
    {
        // Encuentra el objeto con el tag especificado
        GameObject targetGameObject = GameObject.FindWithTag(targetTag);

        if (targetGameObject != null)
        {
            targetObject = targetGameObject.transform;

            // Configura la posici�n inicial de la c�mara
            Vector3 initialPosition = transform.position;
            fixedYPosition = initialPosition.y;
            fixedZPosition = initialPosition.z;
        }
        else
        {
            Debug.LogError("No se encontr� un objeto con el tag 'BalonCancha'. Aseg�rate de que existe en la escena.");
        }
    }

    void Update()
    {
        if (targetObject != null)
        {
            // Actualiza la posici�n de la c�mara en el eje X del objeto seguido
            Vector3 newPosition = new Vector3(targetObject.position.x, fixedYPosition, fixedZPosition);
            transform.position = newPosition;
        }
    }
}
