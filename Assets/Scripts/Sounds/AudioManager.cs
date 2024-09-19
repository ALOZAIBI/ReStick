using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager singleton;

    public Sound[] sounds;

    private void Awake() {
        if (singleton == null) {
            singleton = this;
        }
        else {
            Debug.LogError("More than one AudioManager in the scene");
            return;
        }


        foreach (Sound s in sounds) {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    public void Play(GameObject go, string name) {
        Debug.Log("Playing sound: " + name);
        Sound s = System.Array.Find(sounds, sound => sound.name == name);
        if (s == null) {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }

        AudioSource audioSource = go.AddComponent<AudioSource>();
        audioSource.clip = s.clip;
        audioSource.volume = s.volume;
        audioSource.pitch = s.pitch;
        audioSource.loop = s.loop;

        audioSource.Play();
    }

    public void Stop(string name) {
        Debug.Log("Stopping sound: " + name);
        Sound s = System.Array.Find(sounds, sound => sound.name == name);
        if (s == null) {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.Stop();
    }
}
