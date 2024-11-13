using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class StartGame : MonoBehaviour
{   
    void Update()
    {
        if(Gamepad.all[0].startButton.wasPressedThisFrame || Gamepad.all[1].startButton.wasPressedThisFrame){
           GoToControl(); 
        }
    }
    
    public void GoToControl()
    {   
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }

}
