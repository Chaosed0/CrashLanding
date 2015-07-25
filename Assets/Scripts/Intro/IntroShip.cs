using UnityEngine;
using System.Collections;

public class IntroShip : MonoBehaviour {
    public float crashTime = 3.0f;
    public Rigidbody player; 
    public Camera lookAtCamera;
    public Camera playerCamera;
    public Tutorial tutorial;
    public AudioSource audioSource;
    public AudioClip explosionClip;
    public Transform ship;
    public ParticleSystem dust;

    private float crashTimer = 0.0f;
    private Vector3 initialPosition;

    public delegate void IntroOver();
    public event IntroOver OnIntroOver;

	void Start () {
        initialPosition = ship.rotation * new Vector3(0, 0, -200);
        ship.localPosition = initialPosition;
        lookAtCamera.gameObject.SetActive(true);
        playerCamera.gameObject.SetActive(false);
	}

	void Update () {
        crashTimer += Time.deltaTime;

        if (crashTimer >= crashTime * 0.95) {
            ship.localPosition = new Vector3(0,0,0);
            lookAtCamera.gameObject.SetActive(false);
            playerCamera.gameObject.SetActive(true);
            tutorial.enabled = true;
            dust.Play();
            dust.Simulate(10.0f,false,false);
            dust.Play();
            if (OnIntroOver != null) {
                player.velocity = new Vector3(0, 10, 0);
                audioSource.clip = explosionClip;
                audioSource.Play();
                OnIntroOver();
            }
            enabled = false;
        } else {
            float fraction = (crashTime - crashTimer) / crashTime;
            ship.localPosition = initialPosition * fraction;
            lookAtCamera.transform.LookAt(ship);
        }
	}
}
