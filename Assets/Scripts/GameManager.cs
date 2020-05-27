using UnityEngine;

public class GameManager : MonoBehaviour {
    private static GameManager instance;
    public bool endingPrevented = false;
    public bool caught;
    public float goal = 1000;
    public string sceneName;
    
    private void Awake() {
        if (instance == null) instance = this;
        if(sceneName=="") sceneName = "no name"; 
        Time.timeScale = 1;
    }

    public static GameManager getInstance() {
        return instance;
    }

    public void setEndingPrevented(bool state) {
        endingPrevented = state;
    }

    public void endGame() {
        if(!endingPrevented) {
            float totalPoints = UserInterface.instance.getTotalPoints();
        
            if (totalPoints >= goal) {
                if (!caught) {
                    UserInterface.instance.endScreen(true);
                    AudioManager.instance.Play("Win");
                    PlayerPrefs.SetInt(sceneName, 1);
                    PlayerPrefs.Save();
                    Time.timeScale = 0;
                }
            } else {
                endGameForce();
            }
        }
    }
    
    public void endGameForce() {
        UserInterface.instance.endScreen(false);
        AudioManager.instance.Play("Lose");
        PlayerPrefs.Save();
        Time.timeScale = 0;
    }
}