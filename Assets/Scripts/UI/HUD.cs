using UnityEngine;
using System.Collections;

public class HUD : MonoBehaviour {
    public GameRules gameRules;
    public Character player;
    public ShipPowerText shipPowerText;
    public HealthText healthText;
    
	void Start () {
        gameRules.OnShipPowerChange += shipPowerText.OnShipPowerChange;
        player.OnHealthChanged += healthText.OnHealthChanged;
        gameRules.OnWin += OnWin;
	}

    private void OnWin() {
        GetComponent<Canvas>().enabled = false;
    }
}
