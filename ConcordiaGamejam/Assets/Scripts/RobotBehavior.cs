﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotBehavior : MonoBehaviour {
	// MOVING
	public float speed;
	private Vector2 lastPos;
	
	// SHOOTING
	public GameObject bullet;
	public int shootFreq;
	public float damage = 0.1f;
	private bool isDead;

	// CAMERA
	private Camera cam;
	
	// ROBOT
	private Rigidbody2D robot;
	private GameObject healthbar;
	private int nextUpdate=1;
	private bool following;
	
	// PLAYER
	private GameObject playerObject;
	private Transform player;
	
	// GENERIC MOVEMENT ---------------------------------------------------
	private static void MoveLeft(Transform transform, Rigidbody2D rigidBody, float speed)
	{
		if(transform.position.x <= 7)
		{
			rigidBody.velocity = new Vector2(0, rigidBody.velocity.y);
		}
		else
		{
			rigidBody.velocity = new Vector2(-1.0f*speed, rigidBody.velocity.y);
		}
	}

	private static void MoveRight(Transform transform, Rigidbody2D rigidBody, float speed)
	{
		if(transform.position.x >= 7)
		{
			rigidBody.velocity = new Vector2(0, rigidBody.velocity.y);
		}
		else
		{
			rigidBody.velocity = new Vector2(1.0f*speed, rigidBody.velocity.y);	
		}
	}

	private static void MoveUp(Rigidbody2D rigidBody, float speed)
	{
		rigidBody.velocity = new Vector2(rigidBody.velocity.x,1.0f*speed);
	}

	private static void MoveDown(Rigidbody2D rigidBody, float speed)
	{
		rigidBody.velocity = new Vector2(rigidBody.velocity.x, -1.0f*speed);
	}
	// ----------------------------------------------------------------------
	
	// Follows the player ---------------------------------------------------
	private static void FollowAI(Transform player, Transform robot, Rigidbody2D robotrb2d, float speed)
	{
		Vector2 velocity = new Vector2((robot.position.x - player.position.x) * speed, robotrb2d.velocity.y);
        robot.GetComponent<Rigidbody2D>().velocity = -velocity;
	}
	// ----------------------------------------------------------------------

	// Determines if player changed directions ------------------------------
	private static bool directionChange(float lastPos, float Pos)
	{
		return true;
	}
	// ----------------------------------------------------------------------

	// Determine if it's the time to shoot & Shoot --------------------------
	private static bool shootingTime()
	{
		int randomNumber = Mathf.RoundToInt(Random.value*120);
		if(randomNumber == 1)
		{
			Debug.Log("Shooting Time!");
			return true;
		}
		return false;
	}

	private static void shootBasic(Rigidbody2D robot, Transform player, GameObject bullet)
	{
        Vector2 v1 = player.transform.position;
        Vector2 v2 = robot.transform.position;
        //Find the angle for the two Vectors
        float angle = Vector2.Angle(v1, v2);
		GameObject basicShot = (GameObject)Instantiate (bullet, robot.transform.position, Quaternion.Euler(0, 0, -angle));
	}


	// 2. Earthquake
	private static bool earthquakeTime()
	{
		int randomNumber = Mathf.RoundToInt(Random.value*120);
		if(randomNumber == 1)
		{
			Debug.Log("Earthquake Time!");
			return true;
		}
		return false;
	}

	private static void earthquake()
	{
		GameObject mainCamera = GameObject.Find("Main Camera");
		CameraShake camShakeScript = mainCamera.GetComponent<CameraShake>();
		camShakeScript.shakeDuration = 1.0f;
		/*
		I thought this would work but it doesn't. 
		Debug.Log("EARTHQUAKE!!!!!");
		GameObject[] floorTiles = GameObject.FindGameObjectsWithTag("floorTile");
        foreach (GameObject tile in floorTiles)
        {
			Rigidbody2D tileRigidbody = tile.GetComponent<Rigidbody2D>();
			tileRigidbody.velocity = tile.transform.up * 5.0f;
        }
		*/
	}
	// ----------------------------------------------------------------------

	// BELOW ARE UNITY FUNCTIONS ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
	void Start ()
	{
		//GENERIC
		robot = GetComponent<Rigidbody2D>();
		cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
		
		// MOVEMENT
		lastPos = cam.WorldToViewportPoint(robot.position);
		playerObject = GameObject.FindGameObjectWithTag("Player");
		player = playerObject.GetComponent<Transform>();
		following = true;

		// COLLISION & DAMAGE
		isDead = false;
		Physics2D.IgnoreCollision(playerObject.GetComponent<Collider2D>(), GetComponent<Collider2D>());
		healthbar = GameObject.FindGameObjectWithTag("robotHealth");
	}

	void OnTriggerEnter2D(Collider2D otherCollider)
    {
        // Is this a shot?
        GameObject other = otherCollider.gameObject;
        if (other.tag == "playerBullet")
        {
			if(healthbar.transform.localScale.x <= 0.0f)
			{
				Debug.Log("Robot got KILLED.");
				isDead = true;
				return;
			}
			healthbar.transform.localScale = new Vector2(healthbar.transform.localScale.x - damage, healthbar.transform.localScale.y);
        }
    }

	// Update is called once per frame
	void Update ()
	{
		// Become ragdoll if dead
		if(isDead)
		{
			Debug.Log("Robot died. :(");
			Physics2D.IgnoreCollision(playerObject.GetComponent<Collider2D>(), GetComponent<Collider2D>(), false);
			return;
		}

		// Randomly determine if it's time to shoot
		if (shootingTime())
		{
			shootBasic(robot, player, bullet);
		}

		if(earthquakeTime())
		{
			earthquake();
		}
		// "Basic" AI that follows the player
		if(following)
		{
			FollowAI(player, transform, robot, speed);
		}
		else
		{
			bool left = Mathf.FloorToInt(Random.value*2) == 0;
			if(left)
			{
				MoveLeft(transform, robot, speed * 0.8f);
			}
			else
			{
				MoveRight(transform, robot, speed * 0.8f);
			}
		}

		// Below code is EXPERIMENTAL and NOT USED YET
		// If the robot is too far, it'll have more chances of doing "earthquake" type attacks
		// If the robot is close, it'll have more chances of doing "melee" type attacks
		float distance = Vector3.Distance(transform.position, player.position);

		Vector2 cameraPos = cam.WorldToViewportPoint(player.position);
		if(Time.time>=nextUpdate){
			Debug.Log(Time.time+">="+nextUpdate);
            // Change the next update (current second+1)
            nextUpdate=Mathf.FloorToInt(Time.time)+Mathf.FloorToInt(5*Random.value)+3;
			following = !following;
		}
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
		lastPos = cameraPos;
	}
}
