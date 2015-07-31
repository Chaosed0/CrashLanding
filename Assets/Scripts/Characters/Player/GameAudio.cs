using UnityEngine;
using System.Collections;

public class GameAudio : MonoBehaviour {
    public GameRules gameRules;

    public AudioClip[] shipPowerSounds;
    public AudioSource audioSource;

	void Start () {
        gameRules.OnShipPowerChange += OnShipPowerChange;
	}

    void OnShipPowerChange(int shipPower) {
        if (shipPower%25 == 0) {
            audioSource.clip = shipPowerSounds[shipPower/25 - 1];
            audioSource.Play();
        }
    }
}
