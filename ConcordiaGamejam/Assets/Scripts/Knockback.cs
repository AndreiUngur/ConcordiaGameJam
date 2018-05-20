using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knockback : MonoBehaviour {

    public float kbScale = 40f;

    GameObject knockback;
    GameObject Player;
    Rigidbody2D rbd;
    Movement movement;


    float mindelay = 0.2f;
    float delayElapsed = 0f;

	// Use this for initialization
	void Start () {
        knockback = gameObject;
        Player = knockback.transform.parent.gameObject;
        rbd = Player.GetComponent<Rigidbody2D>();
        movement = Player.GetComponent<Movement>();
	}
	
	// Update is called once per frame
	void Update () {
        if (delayElapsed > 0)
        {
            delayElapsed -= Time.deltaTime;
        }
	}

    private void FixedUpdate()
    {

        if (movement.isGrounded && movement.isKnockedBack && delayElapsed <= 0)
        {
            movement.isKnockedBack = false;
        }
    }

    //this is seems to sometimes just not be called 
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.gameObject.tag == "Robot" && !movement.isKnockedBack && movement.iframes <= 0)
        {
            bool hitOnRight = collision.gameObject.transform.position.x - gameObject.transform.position.x > 0;
            Vector2 kbDir = new Vector2(hitOnRight ? -1 : 1, 1).normalized;

            rbd.velocity = Vector2.zero;
            rbd.AddForce(kbDir * kbScale, ForceMode2D.Impulse);

            movement.isKnockedBack = true;
            delayElapsed = mindelay;
        }
    }
}
