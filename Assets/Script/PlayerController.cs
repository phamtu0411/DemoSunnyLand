using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Collider2D coll;
    private Animator anim;
    private SpriteRenderer sprite;

    private enum State { idle, run, jump, fall, hurt };
    private State state = State.idle;

    [SerializeField] private LayerMask ground;
    [SerializeField] private float speed = 7f;
    [SerializeField] private float jumpForce = 7f;
    [SerializeField] private float hurtForce = 3f;

    [SerializeField] private int cherries = 0;
    [SerializeField] private int health = 3;

    [SerializeField] private TextMeshProUGUI score;
    [SerializeField] private TextMeshProUGUI healthAmount;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        healthAmount.text = health.ToString();
    }

    void Update()
    {
        if (state != State.hurt)
        {
            InputManager();
        }
        AnimationState();
        anim.SetInteger("Status", (int)state);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Cherry")
        {
            Destroy(collision.gameObject);
            cherries += 1;
            score.text = cherries.ToString();
        }
        if (collision.tag == "PowerUp")
        {
            Destroy(collision.gameObject);
            jumpForce = 10f;
            GetComponent<SpriteRenderer>().color = Color.yellow;
            StartCoroutine(ResetBuff());
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if (state == State.fall)
            {
                enemy.JumpOn();
                Jump();
            }
            else
            {
                state = State.hurt;
                HandleHealth();
                if (collision.gameObject.transform.position.x > transform.position.x)
                {
                    rb.velocity = new Vector2(-hurtForce, rb.velocity.y);
                }
                else
                {
                    rb.velocity = new Vector2(hurtForce, rb.velocity.y);
                }
            }
        }
    }

    private void InputManager()
    {
        float direction = Input.GetAxis("Horizontal");
        //move left
        if (direction < 0)
        {
            rb.velocity = new Vector2(direction * speed, rb.velocity.y);
            transform.localScale = new Vector2(-1, 1);
        }
        //move right
        else if (direction > 0)
        {
            rb.velocity = new Vector2(direction * speed, rb.velocity.y);
            transform.localScale = new Vector2(1, 1);
        }
        //jump
        if (Input.GetKeyDown("space") && IsGrounded())
        {
            Jump();
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        state = State.jump;
    }

    private void AnimationState()
    {
        if (state == State.jump)
        {
            if (rb.velocity.y < -.1f)
            {
                state = State.fall;
            }
        }
        else if (state == State.fall)
        {
            if (IsGrounded())
            {
                state = State.idle;
            }
        }
        else if (state == State.hurt)
        {
            if (Mathf.Abs(rb.velocity.x) < .1f)
            {
                state = State.idle;
            }
        }
        else if (Mathf.Abs(rb.velocity.x) > .1f)
        {
            state = State.run;
        }
        else
        {
            state = State.idle;
        }
    }

    private void FootStep()
    {
    }

    private bool IsGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, ground);
    }

    private void HandleHealth()
    {
        health -= 1;
        healthAmount.text = health.ToString();
        if (health <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    private IEnumerator ResetBuff()
    {
        yield return new WaitForSeconds(5);
        jumpForce = 8;
        sprite.color = Color.white;
    }
}
