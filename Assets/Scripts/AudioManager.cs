using System;
using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour {
    public Sound[] sounds;
    public static AudioManager instance;

    private void Awake() {
        if (instance == null)
            instance = this;
        else {
            Destroy(gameObject);
            return;
        }
        
        //DontDestroyOnLoad(gameObject);

        foreach (Sound s in sounds) {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.playOnAwake = false;
            s.source.clip = s.clip;

            s.source.pitch = s.pitch;
            s.source.volume = s.volume;
            s.source.loop = s.loop;
        }
    }

    public void Play(string name) {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
            return;
        Debug.Log("Sound name (play):  " + s.name);
        s.source.Play();
    }
    
    public bool isPlaying(string name) {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
            return false;
        Debug.Log("Sound name (isPlaying):  " + s.name);
        return s.source.isPlaying;
    }
    
    public void Stop(string name) {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
            return;
        s.source.Stop();
    }

    // these are placed from ALARM because we need monobehavior, this is unclean coding
    public void playAlarm(GameObject obj) {
        
        StartCoroutine("wait",obj);
    }
    
    private IEnumerator wait(GameObject obj) {
        UserInterface.instance.playerMessage.text = "Alarm will go off in 3 seconds, hide!";
        yield return new WaitForSeconds(3);
        UserInterface.instance.playerMessage.text = "";
        Play("FireAlarms");
        yield return new WaitForSeconds(1);
        foreach (AIController controller in GameObject.FindObjectsOfType<AIController>()) {
            controller.listenSounds(obj.transform.position);
        }
        yield return 0;
    }
}