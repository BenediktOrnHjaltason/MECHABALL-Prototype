using System.Collections;
using System.Collections.Generic;
using System.Security.Policy;
using TMPro;
using UnityEngine;

public class Launcher : MonoBehaviour
{

    [SerializeField]
    private GameObject LauncherBase;

    [SerializeField]
    private GameObject Platform;

    [SerializeField]
    private Component RightHandAnchor;

    [SerializeField]
    private TextMeshPro ChargeIndicator;

    Vector3 BaseOrigin = new Vector3(0, 0, 0);
    Vector3 inner = new Vector3(0.01097f, 0.0f, 0.0f);
    Vector3 outer = new Vector3(0.01592f, 0.0f, 0.0f);
    float ShakeOffsett = 0.0f;

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


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = RightHandAnchor.transform.position;
        transform.rotation = RightHandAnchor.transform.rotation;


        if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger)) IsCharging = true;

        if (OVRInput.GetUp(OVRInput.Button.SecondaryIndexTrigger))
        {

            ChargeAtRelease = ChargeLevel;

            Debug.Log("ChargeLevel at release time");

            IsCharging = false;
            JustReleasedCharge = true;

            LauncherBase.transform.localPosition = BaseOrigin;

            ChargeLevel = 0;
            ReleaseStopTime = Time.time + ReleaseDuration;
            Platform.transform.localPosition = outer;

        }

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


    public void ShootBall(Collider other)
    {
        other.GetComponent<Ball>().Shoot(transform.forward * (100.0f + (ChargeAtRelease * 2000)));
    }
}
