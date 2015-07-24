using UnityEngine;
using System.Collections;

public class ShipTrigger : MonoBehaviour {
    public delegate void PlayerEnteredShipTrigger();
    public event PlayerEnteredShipTrigger OnPlayerEnteredShipTrigger;

    void OnTriggerEnter(Collider collider) {
        if (collider.gameObject == Util.getPlayer() && OnPlayerEnteredShipTrigger != null) {
            OnPlayerEnteredShipTrigger();
        }
    }
}
