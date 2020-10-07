using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingShooter : MonoBehaviour
{

    [SerializeField]
    GameObject Sphere;

    [SerializeField]
    GameObject BallToSpawn;

    float TimeToShoot = 5;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > TimeToShoot)
        {
            TimeToShoot += 5;

            Ball ball = Instantiate(BallToSpawn, Sphere.transform.position + Sphere.transform.forward * 2, Sphere.transform.rotation).GetComponent<Ball>();
            ball.SetState(Ball.EBallState.FREE);
            ball.Shoot(Sphere.transform.forward * 700);
        }
    }
}
