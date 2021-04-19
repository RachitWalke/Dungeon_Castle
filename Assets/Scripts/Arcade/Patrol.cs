using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : MonoBehaviour
{
    private Vector3 characterIntialPos;
    public float patrolingRange;
    public bool isFacingRight = false;

    void Start()
    {
        characterIntialPos = transform.position;
    }

    public void Flip()
    {
        isFacingRight = !isFacingRight;
        transform.Rotate(0, 180, 0);
    }
    public void patrol()
    {
        if (transform.position.x > characterIntialPos.x + patrolingRange && isFacingRight) Flip();
        if (transform.position.x < characterIntialPos.x - patrolingRange && !isFacingRight) Flip();
    }
}
