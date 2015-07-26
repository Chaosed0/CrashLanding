using UnityEngine;
using System.Collections;

public class DisableOnEnd : MonoBehaviour {
    public Character player;
    public GameRules gameRules;

    public GameObject[] objects;

	void Start () {
        gameRules.OnWin += OnEnd;
        player.OnDied += OnEnd;
	}

    void OnEnd() {
        for (int i = 0; i < objects.Length; i++) {
            objects[i].SetActive(false);
        }
    }
}
