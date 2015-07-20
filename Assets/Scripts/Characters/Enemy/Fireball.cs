using UnityEngine;
using System.Collections;

public class Fireball : MonoBehaviour {
    public float accel = 0.5f;
    public float maxSpeed = 10.0f;
    public float angularVel = 10.0f;

    public Transform fireballDissipatePrefab;

    private Rigidbody body;
    private float speed = 0.0f;
    private GameObject owner;

	void Start () {
        body = GetComponent<Rigidbody>();
	}

	void Update () {
        Transform player = Util.getPlayer().transform;
        Vector3 target = player.position + new Vector3(0,2,0);
        Vector3 dir = target - transform.position;
        Vector3 newForward = Vector3.RotateTowards(transform.forward.normalized, dir.normalized,
                angularVel * Time.deltaTime, 0.0f);

        speed = Mathf.Min(speed + accel * Time.deltaTime, maxSpeed);

        transform.rotation = Quaternion.LookRotation(newForward);
        body.velocity = speed * transform.forward.normalized;
	}

    public void setOwner(GameObject owner) {
        this.owner = owner;
    }

    void OnCollisionEnter(Collision collision) {
        GameObject player = Util.getPlayer();
        if (collision.gameObject == owner) {
            return;
        }
        if (collision.gameObject == player) {
            player.GetComponent<Character>().damage(5);
        }

        Instantiate(fireballDissipatePrefab, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
