using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogAI : Enemy //k? th?a class enemy
{
    [SerializeField] private float left;
    [SerializeField] private float right;
    [SerializeField] private float jumpLength = 4f;
    [SerializeField] private float jumpHeight = 3f;
    [SerializeField] private LayerMask ground;

    private Collider2D coll;
    private bool facingLeft = true;

    protected override void Start() //override
    {
        base.Start();
        coll = GetComponent<Collider2D>();
    }

    void Update()
    {
        //transition from jump to fall
        if (anim.GetBool("Jump"))
        {
            if (rb.velocity.y < .1)
            {
                anim.SetBool("Fall", true);
                anim.SetBool("Jump", false);
            }
        }
        if (coll.IsTouchingLayers(ground) && anim.GetBool("Fall"))
        {
            anim.SetBool("Fall", false);
        }
    }

    private void Move()
    {
        if (facingLeft)
        {
            if (transform.position.x > left)
            {
                if (transform.position.x != 1)
                {
                    transform.localScale = new Vector3(1, 1);
                }
                if (coll.IsTouchingLayers(ground))
                {
                    rb.velocity = new Vector2(-jumpLength, jumpHeight);
                    anim.SetBool("Jump", true);
                }
            }
            else
            {
                facingLeft = false;
            }
        }
        else
        {
            if (transform.position.x < right)
            {
                if (transform.position.x != -1)
                {
                    transform.localScale = new Vector3(-1, 1);
                }
                if (coll.IsTouchingLayers(ground))
                {
                    rb.velocity = new Vector2(jumpLength, jumpHeight);
                    anim.SetBool("Jump", true);
                }
            }
            else
            {
                facingLeft = true;
            }
        }
    }
}