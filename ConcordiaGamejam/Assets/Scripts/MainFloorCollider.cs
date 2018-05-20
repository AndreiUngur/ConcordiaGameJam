using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainFloorCollider : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GameObject[] floorTiles = GameObject.FindGameObjectsWithTag("floorTile");
        foreach (GameObject tile in floorTiles)
        {
			Physics2D.IgnoreCollision(tile.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
