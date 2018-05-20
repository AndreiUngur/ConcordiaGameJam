using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knockback : MonoBehaviour {

    public float kbScale = 70f;
    public SpriteRenderer testSprite;

    GameObject knockback;
    GameObject Player;
    Rigidbody2D rbd;
    Movement movement;

    public bool triggered;

    float mindelay = 0.5f;
    float delayElapsed = 0f;

	// Use this for initialization
	void Start () {

        knockback = gameObject;
        Player = knockback.transform.parent.gameObject;
        rbd = Player.GetComponent<Rigidbody2D>();
        movement = Player.GetComponent<Movement>();

        if (mindelay > movement.iframeDuration)
        {
            mindelay = movement.iframeDuration;
        }

        Physics2D.IgnoreCollision(GetComponent<BoxCollider2D>(), Player.GetComponent<BoxCollider2D>());
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

    private void knock(Collider2D collision)
    {      

        if (collision.gameObject.tag == "Robot" && movement.iframes <= 0)
        {
            Debug.Log("kbed-----------------------------------------");

            float dist = Mathf.Abs(collision.gameObject.transform.position.x - gameObject.transform.position.x);

            bool hitOnRight = collision.gameObject.transform.position.x - gameObject.transform.position.x > 0;
            Vector2 kbDir = new Vector2(hitOnRight ? -1 : 1, 0.5f).normalized;

            rbd.velocity = Vector2.zero;
            rbd.AddForce(kbDir * kbScale, ForceMode2D.Impulse);

            movement.isKnockedBack = true;
            delayElapsed = mindelay;

            if (movement.triggered)
            {
                movement.iframes = movement.iframeDuration;
                movement.triggered = false;
            } else
            {
                triggered = true;
            }
        }
    }

    //this is seems to sometimes just not be called 
    private void OnTriggerStay2D(Collider2D collision)
    {
        knock(collision);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        knock(collision);
    }
}
