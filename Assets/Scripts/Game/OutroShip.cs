using UnityEngine;
using System.Collections;

public class OutroShip : MonoBehaviour {
    public PathNode[] path;
    public Transform ship;
    public GameRules gameRules;
    public AudioSource audioSource;

    public Camera lookAtCamera;
    public Camera playerCamera;

    private float timer = 0.0f;
    private int target = 1;
    private bool driving = true;
    private Quaternion rot;
    private Quaternion nextRot;

    void Awake() {
        gameRules.OnWin += startOutro;
    }

    void startOutro() {
        this.enabled = true;
    }

    void OnEnable() {
        rot = ship.rotation;
        nextRot = Quaternion.LookRotation(path[2].transform.position - path[1].transform.position);
        if (path[0].clip != null) {
            audioSource.clip = path[0].clip;
            audioSource.Play();
        }
        lookAtCamera.gameObject.SetActive(true);
        playerCamera.gameObject.SetActive(false);
    }

	void Update () {
        timer += Time.deltaTime;
        lookAtCamera.transform.LookAt(ship);

        if (driving) {
            Vector3 n1 = path[target-1].transform.position;
            Vector3 n2 = path[target].transform.position;
            float driveTime = path[target].timeToReach;
            float fraction = timer / driveTime;
            ship.position = Vector3.Lerp(n1, n2, easeInOutQuad(fraction));
            ship.rotation = Quaternion.Slerp(rot, nextRot, easeInOutQuad(fraction));
            
            if (timer >= driveTime) {
                driving = false;
                timer = 0.0f;
            }
        }

        if (!driving) {
            if (timer >= path[target].timeToPause) {
                if (path[target].clip != null) {
                    audioSource.clip = path[target].clip;
                    audioSource.Play();
                }

                driving = true;
                timer = 0.0f;
                target++;
                if (target >= path.Length) {
                    target = 0;
                    this.enabled = false;
                } else {
                    rot = nextRot;
                    if (path[target].rotateTowardsNext && target+1 <= path.Length) {
                        nextRot = Quaternion.LookRotation(path[target+1].transform.position - path[target+1].transform.position);
                    }
                }

            }
        }
	}


    float easeInOutQuad(float t) {
        if (t < 0.5) return 2*t*t;
        return -1+(4-2*t)*t;
    }
}
