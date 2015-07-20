using UnityEngine;
using System.Collections;

public class Skeleton : MonoBehaviour {
    public Fireball fireballPrefab;
    public float fireballTime = 5.0f;

    private float fireballTimer = 0.0f;

	void Update () {
        fireballTimer += Time.deltaTime;

        if (fireballTimer >= fireballTime) {
            Fireball fireball = Instantiate(fireballPrefab, transform.position, transform.rotation) as Fireball;
            fireball.setOwner(this.gameObject);
            fireballTimer = 0.0f;
        }
	}
}
