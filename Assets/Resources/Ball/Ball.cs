using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Ball : MonoBehaviour
{
    public enum EBallState
    {
        STARTHOVER,
        STARTSHOOTUP,
        FREE

    }

    Rigidbody rb;

    [SerializeField]
    private EBallState stateFromEditor;
    

    EBallState state;

    Vector3 HoverCenter;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        SetState(stateFromEditor);
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

                    state = newState;
                    break;
                }
        }
    }

    public void Shoot(Vector3 Impulse)
    {
        if (state != EBallState.FREE)
        {
            Debug.Log("Shoot called for ball");

            SetState(EBallState.FREE);
            rb.AddForce(Impulse);
        }
    }
}
