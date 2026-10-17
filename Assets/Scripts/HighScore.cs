using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScore : MonoBehaviour {
    public Text highScoreText;
    private const string HighScoreKey = "HighScore";

    void Start() {
        DisplayHighScore();
    }

    public static void SaveScore(int shots) {
        int bestScore = PlayerPrefs.GetInt(HighScoreKey, -1);

        if(bestScore == -1 || shots < bestScore) {
            PlayerPrefs.SetInt(HighScoreKey, shots);
            PlayerPrefs.Save();
        }
    }

    // Display the saved record on the start screen
    public void DisplayHighScore() {
        int bestScore = PlayerPrefs.GetInt(HighScoreKey, -1);

        if(highScoreText != null) {
            if(bestScore == -1) {
                highScoreText.text = "High Score: No record set";
            }
            else {
                highScoreText.text = "High Score: " + bestScore + " shots";
            }
        }
    }
}
