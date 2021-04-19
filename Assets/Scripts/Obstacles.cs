using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacles : MonoBehaviour
{
    //for fireballs
    public Transform[] fireBalls;
    public Transform[] FBspwanPoints;
    private bool blueBall;
    private bool GreenBall;

    private EnemySpwaner enemySpwaner;

    // Start is called before the first frame update
    void Start()
    {
        enemySpwaner = GameObject.FindObjectOfType<EnemySpwaner>();
    }

    // Update is called once per frame
    void Update()
    {
        enemySpwaner = GameObject.FindObjectOfType<EnemySpwaner>();
        int i = Random.Range(0, 1);
        if(i==0)
        {
            blueBall = true;
        }
        else if(i == 1)
        {
            GreenBall = true;
        }
        if(enemySpwaner.releaseFireBalls())
        {
           if(blueBall)
            {
                spwanFireBalls(fireBalls[0]);
                blueBall = false;
            }
            if (GreenBall)
            {
                spwanFireBalls(fireBalls[1]);
                GreenBall = false;
            }


        }
    }

    void spwanFireBalls(Transform _fireball)
    {
        Transform _sp = FBspwanPoints[Random.Range(0, FBspwanPoints.Length)];
        Instantiate(_fireball, _sp.position, Quaternion.identity);
    }
}
