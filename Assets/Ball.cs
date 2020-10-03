using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{

    public enum States
    {
        STARTHOVER,
        FREE
    }

    Rigidbody rb;

    [HideInInspector]
    public States states;
    Vector3 StartPosition;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        states = States.STARTHOVER;

        if (states == States.STARTHOVER)
        {
            rb.useGravity = false;
            StartPosition = transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (states == States.STARTHOVER) transform.position = StartPosition + 
                                                              new Vector3( 0, Mathf.Sin(Time.time * 5) / 10, 0);

    }
}
