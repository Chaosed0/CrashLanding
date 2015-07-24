using UnityEngine;
using System.Collections;
using Pathfinding;

public class PlayerFollower : MonoBehaviour {
    public float repathTime = 1.0f;
    public float waypointCloseDistance = 1.0f;

    private float repathTimer = 1.0f;

    private Seeker seeker;
    private Path currentPath = null;
    private int waypointIdx;

    private bool pathing = false;

    private CharacterMotor motor;
    private RigidbodyMotor rigidMotor;

    void Start () {
        repathTimer = repathTime;
        waypointIdx = -1;

        seeker = GetComponent<Seeker>();
        motor = GetComponent<CharacterMotor>();
        rigidMotor = GetComponent<RigidbodyMotor>();
    }

    void repath () {
        if (!pathing) {
            GameObject player = Util.getPlayer();
            Vector3 towardPlayer = player.transform.position - transform.position + new Vector3(0,1.5f,0);
            RaycastHit hitInfo;
            bool hit = Physics.Raycast(transform.position, towardPlayer, out hitInfo, 200.0f);
            if (!hit || player != hitInfo.transform.gameObject) {
                seeker.StartPath (transform.position, player.transform.position, OnPathComplete);
                pathing = true;
            } else {
                currentPath = null;
            }
        }
        repathTimer = 0.0f;
    }

	void Update () {
        Vector3 target = new Vector3(0,0,0);
        float yaw = 0.0f;

        repathTimer += Time.deltaTime;

        if (repathTimer >= repathTime) {
            repath();
        }

        if (currentPath != null) { 
            if (waypointIdx < currentPath.vectorPath.Count) {
                target = currentPath.vectorPath[waypointIdx];

                if (Vector3.Distance(transform.position, target) < waypointCloseDistance) {
                    waypointIdx++;
                }
            } else {
                currentPath = null;
            }
        } else {
            GameObject player = Util.getPlayer();
            target = player.transform.position;
        }

        Vector3 dir = target - transform.position;
        Vector3 flatDir = Vector3.Scale(dir, new Vector3(1,0,1));
        float angle = Util.angleTo(transform.forward, flatDir);
        if (Mathf.Abs(angle) > 5.0f) {
            yaw = Mathf.Sign(angle);
        }

        if (motor != null) {
            motor.Move(dir.normalized, yaw, false, false);
        } else if (rigidMotor != null) {
            rigidMotor.Move(dir.normalized, yaw);
        }
	}

    public void OnPathComplete(Path p) {
        currentPath = p;
        waypointIdx = 0;
        pathing = false;
    }
}
