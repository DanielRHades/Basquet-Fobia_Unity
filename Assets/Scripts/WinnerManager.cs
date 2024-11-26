using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WinnerManager : MonoBehaviour
{
    public Image zombi1GanaContraZombi2;
    public Image zombi1GanaContraMomia2;
    public Image momia1GanaContraZombi2;
    public Image momia1GanaContraMomia2;
    public Image zombi2GanaContraZombi1;
    public Image zombi2GanaContraMomia1;
    public Image momia2GanaContraZombi1;
    public Image momia2GanaContraMomia1;

    private Image[] images;

    void Start()
    {
        images = new Image[]
        {
            zombi1GanaContraZombi2, zombi1GanaContraMomia2, momia1GanaContraZombi2, momia1GanaContraMomia2,
            zombi2GanaContraZombi1, zombi2GanaContraMomia1, momia2GanaContraZombi1, momia2GanaContraMomia1
        };

        OcultarTodasLasImagenes();

        // Carga los datos desde PlayerPrefs
        int p1 = PlayerPrefs.GetInt("selectedCharacter1", 0); // Valor por defecto: 0 (Zombi)
        int p2 = PlayerPrefs.GetInt("selectedCharacter2", 0); // Valor por defecto: 0 (Zombi)
        int ganador = PlayerPrefs.GetInt("WinnerPlayer", 0);  // Valor por defecto: 0 (P1 gana)

        MostrarGanador(p1, p2, ganador);

    }

    void MostrarGanador(int p1, int p2, int ganador)
    {
        OcultarTodasLasImagenes();

        if (p1 == 0 && p2 == 0 && ganador == 0)
        {
            zombi1GanaContraZombi2.gameObject.SetActive(true);
        }
        else if (p1 == 0 && p2 == 1 && ganador == 0)
        {
            zombi1GanaContraMomia2.gameObject.SetActive(true);
        }
        else if (p1 == 1 && p2 == 0 && ganador == 0)
        {
            momia1GanaContraZombi2.gameObject.SetActive(true);
        }
        else if (p1 == 1 && p2 == 1 && ganador == 0)
        {
            momia1GanaContraMomia2.gameObject.SetActive(true);
        }
        else if (p1 == 0 && p2 == 0 && ganador == 1)
        {
            zombi2GanaContraZombi1.gameObject.SetActive(true);
        }
        else if (p1 == 0 && p2 == 1 && ganador == 1)
        {
            zombi2GanaContraMomia1.gameObject.SetActive(true);
        }
        else if (p1 == 1 && p2 == 0 && ganador == 1)
        {
            momia2GanaContraZombi1.gameObject.SetActive(true);
        }
        else if (p1 == 1 && p2 == 1 && ganador == 1)
        {
            momia2GanaContraMomia1.gameObject.SetActive(true);
        }
        else
        {
            Debug.Log("Resultado no válido");
        }
    }

    void OcultarTodasLasImagenes()
    {
        foreach (var img in images)
        {
            img.gameObject.SetActive(false);
        }
    }
}