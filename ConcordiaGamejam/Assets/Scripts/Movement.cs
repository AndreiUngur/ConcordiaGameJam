using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float maxSpeed = 10f;
    public float jumpForce = 300f;
    public static bool isFacingRight = true;
    public static Vector2 direction = new Vector2(0, 0);

    private Rigidbody2D rb2d;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    public Transform GroundCheck;
    public LayerMask groundLayer;
    private bool isGrounded;
    private float groundRadius = 0.2f;
    private int aimLevel = 0;

    // Use this for initialization
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (isGrounded && Input.GetKeyDown(KeyCode.X))
        {
            rb2d.AddForce(new Vector2(0, jumpForce));
        }

        spriteRenderer.flipX = !isFacingRight;
        animator.SetBool("isMoving", direction.magnitude > 0.01);
        animator.SetBool("isGrounded", isGrounded);
        animator.SetInteger("aimLevel", aimLevel);

    }

    void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(GroundCheck.position, groundRadius, groundLayer);

        // side to side movement
        float moveH = Input.GetAxis("Horizontal");
        rb2d.velocity = new Vector2(moveH * maxSpeed, rb2d.velocity.y);

        if (isFacingRight && moveH < 0)
        {
            isFacingRight = false;

        }
        else if (!isFacingRight && moveH > 0)
        {
            isFacingRight = true;
        }

        direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        Debug.Log(direction.y);
        if (direction.y > 0.01) {
            aimLevel = 1;
        } else if (direction.y < -0.01) {
            aimLevel = -1;
        } else {
            aimLevel = 0;
        }
    }
}
