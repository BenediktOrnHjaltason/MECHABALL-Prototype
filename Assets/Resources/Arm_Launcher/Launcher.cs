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

    private Vector3 BaseOrigin = new Vector3(0, 0, 0);
    private Vector3 inner = new Vector3(0.01097f, 0.0f, 0.0f);
    private Vector3 outer = new Vector3(0.01592f, 0.0f, 0.0f);
    private float ShakeOffsett = 0.0f;


    private bool IsCharging = false;

    private float ChargeLevel = 0;
    private float padding = 0.01f;



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
        if (OVRInput.GetUp(OVRInput.Button.SecondaryIndexTrigger))   IsCharging = false;

        if (IsCharging)
        {
            if (ChargeLevel < 0.99) ChargeLevel += 0.01f;


            //Set charge shake whenever its charged
            ShakeOffsett = (Mathf.Sin(Time.time * 1000) * ChargeLevel) / 70;
            LauncherBase.transform.localPosition = new Vector3(ShakeOffsett,
                                                               -ShakeOffsett,
                                                               0.0f);
            Platform.transform.localPosition = Vector3.Lerp(outer, inner, ChargeLevel);
        }

        else
        {
            ChargeLevel = 0;
            LauncherBase.transform.localPosition = BaseOrigin;
            Platform.transform.localPosition = outer;
        }

        ChargeIndicator.text = ((ChargeLevel*100) + 0.01f).ToString("F0")  + "%";
    }


    public void ShootBall(Collider other)
    {
        Rigidbody rb = other.attachedRigidbody;
        other.GetComponent<Ball>().states = Ball.States.FREE;
        rb.useGravity = true;

        rb.AddForce(transform.forward * (500.0f + (ChargeLevel * 1000)));
    }
}
