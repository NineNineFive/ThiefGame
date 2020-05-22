using UnityEngine;
public class BackgroundMusic : MonoBehaviour {
    private static BackgroundMusic instance;
    void Awake() {
        if(instance==null) {
	        instance = this;
            instance.GetComponent<AudioSource>().Play();
        }
        DontDestroyOnLoad(instance.gameObject);
    }
}
