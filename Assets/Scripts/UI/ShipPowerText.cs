using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ShipPowerText : MonoBehaviour {
    public Text shipPowerText;

    public void OnShipPowerChange(int power) {
        shipPowerText.text = power + "%";
    }
}
