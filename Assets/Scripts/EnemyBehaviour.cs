using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sprite;

    [SerializeField] private float dirX;
    public float enemySpeed;
    public bool isStaggered = false;
    private bool animbool = true;
    public int enemyHealth;

    //audio
    public AudioSource audiosrc;
    public AudioClip clip;


    //screen Wrapper

    Renderer[] renderers;
    bool isWrappingX = false;
    bool isWrappingY = false;


    //enemy type
    public enum EnemyType { spider,mushroom,skeleton};
    public EnemyType enemytype;

    //efects
    public GameObject bloodeffect;
    private CameraShake shake;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        renderers = GetComponentsInChildren<Renderer>();
        shake = GameObject.FindGameObjectWithTag("ScreenShake").GetComponent<CameraShake>();
        audiosrc = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (enemytype)
        {
            case EnemyType.spider :
                movememt();
                break;

            case EnemyType.mushroom :
                movememt();
                break;
            case EnemyType.skeleton:
                movememt();
                break;
        }

        //destroy enemy
        if(enemyHealth <= 0)
        {
            Destroy(gameObject);
            switch (enemytype)
            {
                case EnemyType.spider:
                    GameManager.instance.UpdateScore(100);
                    break;
                case EnemyType.mushroom:
                    GameManager.instance.UpdateScore(200);
                    break;
                case EnemyType.skeleton:
                    GameManager.instance.UpdateScore(500);
                    break;
            }
        }
    }

    void movememt()
    {
        if (isStaggered)
        {
            rb.velocity = new Vector2(0f, 0f);
            rb.constraints = RigidbodyConstraints2D.FreezePositionX;
            switch (enemytype)
            {
                case EnemyType.spider:
                    StartCoroutine(Revive(4f));
                    break;
                case EnemyType.mushroom:
                    StartCoroutine(Revive(3f));
                    break;
                case EnemyType.skeleton:
                    StartCoroutine(Revive(3.5f));
                    break;
            }
        }  
        else if (!isStaggered)
        {
            rb.velocity = new Vector2(dirX, rb.velocity.y) * enemySpeed;
            rb.constraints = RigidbodyConstraints2D.None;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
            
        ScreenWrap();
    }

    private void flip()
    {
        if(isStaggered == false)
        {
            if (dirX > 0)
            {
                sprite.flipX = true;
            }
            else if (dirX < 0)
            {
                sprite.flipX = false;
                return;
            }
        }
       
    }

    IEnumerator Revive(float waitTime)
    {
        //wait and revive
        yield return new WaitForSeconds(waitTime);
        anim.SetBool("isStagger", animbool);
        isStaggered = !isStaggered;
        animbool = !animbool;
        flip();
        //stop coroutine
        if(isStaggered==false)
        {
            StopAllCoroutines();
        }
        yield break;
    }

    public void TakeDamage(int damage)
    {
        if(isStaggered)
        {
            shake.CamShake();
            enemyHealth -= damage;
            Instantiate(bloodeffect, transform.position, Quaternion.identity);
        }
    }

    public void statMultiiplier()
    {
        switch (enemytype)
        {
            case EnemyType.spider:
                enemySpeed += 1;
                enemyHealth += 1;
                break;
            case EnemyType.mushroom:
                enemySpeed += 1;
                enemyHealth += 1;
                break;
            case EnemyType.skeleton:
                enemyHealth += 1;
                break;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            switch (enemytype)
            {
                case EnemyType.spider:
                    dirX = -dirX;
                    flip();
                    break;
                case EnemyType.mushroom:
                    dirX = -dirX;
                    flip();
                    break;
                case EnemyType.skeleton:
                    dirX = -dirX;
                    flip();
                    break;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && rb.velocity.y == 0f)
        {
            audiosrc.PlayOneShot(clip);
            switch (enemytype)
            {
                case EnemyType.spider:
                    anim.SetBool("isStagger", animbool);
                    isStaggered = !isStaggered;
                    animbool = !animbool;
                    flip();
                    break;
                case EnemyType.mushroom:
                    anim.SetBool("isStagger", animbool);
                    isStaggered = !isStaggered;
                    animbool = !animbool;
                    flip();
                    break;
                case EnemyType.skeleton:
                    anim.SetBool("isStagger", animbool);
                    isStaggered = !isStaggered;
                    animbool = !animbool;
                    flip();
                    break;
            }
        }
        if(collision.tag == "Flipper")
        {
            switch (enemytype)
            {
                case EnemyType.spider:
                    dirX = -dirX;
                    flip();
                    break;
                case EnemyType.mushroom:
                    dirX = -dirX;
                    flip();
                    break;
                case EnemyType.skeleton:
                    dirX = -dirX;
                    flip();
                    break;
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {

        if (collision.gameObject.tag == "Player" && !isStaggered)
        {
            GameManager.instance.takeDamage(1);
            Destroy(collision.gameObject);
            GameManager.instance.spwanPlayer();
        }
        if (collision.gameObject.tag == "Destroyer")
        {
            Destroy(this.gameObject);
        }
        if (collision.gameObject.tag == "Enemy")
        {
            rb.constraints = RigidbodyConstraints2D.FreezePositionY;
        }
    }

    #region Screen Wrapping
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
        transform.position = newPosition;
    }
    #endregion

}
