﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Ball : MonoBehaviour
{
    public enum EBallState
    {
        STARTHOVER,
        STARTSHOOTUP,
        ATTRACTTOPLAYER,
        HELDBYCATCHER,
        FREE
    }

    Rigidbody rb;

    EBallState state;

    Vector3 HoverCenter;

    bool RecentlyShot = false;

    float NextShot;



    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //if (state == EBallState.STARTHOVER) transform.position = HoverCenter + 
                                                              //new Vector3( 0, Mathf.Sin(Time.time * 5) / 10, 0);
    }

    public void SetState(EBallState newState)
    {
        switch (newState)
        {
            case EBallState.STARTHOVER: 
                {
                    rb.useGravity = false;
                    HoverCenter = transform.position;

                    state = newState;
                    break;
                }

            case EBallState.STARTSHOOTUP:
                {
                    rb.useGravity = true;
                    rb.AddForce(new Vector3(0, 1000, 0));

                    state = newState;
                    break;
                }

            case EBallState.FREE:
                {
                    
                    rb.useGravity = true;
                    rb.drag = 0;

                    state = newState;
                    break;
                }

            case EBallState.ATTRACTTOPLAYER:
            {
                    rb.useGravity = false;

                    state = newState;
                    break;
            }

            case EBallState.HELDBYCATCHER:
                {
                    rb.useGravity = false;
                    rb.velocity = new Vector3(0, 0, 0);

                    state = newState;
                    break;
                }
        }
    }

    public EBallState GetState()
    {
        return state;
    }

    public void Shoot(Vector3 Impulse)
    {
        
        //Because overlaps register multiple times per shot. TODO: Find smarte solution for this later
        if (Time.time > NextShot)
        {
            NextShot = Time.time + 0.2f;

            rb.useGravity = true;
            SetState(EBallState.FREE);
            rb.velocity = new Vector3(0, 0, 0);
            rb.AddForce(Impulse);

            //Debug.Log("Ball shot with impulse magnitude: " + Impulse.magnitude + " ChargeAtRelease: " + ChargeAtRelease);
        }
    }

    public void AttractToPlayer(Vector3 Force)
    {
        rb.AddForce(Force);
    }

    public void SetDrag(float drag)
    {
        rb.drag = drag;
    }

    public void SetVelocity(Vector3 velocity)
    {
        rb.velocity = velocity;
    }
}
