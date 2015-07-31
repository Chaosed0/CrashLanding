using UnityEngine;
using System.Collections;

public class IntroShip : MonoBehaviour {
    public float crashTime = 3.0f;
    public Player player; 
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
	}

    void OnEnable() {
        lookAtCamera.gameObject.SetActive(true);
        playerCamera.gameObject.SetActive(false);
        audioSource.Play();
    }

	void Update () {
        crashTimer += Time.deltaTime;

        if (crashTimer >= crashTime * 0.95) {
            ship.localPosition = new Vector3(0,0,0);
            tutorial.enabled = true;

            dust.Simulate(20.0f,false,true);
            dust.Play();

            player.GetComponent<Rigidbody>().velocity = new Vector3(0, 10, 0);
            player.setAcceptInput(true);
            audioSource.clip = explosionClip;
            audioSource.Play();

            if (OnIntroOver != null) {
                OnIntroOver();
            }

            enabled = false;
        } else {
            float fraction = (crashTime - crashTimer) / crashTime;
            ship.localPosition = initialPosition * fraction;
            lookAtCamera.transform.LookAt(ship);
        }
	}

    void OnDisable() {
        lookAtCamera.gameObject.SetActive(false);
        playerCamera.gameObject.SetActive(true);
    }
}
