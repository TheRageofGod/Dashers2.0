using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseWall : MonoBehaviour
{
    public Color colour;
private void Start()
    {
        colour = GetComponent<SpriteRenderer>().color;
    }
    public void Phase()
    {
        GetComponent<BoxCollider2D>().isTrigger = true;
        GetComponent<SpriteRenderer>().color = Color.white;
        StartCoroutine(WaitTime());
    }

    IEnumerator WaitTime()
    {
        yield return new WaitForSeconds(1f);
        GetComponent<BoxCollider2D>().isTrigger = false;
        GetComponent<SpriteRenderer>().color = colour;
        StopCoroutine(WaitTime());
        

    }

    //loosley based of Design and Deploy's video found here: https://www.youtube.com/watch?v=0dcANRNI9a0
}
