using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {
    public float maxSpeed = 10f;
    public float jumpForce = 300f;

    private Rigidbody2D rb2d;
    public Transform GroundCheck;
    public LayerMask groundLayer;
    private bool isGrounded;
    private float groundRadius = 0.2f;

	// Use this for initialization
	void Start () {
        rb2d = GetComponent<Rigidbody2D>();
	}

	void Update()
	{
        if (isGrounded && Input.GetKeyDown(KeyCode.X)) {
            rb2d.AddForce(new Vector2(0, jumpForce));
        }
	}

	void FixedUpdate () {

        isGrounded = Physics2D.OverlapCircle(GroundCheck.position, groundRadius, groundLayer);

        // side to side movement
        float moveH = Input.GetAxis("Horizontal");
        rb2d.velocity = new Vector2(moveH*maxSpeed, rb2d.velocity.y);

        Debug.Log("Grounded: " + isGrounded);
	}
}
