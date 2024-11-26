using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public AudioClip songMainMenu; // Canción para escenas 0-4
    public AudioClip songGameplay; // Canción para escena 5
    public AudioClip songWinner; // Canción para escena 6

    private AudioSource audioSource;

    private static MusicManager instance;

    void Awake()
    {
        // Asegurar que solo hay un MusicManager
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); // Si ya existe uno, destruye este duplicado
            return;
        }

        // Configuración del AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        audioSource.loop = true; // Reproduce en bucle
        PlayMusic(songMainMenu, 0.6f); // Inicia con la canción del menú
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Cambiar la música según la escena
        if (scene.buildIndex >= 0 && scene.buildIndex <= 3)
        {
            PlayMusic(songMainMenu, 0.6f);
        }
        else if (scene.buildIndex == 4)
        {
            PlayMusic(songGameplay, 0.6f);
        }
        else if (scene.buildIndex == 5)
        {
            PlayMusic(songWinner, 1.0f);
            StartCoroutine(ReturnToMainMenuAfterDelay(5f)); // Regresar al menú después de 5 segundos
        }
    }

    public void PlayMusic(AudioClip clip)
    {
        if (audioSource.clip != clip)
        {
            audioSource.Stop();
            audioSource.clip = clip;
            audioSource.Play();
        }
    }

    private System.Collections.IEnumerator ReturnToMainMenuAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // Esperar 5 segundos
        SceneManager.LoadScene(0); // Volver a la escena 0
    }
}
