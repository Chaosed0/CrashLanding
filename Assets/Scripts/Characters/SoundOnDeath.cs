using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Character))]
public class SoundOnDeath : MonoBehaviour {
    public AudioSource audioSource;
    public AudioClip clip;

    void Start() {
        GetComponent<Character>().OnDied += OnDied;
    }

    void OnDied() {
        if (clip == null) {
            audioSource.Stop();
        } else {
            audioSource.clip = clip;
            audioSource.loop = false;
            audioSource.Play();
        }
    }
}
