using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    public void GoToSelectPlayer1()
    {
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }

}
