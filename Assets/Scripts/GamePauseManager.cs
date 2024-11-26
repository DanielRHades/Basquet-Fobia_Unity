using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GamePauseManager : MonoBehaviour
{
    public GameObject canvasPausa; // Canvas que se activa al pausar el juego
    private bool isPaused = false;

    // Referencias a los scripts de puntaje
    public CestaDerechaTriggerP1 cestaDerechaTriggerP1;
    public CestaIzquierdaTriggerP2 cestaIzquierdaTriggerP2;

    private int puntosGuardadosPlayer1 = 0;
    private int puntosGuardadosPlayer2 = 0;

    private void Start()
    {
        // Asegurarse de que el Canvas de pausa esté desactivado al iniciar el juego
        if (canvasPausa != null)
        {
            canvasPausa.SetActive(false);
        }
    }

    private void Update()
    {
        // Escucha la tecla "P" para pausar o reanudar el juego
        if (Gamepad.all[0].startButton.wasPressedThisFrame || Gamepad.all[1].startButton.wasPressedThisFrame)
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        if (canvasPausa != null)
        {
            // Guardar los puntajes actuales
            puntosGuardadosPlayer1 = cestaDerechaTriggerP1 != null ? cestaDerechaTriggerP1.GetPuntosPlayer1() : 0;
            puntosGuardadosPlayer2 = cestaIzquierdaTriggerP2 != null ? cestaIzquierdaTriggerP2.GetPuntosPlayer2() : 0;

            // Pausar el juego
            Time.timeScale = 0;
            isPaused = true;

            // Activar el Canvas de pausa
            canvasPausa.SetActive(true);
        }
    }

    public void ResumeGame()
    {
        if (canvasPausa != null)
        {
            // Reanudar el juego
            Time.timeScale = 1;
            isPaused = false;

            // Desactivar el Canvas de pausa
            canvasPausa.SetActive(false);
        }
    }

    public int GetPuntosGuardadosPlayer1()
    {
        return puntosGuardadosPlayer1;
    }

    public int GetPuntosGuardadosPlayer2()
    {
        return puntosGuardadosPlayer2;
    }
}
