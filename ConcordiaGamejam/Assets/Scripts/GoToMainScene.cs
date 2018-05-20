using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GoToMainScene : MonoBehaviour
{

    private Text t;
    private Button button;
    private GameObject parentCanvas;

    // Use this for initialization
    void Start()
    {
        parentCanvas = GameObject.FindGameObjectWithTag("menuCanvas");
        t = GetComponentInChildren<Text>();
        button = GetComponent<Button>();
        button.onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick()
    {
        SceneManager.LoadScene("MainScene", LoadSceneMode.Single);
    }
}