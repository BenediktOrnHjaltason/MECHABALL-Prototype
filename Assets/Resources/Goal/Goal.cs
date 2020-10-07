using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    [SerializeField]
    Material Active;

    [SerializeField]
    Material Inactive;

    [SerializeField]
    GameObject Parent;

    MeshRenderer FrontTarget;

   

    float RotationIncrement;

    // Start is called before the first frame update
    void Start()
    {
        FrontTarget = GetComponentInChildren<MeshRenderer>();

        RotationIncrement = transform.rotation.eulerAngles.y;
    }

    // Update is called once per frame
    void Update()
    {
    //   Parent.transform.rotation = Quaternion.Euler(new Vector3(Parent.transform.rotation.eulerAngles.x, ++RotationIncrement, Parent.transform.rotation.eulerAngles.z));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer.Equals(9))
        {
            Debug.Log("Ball entered goal trigger. Destroyed ball");
            Destroy(other);
            
            

            FrontTarget.material = Inactive;
        }
    }
}
