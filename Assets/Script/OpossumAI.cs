using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpossumAI : Enemy
{
    [SerializeField] private float left;
    [SerializeField] private float right;
    [SerializeField] private float move = 4f;
    private bool facingLeft = true;

    protected override void Start() //override
    {
        base.Start();
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
                    rb.velocity = new Vector2(-4, 0);
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
                    rb.velocity = new Vector2(4, 0);
                }
            }
            else
            {
                facingLeft = true;
            }
        }
    }
}
