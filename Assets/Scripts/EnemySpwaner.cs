using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpwaner : MonoBehaviour
{
    public enum spwanState { Spwaning,Waiting,Counting};

    [System.Serializable]
    public class Wave
    {
        public string Name;
        public Transform enemy;
        public int count;
        public float rate;
    }

    public Wave[] waves;
    private int nextwave = 0;

    public float timeBetweenWaves = 5f;
    public float waveCountDown;

    private spwanState state = spwanState.Counting;
    // Start is called before the first frame update
    void Start()
    {
        waveCountDown = timeBetweenWaves; 
    }

    // Update is called once per frame
    void Update()
    {
        if(waveCountDown <= 0)
        {
            if (state != spwanState.Spwaning)
            {
                StartCoroutine(spwanWave(waves[nextwave]));
            }
            else
            {
                waveCountDown -= Time.deltaTime;
            }
        }
    }


    IEnumerator spwanWave(Wave _wave)
    {
        state = spwanState.Spwaning;
        for(int i = 0;i < _wave.count;i++)
        {
            spwanEnemy(_wave.enemy);
            yield return new WaitForSeconds(1f / _wave.rate);
        }
        state = spwanState.Waiting;

        yield break;
    }

    void spwanEnemy(Transform _enemy)
    {

    }

}
