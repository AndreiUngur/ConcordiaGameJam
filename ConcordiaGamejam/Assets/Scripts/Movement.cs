using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Movement : MonoBehaviour { 
    public const int max_health = 3;
    public int currentHealth;

    public float iframeDuration = 2f;
    public float iframes = 0;

    public float maxSpeed = 10f;
    public float jumpForce = 300f;
    public static bool isFacingRight = true;
    public static Vector2 direction = new Vector2(0,0);

    private Knockback kb;

    public bool isKnockedBack;

    private Rigidbody2D rb2d;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    public Transform GroundCheck;
    public LayerMask groundLayer;
    public bool isGrounded;
    private float groundRadius = 0.2f;
    private int aimLevel = 0;

    public bool triggered;

    private bool isDead = false;

	// Use this for initialization
	void Start () {
        rb2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        currentHealth = max_health;

        kb = gameObject.GetComponentInChildren<Knockback>();
	}

	void Update()
    {
        if (iframes > 0)
        {
            iframes -= Time.deltaTime;
        }

        spriteRenderer.flipX = !isFacingRight;
        animator.SetBool("isMoving", direction.magnitude > 0.01);
        animator.SetBool("isGrounded", isGrounded);
        animator.SetInteger("aimLevel", aimLevel);

    }

	void FixedUpdate () {

        if (isDead)
        {
            return;
        }

        isGrounded = Physics2D.OverlapCircle(GroundCheck.position, groundRadius, groundLayer);

        // side to side movement
        float moveH = Input.GetAxis("Horizontal");

        //no moving while getting knocked back
        if (!isKnockedBack)
        {
            rb2d.velocity = new Vector2(moveH * maxSpeed, rb2d.velocity.y);
        }

        if (isGrounded && Input.GetKeyDown(KeyCode.X))
        {
            rb2d.AddForce(new Vector2(0, jumpForce));
        }

        if (isFacingRight && moveH < 0)
        {
            isFacingRight = false;

        }
        else if (!isFacingRight && moveH > 0)
        {
            isFacingRight = true;
        }

        direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
    
        if (direction.y > 0.01) {
            aimLevel = 1;
        } else if (direction.y < -0.01) {
            aimLevel = -1;
        } else {
            aimLevel = 0;
        }
	}


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Damager>() != null && iframes <= 0)
        {
            Damager damager = collision.gameObject.GetComponent<Damager>();

            damage(damager, collision);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Damager>() != null && iframes <= 0)
        {
            Damager damager = collision.gameObject.GetComponent<Damager>();

            damage(damager, collision);
        }
    }

    public void damage(Damager damager, Collider2D collision)
    {

        //don't keep this
        currentHealth -= damager.damage;

        if (collision.gameObject.tag == "Robot")
        {

            if (kb.triggered)
            {
                iframes = iframeDuration;
                kb.triggered = false;
            } else
            {
                triggered = true;
            }
        } else
        {
            iframes = iframeDuration;
        }

        if (currentHealth <= 0)
        {
            die();
        }
    }

    public void die()
    {
        Debug.Log("YOU DIED");
        isDead = true;

    }
}
