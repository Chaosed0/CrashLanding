using UnityEngine;
using System.Collections;

public class Expires : MonoBehaviour {
    public float expiryTime = 1.0f;
    public bool active = true;
    
    private float expiryTimer = 0.0f;
	
	void Update () {
        if (!active) {
            return;
        }

        expiryTimer += Time.deltaTime;
        if (expiryTimer >= expiryTime) {
            Destroy(gameObject);
        }
	}
}
