using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class CestaIzquierdaTriggerP2 : MonoBehaviour
{
    private int contadorPuntosPlayer2 = 0;
    private const int puntosParaGanar = 12;

    // Referencia al TextMeshPro en pantalla para Player 2
    public TextMeshProUGUI puntosPlayer2Text;

    private void Start()
    {
        ActualizarTextoPuntos();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BalonCancha"))
        {
            Debug.Log("Balón detectado en PuntoPlayer2. Tag del balón: " + other.tag);
            contadorPuntosPlayer2 += 2;
            Debug.Log("Puntos Jugador 2: " + contadorPuntosPlayer2);

            ActualizarTextoPuntos();

            if (contadorPuntosPlayer2 >= puntosParaGanar)
            {
                CambiarAEscenaGanador();
            }
        }
    }

    private void ActualizarTextoPuntos()
    {
        if (puntosPlayer2Text != null)
        {
            puntosPlayer2Text.text = contadorPuntosPlayer2.ToString();
        }
    }

    private void CambiarAEscenaGanador()
    {
        SceneManager.LoadScene(0);
    }
}
