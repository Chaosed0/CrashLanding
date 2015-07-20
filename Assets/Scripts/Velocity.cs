using UnityEngine;
using System.Collections;

public class Velocity : MonoBehaviour {
    public float speed = 100.0f;

	void Start () {
	}

	void Update () {
        transform.position += transform.forward.normalized * speed * Time.deltaTime;
	}
}
