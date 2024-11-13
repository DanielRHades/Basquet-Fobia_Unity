using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class CharacterSelectionPlayer2 : MonoBehaviour
{
    public GameObject[] characters; // Array de prefabs del jugador 2
    public Transform spawnPoint; // Posición de visualización
    private int selectedCharacter = 0; // Índice del personaje seleccionado

    private void Start()
    {
        // Desactivar PlayerInput en cada prefab
        foreach (var character in characters)
        {
            var playerInput = character.GetComponent<PlayerInput>();
            if (playerInput != null)
            {
                playerInput.enabled = false; // Desactivar PlayerInput
            }
        }

        UpdateCharacter();
    }

    private void Update()
    {
        // Comprobar si hay al menos dos gamepads
        if (Gamepad.all.Count > 1)
        {
            // Leer las entradas de la cruceta
            if (Gamepad.all[1].dpad.right.wasPressedThisFrame) // Derecha
            {
                NextCharacter();
            }
            else if (Gamepad.all[1].dpad.left.wasPressedThisFrame) // Izquierda
            {
                PreviousCharacter();
            }

            // Al presionar el botón "South", ir a la siguiente escena
            if (Gamepad.all[1].startButton.wasPressedThisFrame)
            {
                StartGame();
            }
        }
    }

    public void NextCharacter()
    {
        characters[selectedCharacter].SetActive(false);
        selectedCharacter = (selectedCharacter + 1) % characters.Length;
        UpdateCharacter();
    }

    public void PreviousCharacter()
    {
        characters[selectedCharacter].SetActive(false);
        selectedCharacter--;
        if (selectedCharacter < 0)
        {
            selectedCharacter += characters.Length;
        }
        UpdateCharacter();
    }

    private void UpdateCharacter()
    {
        foreach (var character in characters)
        {
            character.SetActive(false);
        }
        characters[selectedCharacter].SetActive(true);
        characters[selectedCharacter].transform.position = spawnPoint.position;
        characters[selectedCharacter].transform.rotation = spawnPoint.rotation;
    }

    public void StartGame()
    {
        PlayerPrefs.SetInt("selectedCharacter2", selectedCharacter); // Guardar personaje del jugador 2
        SceneManager.LoadScene(4, LoadSceneMode.Single);
    }
}
