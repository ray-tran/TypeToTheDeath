using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScoreHandler : MonoBehaviour
{   
    private int highScore = 0;

    void Start()
    {
        if (PlayerPrefs.HasKey("HighScore"))
        {
            highScore = PlayerPrefs.GetInt("HighScore");
        }
        //PlayerPrefs.SetInt("HighScore", highScore);
    }

    // Update is called once per frame
    void Update()
    {
        Text gt = this.GetComponent<Text>();
        gt.text = "High Score: " + highScore;
        if (highScore > PlayerPrefs.GetInt("HighScore"))
        {
            print("high score has been updated");
            PlayerPrefs.SetInt("HighScore", highScore);
        }
    }
}
