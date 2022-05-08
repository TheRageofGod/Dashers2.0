using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseWall : MonoBehaviour
{
    public Color colour;
private void Start()
    {
        colour = GetComponent<SpriteRenderer>().color; // gets objects current colour
    }
    public void Phase()
    {
        GetComponent<BoxCollider2D>().isTrigger = true;//sets collider to trigger alowing the player to pas through
        GetComponent<SpriteRenderer>().color = Color.white;//changes the colour to indicate phasing
        StartCoroutine(WaitTime());//starts corotutine that returns everything to normal
    }

    IEnumerator WaitTime()
    {
        yield return new WaitForSeconds(1f);//makes the code wait
        GetComponent<BoxCollider2D>().isTrigger = false;//turns the trigger function off
        GetComponent<SpriteRenderer>().color = colour;// turn the colour back to normal
        StopCoroutine(WaitTime());// stop the coroutine so that the phasing can be done more that once
        

    }

    //loosley based of Design and Deploy's video found here: https://www.youtube.com/watch?v=0dcANRNI9a0
}
