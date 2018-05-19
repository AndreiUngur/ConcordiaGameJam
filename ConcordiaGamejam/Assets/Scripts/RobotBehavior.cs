using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotBehavior : MonoBehaviour {
	// MOVING
	public float speed;

	// SHOOTING
	public GameObject bullet;
	public int shootFreq;
	/*
	private float fireRate;
	private float damage;
	private LayerMask notToHit;
	*/
	private int nextUpdate=1;
	private Vector2 lastPos;
	private Camera cam;
	private BoxCollider2D boxCollider;
	private Rigidbody2D rigidBody;
	//public Transform target;
	// Use this for initialization
	
	void Start ()
	{
		//GENERIC
		rigidBody = GetComponent<Rigidbody2D>();
		cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
		// MOVEMENT
		lastPos = cam.WorldToViewportPoint(rigidBody.position);

	}
	
	private static void MoveLeft(Rigidbody2D rigidBody, float speed)
	{
		rigidBody.velocity = new Vector2(-1.0f*speed, rigidBody.velocity.y);
	}

	private static void MoveRight(Rigidbody2D rigidBody, float speed)
	{
		rigidBody.velocity = new Vector2(1.0f*speed, rigidBody.velocity.y);
	}

	private static void MoveUp(Rigidbody2D rigidBody, float speed)
	{
		rigidBody.velocity = new Vector2(rigidBody.velocity.x,1.0f*speed);
	}

	private static void MoveDown(Rigidbody2D rigidBody, float speed)
	{
		rigidBody.velocity = new Vector2(rigidBody.velocity.x, -1.0f*speed);
	}

	private static void MovementHandler(Vector2 pos, Rigidbody2D rigidBody, float speed)
	{	
		if(pos.x < 0.5)
		{
			MoveRight(rigidBody, speed);
			Debug.Log("Moving right");
		}
		// Right of camera
		if(0.5 < pos.x)
		{
			MoveLeft(rigidBody, speed);
			Debug.Log("Moving left");
		}
		// Left of camera
		// Below camera
		if(pos.y < 0.5)
		{
			//MoveUp(rigidBody, speed);
		}
		// Above camera
		if(0.5 < pos.y)
		{
			//MoveDown(rigidBody, speed);
		}
	}

	private static bool directionChange(float lastPos, float Pos)
	{
		return true;
	}

	private static bool shootingTime()
	{
		if(Mathf.RoundToInt(Random.value*10) == 1)
		{
			return true;
			Debug.Log("Shooting Time!");
		}
		return false;
	}

	private static void shootBasic(Rigidbody2D rigidBody, GameObject bullet)
	{
		GameObject basicShot = (GameObject)Instantiate (bullet, rigidBody.transform.position);
		MoveRight(basicShot.GetComponent<Rigidbody2D>(), 10f);
	}

	// Update is called once per frame
	void Update ()
	{
		Vector2 pos = cam.WorldToViewportPoint(rigidBody.position);
		//if (directionChange(lastPos, pos))
		//{
			/*
			// If the next update is reached
			if(Time.time>=nextUpdate){
				Debug.Log(Time.time+">="+nextUpdate);
            	// Change the next update (current second+1)
            	nextUpdate=Mathf.FloorToInt(Time.time)+;
			}
			*/
		//}

		lastPos = pos;
 
		if (shootingTime())
		{
			shootBasic(rigidBody, bullet);
		}
	
		MovementHandler(pos, rigidBody, speed);
	}
}
