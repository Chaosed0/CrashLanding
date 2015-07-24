using UnityEngine;
using System.Collections;

public class GameRules : MonoBehaviour {
    public int initialShipPower = 5;
    public int maxShipPower = 100;
    public float maxShipPowerTime = 180.0f;
    public ShipTrigger ship;

    public float startHorses = 60.0f;
    public float startSkeletons = 120.0f;

    public EnemySpawner[] horseSpawners;
    public EnemySpawner[] skeletonSpawners;

    private float timer = 0.0f;
    private bool enabledHorses = false;
    private bool enabledSkeletons = false;
    private bool won = false;
    private int shipPower;

    public delegate void ShipPowerChange(int shipPower);
    public event ShipPowerChange OnShipPowerChange;

    public delegate void Win();
    public event Win OnWin;

	void Start () {
        for (int i = 0; i < horseSpawners.Length; i++) {
            horseSpawners[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < skeletonSpawners.Length; i++) {
            skeletonSpawners[i].gameObject.SetActive(false);
        }
        shipPower = initialShipPower;
	}

	void Update () {
        if (timer >= maxShipPowerTime) {
            return;
        }

        timer += Time.deltaTime;
        if (!enabledHorses && timer >= startHorses) {
            for (int i = 0; i < horseSpawners.Length; i++) {
                horseSpawners[i].gameObject.SetActive(true);
            }
        }
        if (!enabledSkeletons && timer >= startSkeletons) {
            for (int i = 0; i < skeletonSpawners.Length; i++) {
                skeletonSpawners[i].gameObject.SetActive(true);
            }
        }

        int newShipPower = initialShipPower + (int)((1.0f - (maxShipPowerTime - timer) / maxShipPowerTime) * (maxShipPower - initialShipPower));
        if (newShipPower > shipPower && OnShipPowerChange != null) {
            OnShipPowerChange(newShipPower);
        }
        shipPower = newShipPower;

        if (shipPower >= 100) {
            ship.OnPlayerEnteredShipTrigger += WinGame;
        }
	}

    void WinGame() {
        if (!won && OnWin != null) {
            OnWin();
            won = false;
        }
    }
}
