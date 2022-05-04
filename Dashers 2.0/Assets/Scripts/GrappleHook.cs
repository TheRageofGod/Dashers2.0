using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleHook : MonoBehaviour
{
    LineRenderer line;
    [SerializeField] LayerMask GrappleableMask;
    [SerializeField] float maxDist = 10f;
    [SerializeField] float grappleSpeed = 10f;
    [SerializeField] float grappleShootSpeed = 20f;

    bool isGrappling = false;
    [HideInInspector] public bool retracting = false;


    Vector2 target;

    private void Start()
    {
        line = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isGrappling)
        {
            StartGrapple();
        }
        if (retracting)// this starts moving the player towards the grapple point
        {
            Vector2 grapplePos = Vector2.Lerp(transform.position, target, grappleSpeed * Time.deltaTime);

            transform.position = grapplePos;
            line.SetPosition(0, transform.position);

            if(Vector2.Distance(transform.position,target) < 1f) 
            {
                retracting = false;
                isGrappling = false;
                line.enabled = false;
            }
        }
    }

    private void StartGrapple() // this is to tell the system that we have actually hit something within range
    {
        Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, maxDist, GrappleableMask);

        if (hit.collider != null)
        {
            isGrappling = true;
            target = hit.point;
            line.enabled = true;
            line.positionCount = 2;

            StartCoroutine(Grapple());
        }

        IEnumerator Grapple() //this tell the system where we have hiot and how far everything is
        {
            float t = 0;
            float time = 10;
            line.SetPosition(0, transform.position);
            line.SetPosition(1, transform.position);

            Vector2 newPos;
            for(; t < time; t += grappleShootSpeed * Time.deltaTime)
            {
                newPos = Vector2.Lerp(transform.position, target, t / time);
                line.SetPosition(0, transform.position);
                line.SetPosition(1, newPos);
                yield return null;
            }
            line.SetPosition(1, target);
            retracting = true;
        }
    }
}
