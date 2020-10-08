using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatchersMitt : MonoBehaviour
{
    [SerializeField]
    GameObject Parent;

    [SerializeField]
    GameObject LeftHandAnchor;

    Ball CaughtBall;

    bool TriggerIsDown;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger)) TriggerIsDown = true;
        if (OVRInput.GetUp(OVRInput.Button.PrimaryHandTrigger)) TriggerIsDown = false;


        Parent.transform.SetPositionAndRotation(LeftHandAnchor.transform.position, LeftHandAnchor.transform.rotation);

        if (CaughtBall && TriggerIsDown && (CaughtBall.GetState() == Ball.EBallState.HELDBYCATCHER || CaughtBall.GetState() == Ball.EBallState.ATTRACTTOPLAYER))
        {
            CaughtBall.transform.position = transform.position;
        }
        else if (!TriggerIsDown && CaughtBall)
        {
            CaughtBall.SetState(Ball.EBallState.FREE);

            CaughtBall.SetVelocity(OVRInput.GetLocalControllerVelocity(OVRInput.Controller.LTouch) * 2);

            CaughtBall = null;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer.Equals(9) && TriggerIsDown && !CaughtBall)
        {
            CaughtBall = other.GetComponent<Ball>();
            CaughtBall.SetState(Ball.EBallState.HELDBYCATCHER);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer.Equals(9) && other.gameObject == CaughtBall.gameObject && TriggerIsDown)
        {
            TriggerIsDown = false;
            CaughtBall = null;
        }
    }
}
