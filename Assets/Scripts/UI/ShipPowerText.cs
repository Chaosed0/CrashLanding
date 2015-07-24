using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ShipPowerText : MonoBehaviour {
    public GameRules gameRules;

    private Text text;

	void Start () {
        text = GetComponent<Text>();
        gameRules.OnShipPowerChange += OnShipPowerChange;
	}

    private void OnShipPowerChange(int power) {
        text.text = power + "%";
    }
}
