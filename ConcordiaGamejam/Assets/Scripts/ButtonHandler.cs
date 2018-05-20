using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonHandler : MonoBehaviour {

	private Text t;
	private Button button;
	private GameObject parentCanvas;

	// Use this for initialization
	void Start () {
		parentCanvas = GameObject.FindGameObjectWithTag("MainCanvas");
		t = GetComponentInChildren<Text>();
		button = GetComponent<Button>();
		button.onClick.AddListener(TaskOnClick);
		t.color = Color.white;
		t.text = "Got it!"; 
	}
	
    void TaskOnClick()
    {
		Time.timeScale = 1;
		parentCanvas.SetActive(false);
    }
}
