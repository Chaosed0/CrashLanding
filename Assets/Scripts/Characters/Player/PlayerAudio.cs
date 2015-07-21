using UnityEngine;
using System.Collections;

[RequireComponent(typeof (Character))]
[RequireComponent(typeof (RigidbodyMotor))]
public class PlayerAudio : MonoBehaviour {
    public AudioSource[] audioSources;

    public AudioClip[] gruntSounds;
    public AudioClip[] boostSounds;

    private int nextSource = 0;
    private RigidbodyMotor motor;

	void Start () {
        motor = GetComponent<RigidbodyMotor>();
        motor.OnDodge += OnDodge;
        motor.OnJump += OnJump;
	}

    void OnDodge(bool inAir) {
        PlaySound(gruntSounds[Random.Range(0, gruntSounds.Length)], false);
        PlaySound(boostSounds[Random.Range(0, boostSounds.Length)], false);
    }

    void OnJump(bool inAir) {
        PlaySound(gruntSounds[Random.Range(0, gruntSounds.Length)], false);
        if (inAir) {
            PlaySound(boostSounds[Random.Range(0, boostSounds.Length)], false);
        }
    }

    void PlaySound(AudioClip clip, bool loop) {
        AudioSource source = audioSources[nextSource];
        source.clip = clip;
        source.loop = loop;
        source.Play();
        nextSource = (nextSource+1)%audioSources.Length;
    }
}
