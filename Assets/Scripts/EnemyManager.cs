using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    private float dirX =1.0f;
    public float enemySpeed;
    private bool isStaggered = false;

    Renderer[] renderers;
    bool isWrappingX = false;
    bool isWrappingY = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        renderers = GetComponentsInChildren<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isStaggered)
            rb.velocity = new Vector2(0f,0f);
        else if(!isStaggered)
            rb.velocity = new Vector2(dirX, rb.velocity.y) * enemySpeed;

        ScreenWrap();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            dirX = -dirX;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            Debug.Log("collision");
            anim.SetBool("isStagger", true);
            isStaggered = true;
        }
    }

    bool CheckRenderers()
    {
        foreach (Renderer renderer in renderers)
        {
            // If at least one render is visible, return true
            if (renderer.isVisible)
            {
                Debug.Log("Object is visible");
                return true;
            }
            else
                Debug.Log("Object is not visible");
        }

        // Otherwise, the object is invisible
        return false;
    }
    public void ScreenWrap()
    {
        bool isVisible = CheckRenderers();

        if (isVisible)
        {
            isWrappingX = false;
            isWrappingY = false;
            return;
        }

        if (isWrappingX && isWrappingY)
        {
            return;
        }

        var cam = Camera.main;
        var viewportPosition = cam.WorldToViewportPoint(transform.position);
        var newPosition = transform.position;

        if (!isWrappingX && (viewportPosition.x > 1 || viewportPosition.x < 0))
        {
            newPosition.x = -newPosition.x;

            isWrappingX = true;
        }

        if (!isWrappingY && (viewportPosition.y > 1 || viewportPosition.y < 0))
        {
            newPosition.y = -newPosition.y;

            isWrappingY = true;
        }

        transform.position = newPosition;
    }
}
