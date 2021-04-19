using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public int playerHealth;
    public HealthBar healthBar;

    public AudioSource audiosrc;
    public AudioClip clip;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        renderers = GetComponentsInChildren<Renderer>();
        localScale = transform.localScale;
        healthBar.setMaxHealth(playerHealth);
        audiosrc = GetComponent<AudioSource>();
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

        if(rb.velocity.y == 0)
        {
            anim.SetBool("isJumping", false);
        }
        if (rb.velocity.y > 0)
            anim.SetBool("isJumping", true);
        if (rb.velocity.y < 0)
            anim.SetBool("isJumping", true);

        //screenwrap
        ScreenWrap();
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

    public void TakeDamage(int Damage)
    {
        playerHealth -= Damage;
        healthBar.setHealth(playerHealth);
        audiosrc.PlayOneShot(clip);
        if(playerHealth == 0)
        {
            SceneManager.LoadScene("GameOver");
        }
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Destroyer")
        {
            playerHealth = 0;
            SceneManager.LoadScene("GameOver");
        }
        if (collision.tag == "LevelEnd")
        {
            SceneManager.LoadScene("GameOver");
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
