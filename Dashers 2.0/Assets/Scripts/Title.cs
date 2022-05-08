using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;


public class Title : MonoBehaviour
{
    public void StartGame() //starts game
    {
        SceneManager.LoadScene("MainLevel");
        
    }

    public void QuitGame() // closes application
    {
        Application.Quit();
    }
}
