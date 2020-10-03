using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformTrigger : MonoBehaviour
{
    [SerializeField]
    Launcher ParentLauncherScript;

    // Start is called before the first frame update
    void Start()
    {
        ParentLauncherScript = GetComponentInParent<Launcher>();   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Colliding with something");
        ParentLauncherScript.ShootBall(other);
    }
}
