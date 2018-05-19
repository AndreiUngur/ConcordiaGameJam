using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour {

    public const float FireDelay = 0.2f;

    public Rigidbody2D Prefab;

    private Vector2 LastDirection;
    private bool FirePressed;
    private float LastFired = 0;

	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    void Update () {
        FirePressed = Input.GetKey(KeyCode.G);
        LastFired -= Time.deltaTime;


        if (FirePressed && LastFired <= 0)
        {
            GameObject bullet = BulletPool.pool.GetBullet();
            if (bullet != null)
            {
                bullet.SetActive(true);
                bullet.transform.position = transform.position + new Vector3(1, 0, 0);
            }
            LastFired = FireDelay;
        }
	}
}
