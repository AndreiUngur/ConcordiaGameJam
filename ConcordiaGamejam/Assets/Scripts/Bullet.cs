using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour {


    public Vector2 Direction { get; set; }

    private Rigidbody2D bullet;


    // Use this for initialization
    void Start () {
        //temp
        Direction.Set(1, 0);
        bullet = GetComponent<Rigidbody2D>();

    }

    private void FixedUpdate()
    {
        bullet.velocity = new Vector2(10, 0);

    }

    // Update is called once per frame
    void Update () {

	}

    private void OnBecameInvisible()
    {
        gameObject.SetActive(false);
    }

}
