using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    public float playerSpeed;
    public float jumpforce;
    private float dirX;
    private bool facingRight = true;
    private Vector3 localScale;

    Renderer[] renderers;
    bool isWrappingX = false;
    bool isWrappingY = false;
    bool isAttack = false;
    bool is_Stagger = false;

    //player lives
    private int playerLives = 3;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        renderers = GetComponentsInChildren<Renderer>();
        localScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        dirX = Input.GetAxisRaw("Horizontal") * playerSpeed;
        

        if (Input.GetKeyDown(KeyCode.Space) && rb.velocity.y == 0)
        {
            rb.AddForce(Vector2.up * jumpforce);
        }
        if(Mathf.Abs(dirX) > 0 && rb.velocity.y == 0)
        {
            anim.SetBool("isRunning", true);
        }
        else
        {
            anim.SetBool("isRunning", false);
        }

        if(Input.GetMouseButton(0))
        {
            anim.SetBool("isAttacking", true);
            isAttack = true;
        }
        else
        {
            anim.SetBool("isAttacking", false);
            isAttack = false;
        }

        if(rb.velocity.y == 0)
        {
            anim.SetBool("isJumping", false);
        }
        if (rb.velocity.y > 0)
            anim.SetBool("isJumping", true);
        if (rb.velocity.y < 0)
            anim.SetBool("isJumping", true);

        ScreenWrap();
        //GameManager.instance.screenWrapper.ScreenWrap();
    }
    private void FixedUpdate()
    {
        rb.velocity = new Vector2(dirX, rb.velocity.y);
    }
    private void LateUpdate()
    {
        if (dirX > 0)
            facingRight = true;
        else if (dirX < 0)
            facingRight = false;

        if (((facingRight) && (localScale.x < 0)) || ((!facingRight) && (localScale.x > 0)))
            localScale.x *= -1;

        transform.localScale = localScale;
    }

    public void getStagger(bool isStagger)
    {
        is_Stagger = isStagger;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Enemy" && isAttack && is_Stagger)
        {
            Destroy(collision.gameObject);
        }
        if(collision.gameObject.tag == "Enemy" && is_Stagger == false)
        {
            playerLives--;
            Debug.Log("Playerlives = " + playerLives);
            Destroy(this.gameObject);
        }
    }

    bool CheckRenderers()
    {
        foreach (Renderer renderer in renderers)
        {
            // If at least one render is visible, return true
            if (renderer.isVisible)
            {
                return true;
            }
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
