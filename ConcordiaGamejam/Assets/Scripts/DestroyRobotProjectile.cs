using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyRobotProjectile : MonoBehaviour {

	private float maxX = 14.0f;
	private float minX = -14.0f;
	private float maxY = 8.0f;
	private float minY = -8.0f;

	// Use this for initialization
	void Start () {	
	}
	
	// Update is called once per frame
	void Update () {
		if(transform.position.x > maxX || transform.position.x < minX || transform.position.y > maxY || transform.position.y < minY)
		{
			Debug.Log("Deleting object");
			Destroy(transform.root.gameObject);
		}
	}
}
