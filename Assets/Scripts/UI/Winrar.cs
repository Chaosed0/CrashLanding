using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Winrar : MonoBehaviour {
    public GameRules gameRules;

	void Start () {
        gameRules.OnWin += OnWin;
	}

    private void OnWin() {
        Debug.Log("WINRAR");
        Text text = GetComponent<Text>();
        Color color = text.color;
        color.a = 1.0f;
        text.color = color;
    }
}
