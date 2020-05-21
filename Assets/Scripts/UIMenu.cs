using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIMenu : MonoBehaviour {
    public GameObject options;
    public bool resetGameOnAwake;
    
    private void Awake() {
        if(resetGameOnAwake){ // Set true if you want to reset if levels completed
            PlayerPrefs.DeleteAll();
        }

        if (PlayerPrefs.GetInt("menu")==1) {
            options.SetActive(true);
            gameObject.SetActive(false);
        } else {
            options.SetActive(false);
            gameObject.SetActive(true);
        }
    }

    public void playGame(int i) {
        PlayerPrefs.SetInt("menu",1);
        PlayerPrefs.Save();
        SceneManager.LoadScene(i);
        //Application.LoadLevel("Pathfinding");
    }

    public void openOptions() {
        options.SetActive(true);
        gameObject.SetActive(false);
    }
    
    public void closeOptions() {
        PlayerPrefs.SetInt("menu",0);
        PlayerPrefs.Save();
        options.SetActive(false);
        gameObject.SetActive(true);
    }
    
    public void resetGame() {
        PlayerPrefs.DeleteAll();
        options.SetActive(false);
        gameObject.SetActive(true);
        exitGame();
    }

    public void exitGame() {
        Application.Quit();
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
