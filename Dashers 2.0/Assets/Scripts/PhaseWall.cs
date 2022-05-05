using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseWall : MonoBehaviour
{
    public GameObject wall;
    public void Phase()
    {
        wall.GetComponent<Collider2D>().enabled = false;
        StartCoroutine(Wait());
       // wall.GetComponent<Collider2D>().enabled = true;
    }
 
    IEnumerator Wait()
    {
        yield return new WaitForSeconds(1);
    }
}
