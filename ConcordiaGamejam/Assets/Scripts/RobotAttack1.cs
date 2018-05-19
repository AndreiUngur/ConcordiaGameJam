using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotAttack1 : MonoBehaviour {

	private GameObject player;
	
    public float bulletDamage;
    public float bulletSpeed = 10f;

    private Transform playerTransform;
    private Vector3 playerPosition;
    private Vector3 direction;
    private Vector3 bulletForward;
    
	// Use this for initialization
	void Start () {
		playerTransform = GameObject.FindWithTag("Player").GetComponent<Transform>();
        playerPosition = playerTransform.position;
        direction = (playerPosition - transform.position).normalized;
	}
	
	// Update is called once per frame
	void Update () {
		transform.position += direction * bulletSpeed * Time.deltaTime;
	}
}
