using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerHealth : MonoBehaviour {

    public Texture heart;
    GameObject player;
    Movement movement;

	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        movement = player.GetComponent<Movement>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnGUI()
    {
        if (movement != null)
        {
            for (int i = 0; i < movement.currentHealth; i++)
            {
                Rect pos = new Rect(5, i * 25 + 5, 25, 25);
                GUI.DrawTexture(pos, heart, ScaleMode.ScaleToFit);
            }
        }
    }
}
