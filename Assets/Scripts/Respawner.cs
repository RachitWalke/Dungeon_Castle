using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawner : MonoBehaviour
{
    public Transform leftSpwan;
    public Transform RightSpwan;
         
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Enemy" && this.tag == "Respwanleft")
        {
            collision.gameObject.transform.position = RightSpwan.transform.position;
        }
        else if(collision.tag == "Enemy" && this.tag == "RespwanRight")

        {
            collision.gameObject.transform.position = leftSpwan.transform.position;
        }
    }
}
