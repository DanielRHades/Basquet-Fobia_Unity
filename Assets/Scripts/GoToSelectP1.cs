using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class GoToSelectP1 : MonoBehaviour
{
    void Update()
    {
        if(Gamepad.all[0].startButton.wasPressedThisFrame || Gamepad.all[1].startButton.wasPressedThisFrame){
           GoToSelectPlayer1(); 
        }
    }
    
    public void GoToSelectPlayer1()
    {   
        SceneManager.LoadScene(2, LoadSceneMode.Single);
    }

}
