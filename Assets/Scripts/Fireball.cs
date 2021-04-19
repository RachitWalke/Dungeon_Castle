using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    Rigidbody2D rb;
    private float ballSpeed = 2.0f;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = new Vector2(-1, rb.velocity.y) * ballSpeed;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GameManager.instance.playerLives--;
            GameManager.instance.takeDamage(1);
            Destroy(collision.gameObject);
            Destroy(this.gameObject);
            GameManager.instance.spwanPlayer();
        }
    }
}
