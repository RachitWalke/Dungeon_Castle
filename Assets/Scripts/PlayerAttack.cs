using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private float timeBtwAttack;
    public float startTimeBtwAttack;

    public Transform attackPos;
    public float attackRange;
    public LayerMask whatIsEnemy;
    public int damage;
    public bool isAttack = false;

    private Animator anim;

    //audio
    public AudioSource audiosrc;
    public AudioClip clip;
    public AudioClip clip2;
    private void Start()
    {
        anim = GetComponent<Animator>();
        audiosrc = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (timeBtwAttack <= 0)
        {
            if (Input.GetMouseButtonDown(0))
            {
                anim.SetBool("isAttacking", true);
                Collider2D[] enemiesToHit = Physics2D.OverlapCircleAll(attackPos.position, attackRange, whatIsEnemy);
                for (int i = 0; i < enemiesToHit.Length; i++)
                {
                    enemiesToHit[i].GetComponent<EnemyBehaviour>().TakeDamage(damage);
                    audiosrc.PlayOneShot(clip2);
                }
                timeBtwAttack = startTimeBtwAttack;
                audiosrc.PlayOneShot(clip);
            }
            else
            {
                anim.SetBool("isAttacking", false);
            }
        }
        else
        {
            timeBtwAttack -= Time.deltaTime;
        }
        isAttack = anim.GetBool("isAttacking");
    }

    public void setVolume(float musicvol)
    {
        audiosrc.volume = musicvol;
    }

    public void Mute()
    {
        if (audiosrc.mute)
        {
            audiosrc.mute = false;
        }
        else
        {
            audiosrc.mute = true;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }
}
