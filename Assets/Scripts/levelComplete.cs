using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class levelComplete : MonoBehaviour {
    public string sceneName;
    void Awake() {
        int levelCompleted = PlayerPrefs.GetInt(sceneName);
        if (levelCompleted==1) {
            GetComponent<Image>().color = Color.green;
        } else if(levelCompleted==0) {
            GetComponent<Image>().color = Color.red;
        }
    }
}
