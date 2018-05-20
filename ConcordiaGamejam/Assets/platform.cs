using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class platform : MonoBehaviour {

    Rigidbody2D parent;
    BoxCollider2D pCollider;

	// Use this for initialization
	void Start () {
        parent = transform.parent.gameObject.GetComponent<Rigidbody2D>();
        pCollider = parent.GetComponent<BoxCollider2D>();
        BoxCollider2D robo = GameObject.FindGameObjectWithTag("Robot").GetComponent<BoxCollider2D>();
        Physics2D.IgnoreCollision(pCollider, robo);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("ENter");
        if (collision.gameObject.tag == "Player")
        {
            Physics2D.IgnoreCollision(pCollider, collision);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Physics2D.IgnoreCollision(pCollider, collision, false);
        }
    }
}
