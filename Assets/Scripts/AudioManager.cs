using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource audiosrc;

    public float musicVolume = 1f;

    private GameObject[] enemyAudSrc;
    private GameObject playerAudSrc;



    // Start is called before the first frame update
    void Start()
    {
        audiosrc = GetComponent<AudioSource>();
        enemyAudSrc = GameObject.FindGameObjectsWithTag("Enemy");
        playerAudSrc = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        audiosrc.volume = musicVolume;
        for (int i = 0; i < enemyAudSrc.Length; i++)
        {
            enemyAudSrc[i].GetComponent<Skeleton>().setVolume(musicVolume);
        }
        playerAudSrc.GetComponent<PlayerController>().setVolume(musicVolume);
        playerAudSrc.GetComponent<PlayerAttack>().setVolume(musicVolume);
    }

    public void setVolume(float vol)
    {
        musicVolume = vol;
    }

    public void Mute()
    {
        if(audiosrc.mute)
        {
            audiosrc.mute = false;
        }
        else
        {
            audiosrc.mute = true;
        }
        for (int i = 0; i < enemyAudSrc.Length; i++)
        {
            enemyAudSrc[i].GetComponent<Skeleton>().Mute();
        }
        playerAudSrc.GetComponent<PlayerController>().Mute();
        playerAudSrc.GetComponent<PlayerAttack>().Mute();
    }
}
