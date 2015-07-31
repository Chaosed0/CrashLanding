using UnityEngine;
using System.Collections;

public class PlayerAudio : MonoBehaviour {
    public GameRules gameRules;
    public AudioSource[] audioSources;

    private int nextSource = 0;

    public void PlaySound(AudioClip clip, bool loop) {
        AudioSource source = audioSources[nextSource];
        source.clip = clip;
        source.loop = loop;
        source.Play();
        nextSource = (nextSource+1)%audioSources.Length;
    }
}
