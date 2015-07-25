using UnityEngine;
using System.Collections;

[RequireComponent(typeof (Character))]
public class CollideDamager : MonoBehaviour {
    public int damageOnCollide = 2;
    public bool dieOnCollide = false;

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
        }
    }
}
