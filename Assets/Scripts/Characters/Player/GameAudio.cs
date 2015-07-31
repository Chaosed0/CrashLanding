using UnityEngine;
using System.Collections;

public class GameAudio : MonoBehaviour {
    public PlayerAudio playerAudio;
    public GameRules gameRules;

    public AudioClip[] shipPowerSounds;

	void Start () {
        gameRules.OnShipPowerChange += OnShipPowerChange;
	}

    void OnShipPowerChange(int shipPower) {
        if (shipPower%25 == 0) {
            playerAudio.PlaySound(shipPowerSounds[shipPower/25 - 1], false);
        }
    }
}
