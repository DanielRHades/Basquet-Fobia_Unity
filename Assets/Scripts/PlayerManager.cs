using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    public GameObject[] decoracionesPrefabs; // Array de prefabs tomando el jugador 1

    public GameObject[] characterPrefabsP1; // Array de prefabs para el jugador 1
    public GameObject[] characterPrefabsP2; // Array de prefabs para el jugador 2
    private void Start()
    {

        // Instanciar la decoración del jugador 1 en (0,0,0) con rotación de 180 grados sobre el eje Y
        int selectedDecoration1 = PlayerPrefs.GetInt("selectedCharacter1"); // Asegúrate de que esta clave esté definida en PlayerPrefs
        if (selectedDecoration1 >= 0 && selectedDecoration1 < decoracionesPrefabs.Length)
        {
            // Instancia la decoración en (0,0,0) con rotación de 180 grados sobre el eje Y
            Instantiate(decoracionesPrefabs[selectedDecoration1], Vector3.zero, Quaternion.Euler(0, 180, 0));
        }

        PlayerInput.Instantiate(characterPrefabsP1[PlayerPrefs.GetInt("selectedCharacter1")], 0, "Gamepad", 0, Gamepad.all[0]);
        
        PlayerInput.Instantiate(characterPrefabsP2[PlayerPrefs.GetInt("selectedCharacter2")], 1, "Gamepad", 0, Gamepad.all[1]);
        
    }
}
