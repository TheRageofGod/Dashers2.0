using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Hazards : MonoBehaviour
{
    public void Reload(GameObject gameObject)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }    
}
