using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformTrigger : MonoBehaviour
{

    /// <summary>
    /// Top-level script for launcher arm
    /// </summary>
    [SerializeField]
    Launcher Launcher;

    // Start is called before the first frame update
    void Start()
    {
        Launcher = GetComponentInParent<Launcher>();   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer.Equals(9)) Launcher.ShootBall(other);
    }
}
