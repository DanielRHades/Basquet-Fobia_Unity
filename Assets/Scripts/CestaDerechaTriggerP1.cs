using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class CestaDerechaTriggerP1 : MonoBehaviour
{
    private int contadorPuntosPlayer1 = 0;
    private const int puntosParaGanar = 12;

    // Referencia al TextMeshPro en pantalla para Player 1
    public TextMeshProUGUI puntosPlayer1Text;

    private void Start()
    {
        ActualizarTextoPuntos();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BalonCancha"))
        {
            Debug.Log("Balón detectado en PuntoPlayer1. Tag del balón: " + other.tag);
            contadorPuntosPlayer1 += 2;
            Debug.Log("Puntos Jugador 1: " + contadorPuntosPlayer1);

            ActualizarTextoPuntos();

            if (contadorPuntosPlayer1 >= puntosParaGanar)
            {
                CambiarAEscenaGanador();
            }
        }
    }

    private void ActualizarTextoPuntos()
    {
        if (puntosPlayer1Text != null)
        {
            puntosPlayer1Text.text = "PUNTOS P1: " + contadorPuntosPlayer1.ToString();
        }
    }

    private void CambiarAEscenaGanador()
    {
        SceneManager.LoadScene(0);
    }
}
