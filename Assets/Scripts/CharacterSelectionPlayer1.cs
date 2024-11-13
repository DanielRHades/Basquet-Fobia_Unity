using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class CharacterSelectionPlayer1 : MonoBehaviour
{
    public GameObject[] characters; // Array de prefabs del jugador 1
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
        // Comprobar si hay al menos un gamepad
        if (Gamepad.all.Count > 0)
        {
            // Leer las entradas de la cruceta
            if (Gamepad.all[0].dpad.right.wasPressedThisFrame) // Derecha
            {
                NextCharacter();
            }
            else if (Gamepad.all[0].dpad.left.wasPressedThisFrame) // Izquierda
            {
                PreviousCharacter();
            }

            // Al presionar el botón "South", ir a la siguiente escena
            if (Gamepad.all[0].startButton.wasPressedThisFrame)
            {
                GoSecondPlayerScene();
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

    public void GoSecondPlayerScene()
    {
        PlayerPrefs.SetInt("selectedCharacter1", selectedCharacter); // Guardar personaje del jugador 1
        SceneManager.LoadScene(3, LoadSceneMode.Single);
    }
}
