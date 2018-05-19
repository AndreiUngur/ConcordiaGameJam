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
	private Rigidbody2D robot;
	private GameObject playerObject;
	private Transform player;
	//public Transform target;
	// Use this for initialization
	
	void Start ()
	{
		//GENERIC
		robot = GetComponent<Rigidbody2D>();
		cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
		// MOVEMENT
		lastPos = cam.WorldToViewportPoint(robot.position);

		playerObject = GameObject.FindGameObjectWithTag("Player");

		player = playerObject.GetComponent<Transform>();

		Physics2D.IgnoreCollision(playerObject.GetComponent<Collider2D>(), GetComponent<Collider2D>());

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

	private static void MovementHandler(Transform player, Transform robot, Rigidbody2D robotrb2d, float speed)
	{
		Vector2 velocity = new Vector2((robot.position.x - player.position.x) * speed, robotrb2d.velocity.y);
        robot.GetComponent<Rigidbody2D>().velocity = -velocity;	
		/*
		robot.LookAt(player);
		robot.Rotate(new Vector3(0,-90,0),Space.Self);//correcting the original rotation
        //robot.position = Vector2.MoveTowards(robot.position,player.transform.position, speed*Time.deltaTime); 
         
         //move towards the player
         if (Vector3.Distance(robot.position,player.position)>1f){//move if distance from target is greater than 1
             robot.Translate(new Vector3(speed* Time.deltaTime, 0) );
         }
		 */
	}

	private static bool directionChange(float lastPos, float Pos)
	{
		return true;
	}

	private static bool shootingTime()
	{
		if(Mathf.RoundToInt(Random.value*10) == 1)
		{
			Debug.Log("Shooting Time!");
			return true;
		}
		return false;
	}

	private static void shootBasic(Rigidbody2D robot, Transform player, GameObject bullet)
	{
		// (Rotation is temporary)
		GameObject basicShot = (GameObject)Instantiate (bullet, robot.transform.position, player.transform.rotation);
		//basicShot.transform.position = new Vector2(robot.position.x, player.position.y);
		//basicShot.GetComponent<Rigidbody2D>().velocity = new Vector2(playerRbd.velocity.x, playerRbd.velocity.y);
	}

	// Update is called once per frame
	void Update ()
	{
		// If the robot is too far, it'll have more chances of doing "earthquake" type attacks
		// If the robot is close, it'll have more chances of doing "melee" type attacks
		float distance = Vector3.Distance(transform.position, player.position);

		Vector2 cameraPos = cam.WorldToViewportPoint(player.position);
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
 
		if (shootingTime())
		{
			shootBasic(robot, player, bullet);
		}

		MovementHandler(player, transform, robot, speed);
	}
}
