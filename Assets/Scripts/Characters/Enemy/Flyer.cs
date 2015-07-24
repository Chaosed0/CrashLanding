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
        float height = player.position.y + flightHeight;
        Vector3 target = Vector3.Scale(player.position, new Vector3(1,0,1)) + new Vector3(0,player.position.y+height,0);
        Vector3 dir = target - transform.position;
        Vector3 flatDir = Vector3.Scale(dir, new Vector3(1,0,1));

        float angle = Util.angleTo(transform.forward, flatDir);
        float yaw = 0.0f;
        if (Mathf.Abs(angle) > 5.0f) {
            yaw = Mathf.Sign(angle);
        }

        if (dir.magnitude <= followDistance) {
            dir = new Vector3(0,dir.y,0);
        }
        rigidMotor.Move(dir.normalized, yaw);
	}
}
