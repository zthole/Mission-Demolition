using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartScreen : MonoBehaviour
{
    public Text gameOverText;

    // Start the game
    public void Start() {
        // Hide Game Over initially
        gameOverText.gameObject.SetActive(false);
        // Check if the player lost the game
        if (PlayerPrefs.GetInt("GameOver", 0) == 1) {
            gameOverText.gameObject.SetActive(true);

            // Reset the Game Over flag
            PlayerPrefs.SetInt("GameOver", 0);
            PlayerPrefs.Save();
        }
    }

    public void PlayGame() {
        SceneManager.LoadScene("_Scene_0");
    }
}