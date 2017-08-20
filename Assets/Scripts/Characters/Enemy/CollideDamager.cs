using UnityEngine;
using System.Collections;

[RequireComponent(typeof (Character))]
public class CollideDamager : MonoBehaviour {
    public int damageOnCollide = 2;
    public bool dieOnCollide = false;
    public float forceOnCollide = 0;

    void OnCollisionEnter(Collision collision) {
        if (!this.enabled) {
            return;
        }

        GameObject player = Util.getPlayer();
        if (collision.gameObject == player) {
            player.GetComponent<Character>().damage(damageOnCollide);

            if (dieOnCollide) {
                this.GetComponent<Character>().setHealth(0);
            }

            if (forceOnCollide > 0) {
                Vector3 forceVector = (player.transform.position - transform.position).normalized;
                forceVector *= forceOnCollide;
                player.GetComponent<Rigidbody>().AddForce(forceVector);
            }
        }
    }
}
