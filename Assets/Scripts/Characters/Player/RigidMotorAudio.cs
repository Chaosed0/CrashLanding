using UnityEngine;
using System.Collections;

public class RigidMotorAudio : MonoBehaviour {
    public PlayerAudio playerAudio;
    public RigidbodyMotor motor;

    public AudioClip[] gruntSounds;
    public AudioClip[] boostSounds;

	void Start () {
        motor.OnDodge += OnDodge;
        motor.OnJump += OnJump;
	}

    void OnDodge(bool inAir) {
        playerAudio.PlaySound(gruntSounds[Random.Range(0, gruntSounds.Length)], false);
        playerAudio.PlaySound(boostSounds[Random.Range(0, boostSounds.Length)], false);
    }

    void OnJump(bool inAir) {
        playerAudio.PlaySound(gruntSounds[Random.Range(0, gruntSounds.Length)], false);
        if (inAir) {
            playerAudio.PlaySound(boostSounds[Random.Range(0, boostSounds.Length)], false);
        }
    }
}
