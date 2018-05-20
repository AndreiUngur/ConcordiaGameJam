using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RobotBehavior : MonoBehaviour {
	// MOVING
	public float speed = 0.5f;
    public static float sideSpeed = 0.8f;
	private Vector2 lastPos;
    private bool isFacingRight = true;
	
	// ATTACKING
	public GameObject bullet;
	public GameObject ice;
	public GameObject wind;
	public int windSpeed;
	public float damage;
    private bool isDead;

	// CAMERA & UI
	public GameObject canvasUI;
	private Text canvasText;
	private Camera cam;
	private string[] powers;

	// ROBOT
	public float maxHealth;
	private Rigidbody2D robot;
    private SpriteRenderer spriteRenderer;
	private GameObject healthbar;
	private int nextUpdate=1;
	private bool following;
	private int state;
		
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

	// Determine if it's the time for an attack --------------------------
	private static bool attackTime(int frequency, string attackType)
	{
		int randomNumber = Mathf.RoundToInt(Random.value*frequency);
		if(randomNumber == 1)
		{
			Debug.Log(attackType);
			return true;
		}
		return false;
	}
	// ----------------------------------------------------------------------

	// Attack handlers ------------------------------------------------------

	// 1. Shoot rockets
	private static void shootBasic(Rigidbody2D robot, Transform player, GameObject bullet)
	{
        Vector2 v1 = player.transform.position;
        Vector2 v2 = robot.transform.position;
        //Find the angle for the two Vectors
        float angle = Vector2.Angle(v1, v2);
		GameObject basicShot = (GameObject)Instantiate (bullet, robot.transform.position, Quaternion.Euler(0, 0, -angle));
	}


	// 2. Earthquake
	private static void earthquake()
	{
		GameObject mainCamera = GameObject.Find("Main Camera");
		CameraShake camShakeScript = mainCamera.GetComponent<CameraShake>();
		camShakeScript.shakeDuration = 1.0f;
	}

	// 3. Ice
	private static void iceRain(GameObject player, int maxIcicles, GameObject ice)
	{
		int icicleCount = Mathf.RoundToInt(Random.value*maxIcicles);
		for(int i=0; i<icicleCount; i++)
		{
			bool isNegative = Mathf.FloorToInt(Random.value*2)==0;
			float randomPos;
			if (isNegative)
			{
				randomPos = Random.value*10;
			}
			else
			{
				randomPos = Random.value * (-10);
			}
			GameObject icicle = (GameObject)Instantiate (ice, new Vector2(player.transform.position.x+randomPos,6), Quaternion.Euler(0,0,0));
		}	
	}

	private static void windBlow(GameObject wind, float windSpeed)
	{
		float startingPos = -3.5f + 8.0f*Random.value;
		
		bool isNegative = Mathf.FloorToInt(Random.value*2)==0;
		if(isNegative)
		{
			GameObject windGust = (GameObject)Instantiate (wind, new Vector2(-10,startingPos), Quaternion.Euler(0,0,0));
			Rigidbody2D windGustRbd = windGust.GetComponent<Rigidbody2D>();
			windGustRbd.velocity = new Vector2(windSpeed, windGustRbd.velocity.y); 
		}
		else
		{
			GameObject windGust = (GameObject)Instantiate (wind, new Vector2(10,startingPos), Quaternion.Euler(0,0,0));
			Rigidbody2D windGustRbd = windGust.GetComponent<Rigidbody2D>();
			windGustRbd.velocity = new Vector2(-windSpeed, windGustRbd.velocity.y); 
		}
	}
	// ----------------------------------------------------------------------

	// Goes to one of the two sides of the screen at random -----------------
	private static void randomlyMoveToTheSide(Transform transform, Rigidbody2D robot, float speed)
	{
		bool left = Mathf.FloorToInt(Random.value*2) == 0;
		if(left)
		{
			MoveLeft(transform, robot, sideSpeed);
		}
		else
		{
			MoveRight(transform, robot, sideSpeed);
		}
	}

	// Heals until the HP bar is back to full --------------------------------
	private static bool healUntilAlive(GameObject healthbar, float healingSpeed, float maxHealth)
	{
		healthbar.transform.localScale = new Vector2(healthbar.transform.localScale.x + healingSpeed, healthbar.transform.localScale.y);
		if(healthbar.transform.localScale.x >= maxHealth)
		{
			// Considered dead until full HP
			return false;
		}
		// Considered alive
		return true;
	}

	// BELOW ARE UNITY FUNCTIONS ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
	void Start ()
	{
		//GENERIC
		canvasUI.SetActive(false);
		canvasText = canvasUI.GetComponent<Canvas>().GetComponentInChildren<Text>();
		robot = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
		cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
		state = 0;
		powers = new string[] {"The robot can shoot rockets!", "The robot can now shake the earth itself!", "Frozen rain will fall on the robot's command!", "Mother nature blows strong gusts of wind that don't affect the robot's strong frame!"};
		
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
        if (other.tag == "playerBullet" && !isDead)
        {
			other.SetActive(false);
            spriteRenderer.color = Color.red;
			healthbar.transform.localScale = new Vector2(healthbar.transform.localScale.x - damage, healthbar.transform.localScale.y);
        	if(healthbar.transform.localScale.x <= 0.0f)
			{
				Debug.Log("Robot got KILLED.");
				isDead = true;
			}
		}
    }

    void OnTriggerExit2D(Collider2D otherCollider)
    {
        spriteRenderer.color = Color.white;
    }

	// Update is called once per frame
	void Update ()
	{
		// Robot died at an earlier phase, he's not done fighting !!
		if(isDead && state <=3)
		{
			Debug.Log("Stage complete");
			randomlyMoveToTheSide(transform, robot, speed*3);
			isDead = healUntilAlive(healthbar, 1.0f/(5.0f*20.0f), maxHealth);
			// Robot came back to full HP ! Next phase of the battle starts
			if(!isDead)
			{
				// Pause the game to warn the user about the state change
				canvasUI.SetActive(true);
				state += 1;
				if(state == 3)
				{
					canvasText.text = powers[state];
				}	
				else
				{
					canvasText.text = "Mother Nature has healed the robot and granted it new powers! "+powers[state];
				}
				Time.timeScale = 0.0f;
				Debug.Log("Updating state");
			}
			return;
		}
		// Robot died at his last phase... sadly he's GONE.
		else if(isDead && state > 3)
		{
			Debug.Log("Robot died. :(");
			Physics2D.IgnoreCollision(playerObject.GetComponent<Collider2D>(), GetComponent<Collider2D>());
			return;
		}

		// Randomly determine if it's time to shoot
		if (attackTime(120, "rocket") && !isDead)
		{
			shootBasic(robot, player, bullet);
		}

		if(attackTime(120, "earthquake") && state > 0 && !isDead)
		{
			earthquake();
		}

		if(attackTime(120, "ice") && state > 1 && !isDead)
		{
			iceRain(playerObject, 6, ice);
		}
		
		if(attackTime(120, "wind") && state > 2 && !isDead)
		{
			windBlow(wind, windSpeed);
		}

		// "Basic" AI that follows the player
		if(following)
		{
			FollowAI(player, transform, robot, speed);
		}
		else
		{
			randomlyMoveToTheSide(transform, robot, speed);
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

        // Always make sprite face player
        spriteRenderer.flipX = !isFacingRight;
	}

	void FixedUpdate()
	{
        isFacingRight = (transform.position.x < player.position.x);
	}
}
