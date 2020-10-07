using System.Collections;
using System.Collections.Generic;
using System.Security.Policy;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Launcher : MonoBehaviour
{

    enum EAttractState
    {
        LOCKING,
        ATTRACTING,
        NONE
    }


    [SerializeField]
    private GameObject LauncherBase;

    [SerializeField]
    private GameObject Platform;

    [SerializeField]
    private Component RightHandAnchor;

    [SerializeField]
    private TextMeshPro ChargeIndicator;


    //Launch platform positions
    Vector3 inner = new Vector3(0.01097f, 0.0f, 0.0f);
    Vector3 outer = new Vector3(0.01592f, 0.0f, 0.0f);



    //Arm shake on charge
    Vector3 BaseOrigin = new Vector3(0, 0, 0);
    float ShakeOffsett = 0.0f;


    //---- Charging ----//

    bool IsCharging = false;
    bool JustReleasedCharge = false;

    //The value used to increment charge and display on launcher
    float ChargeLevel = 0;

    //The value that is actually used in the impulse vector calculation
    float ChargeAtRelease = 0;

    //The time when window of adding charge power is over
    float ReleaseStopTime = 0;

    //Platform is placed on outer position instantaneously when charge is released. 
    //Release duration is to fake an actual forward thrust that takes time.
    const float ReleaseDuration = 0.2f;

    //---/Charging ----//

    //---- Attracting ball ----//
    EAttractState AttractState;
    RaycastHit HitBall;
    Ball LockedBall;

    float DistanceToBall;
    float Distance_ThreeQuarterToBall;

    Vector3 AimPoint;
    Vector3 BallToAimPoint;

    Vector3 LauncherFront;
    Vector3 BallToLauncerFront;

    float PowerScalar = 5.0f;





    //Drawing lines for visualizing aiming and attracting ball
    LineRenderer Line;

    // Start is called before the first frame update
    void Start()
    {
        AttractState = EAttractState.NONE;
        Line = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

        transform.SetPositionAndRotation(RightHandAnchor.transform.position, RightHandAnchor.transform.rotation);

        //Trigger button
        if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger)) IsCharging = true;

        if (OVRInput.GetUp(OVRInput.Button.SecondaryIndexTrigger))
        {
            Debug.Log("OnTriggerRelease | ChargeLevel: " + ChargeLevel);

            ChargeAtRelease = ChargeLevel;
            ChargeLevel = 0;



            IsCharging = false;
            JustReleasedCharge = true;

            LauncherBase.transform.localPosition = BaseOrigin;

            
            ReleaseStopTime = Time.time + ReleaseDuration;
            Platform.transform.localPosition = outer;

            AttractState = EAttractState.NONE;
            LockedBall = null;

        }

        //------------------Attract

        //Grip button
        if (OVRInput.GetDown(OVRInput.Button.SecondaryHandTrigger)) AttractState = EAttractState.LOCKING;


        if (OVRInput.GetUp(OVRInput.Button.SecondaryHandTrigger)) AttractState = EAttractState.NONE;

        //Look for lock hit
        if (AttractState == EAttractState.LOCKING)
        {
            //Draw line to aim
            Line.enabled = true;
            Line.SetPosition(0, transform.position);
            Line.SetPosition(1, transform.position + transform.forward * 700);

            //If locked on ball
            if (Physics.Raycast(transform.position, transform.forward, out HitBall, Mathf.Infinity, 1 << 11))
            {
                //Ball
                LockedBall = HitBall.transform.GetComponentInParent<Ball>();
                LockedBall.SetState(Ball.EBallState.ATTRACTTOPLAYER);

                //Launcher
                AttractState = EAttractState.ATTRACTING;
                Line.enabled = false;
            }
        }

        else if (AttractState == EAttractState.ATTRACTING && LockedBall)
        {
            DistanceToBall = (LockedBall.transform.position - transform.position).magnitude;
            Distance_ThreeQuarterToBall = DistanceToBall / 1.5f;
            AimPoint = transform.position + transform.forward * Distance_ThreeQuarterToBall;
            BallToAimPoint = AimPoint - LockedBall.transform.position;

            LauncherFront = transform.position + transform.forward * 5;

            BallToLauncerFront = LauncherFront - LockedBall.transform.position;

            DistanceToBall = (LauncherFront - LockedBall.transform.position).magnitude;

            //Draw visualization
            Line.SetPosition(0, LockedBall.transform.position);
            Line.SetPosition(1, AimPoint);
            Line.enabled = true;

            //LockedBall.AttractToPlayer((transform.position - LockedBall.transform.position).normalized * 10);
            //

            if (DistanceToBall > 20)
            {
                LockedBall.AttractToPlayer((BallToAimPoint + BallToLauncerFront).normalized * 10);
            }

            else
            {
                LockedBall.SetDrag(5 / DistanceToBall);
                LockedBall.AttractToPlayer(BallToAimPoint.normalized * 10);
            }

            
        }

        else if (AttractState == EAttractState.NONE)
        {
            if(LockedBall)
            {
                LockedBall.SetState(Ball.EBallState.FREE);
                LockedBall = null;
            }

            Line.enabled = false;
        }




        //------------------/Attract


        //------------------Launch

        if (IsCharging)
         {
            if (ChargeLevel < 0.99) ChargeLevel += 0.01f;


            //Set charge shake whenever its charging
            ShakeOffsett = (Mathf.Sin(Time.time * 1000) * ChargeLevel) / 70;
            LauncherBase.transform.localPosition = new Vector3(ShakeOffsett,
                                                                -ShakeOffsett,
                                                                0.0f);
            Platform.transform.localPosition = Vector3.Lerp(outer, inner, ChargeLevel);
         }

        else 
        {
            //Handle release time
            if (JustReleasedCharge && Time.time > ReleaseStopTime)
            {
                ChargeAtRelease = 0;
                JustReleasedCharge = false;
            }
        }

        ChargeIndicator.text = ((ChargeLevel*100) + 0.01f).ToString("F0")  + "%";
    }

    //------------------/Launch

    public void ShootBall(Collider other)
    {
        other.GetComponent<Ball>().Shoot(transform.forward * (100.0f + (ChargeAtRelease * 2000)));
    }
}
