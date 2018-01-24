using UnityEngine;
using System.Collections;

public class Flyer : MonoBehaviour {
    public float flightHeight = 5.0f;
    public float followDistance = 5.0f;

    private RigidbodyMotor rigidMotor;

    void Start () {
        rigidMotor = GetComponent<RigidbodyMotor>();
    }

	void Update () {
        Transform player = Util.getPlayer().transform;
        Vector3 target = player.position + new Vector3(0,flightHeight,0);
        Vector3 dir = target - transform.position;
        Vector3 flatDir = Vector3.Scale(dir, new Vector3(1,0,1));

        float angle = Util.angleTo(transform.forward, flatDir);
        float yaw = 0.0f;
        if (Mathf.Abs(angle) > 5.0f) {
            yaw = Mathf.Sign(angle);
        }

        if (flatDir.magnitude <= followDistance) {
            flatDir = Vector3.zero;
        } else {
            flatDir = flatDir.normalized;
        }

        Vector3 movement = flatDir + new Vector3(0,dir.y,0);
        rigidMotor.Move(movement, yaw);
	}
}
