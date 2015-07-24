using UnityEngine;
using System.Collections;

public class IntroShip : MonoBehaviour {
    public float crashTime = 3.0f;
    public Rigidbody player; 
    public Camera lookAtCamera;
    public Camera playerCamera;
    public Tutorial tutorial;

    private float crashTimer = 0.0f;
    private Vector3 initialPosition;

    public delegate void IntroOver();
    public event IntroOver OnIntroOver;

	void Start () {
        initialPosition = transform.rotation * new Vector3(0, 0, -200);
        transform.localPosition = initialPosition;
        lookAtCamera.gameObject.SetActive(true);
        playerCamera.gameObject.SetActive(false);
	}

	void Update () {
        crashTimer += Time.deltaTime;

        if (crashTimer >= crashTime) {
            transform.localPosition = new Vector3(0,0,0);
            lookAtCamera.gameObject.SetActive(false);
            playerCamera.gameObject.SetActive(true);
            tutorial.gameObject.SetActive(true);
            if (OnIntroOver != null) {
                player.velocity = new Vector3(0, 10, 0);
                OnIntroOver();
            }
            enabled = false;
        }

        float fraction = (crashTime - crashTimer) / crashTime;
        transform.localPosition = initialPosition * fraction;
        lookAtCamera.transform.LookAt(transform);
	}
}
