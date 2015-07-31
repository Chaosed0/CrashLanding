using UnityEngine;
using System.Collections;

public class GunAudio : MonoBehaviour {
    public PlayerAudio playerAudio;
    public Gun gun;

    public AudioClip[] gunClips;

	void Start () {
        gun.OnFire += OnFire;
	}

    void OnFire() {
        playerAudio.PlaySound(gunClips[Random.Range(0, gunClips.Length)], false);
    }
}
