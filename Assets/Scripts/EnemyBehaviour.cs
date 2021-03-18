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
    private bool isStaggered = false;
    private bool animbool = true;

    //screen Wrapper

    Renderer[] renderers;
    bool isWrappingX = false;
    bool isWrappingY = false;


    //enemy type
    public enum EnemyType { spider,mushroom};
    public EnemyType enemytype;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        renderers = GetComponentsInChildren<Renderer>();
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
        }

    }

    void movememt()
    {
        if (isStaggered)
        {
            rb.velocity = new Vector2(0f, 0f);
        }  
        else if (!isStaggered)
            rb.velocity = new Vector2(dirX, rb.velocity.y) * enemySpeed;

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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Flipper")
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
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && rb.velocity.y == 0f)
        {
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
            }
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
        transform.position = newPosition;
    }
    #endregion

}
