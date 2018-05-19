using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour {


    public Vector2 Direction { get; set; }
    public float bulletVelocity = 10f;

    private Rigidbody2D bullet;


    // Use this for initialization
    void Start () {
        bullet = GetComponent<Rigidbody2D>();
        init();

    }

    private void FixedUpdate()
    {
        bullet.velocity = Direction * bulletVelocity;
    }

    // Update is called once per frame
    void Update () {

	}

    private void OnBecameInvisible()
    {
        gameObject.SetActive(false);
    }

	private void OnBecameVisible()
	{
        init();
	}

	public void init() {
        if (Movement.direction.magnitude < 0.1)
        {
            Direction = new Vector2(Movement.isFacingRight ? 1 : -1, 0);
        }
        else
        {
            Direction = Movement.direction;
        }
    }
}
