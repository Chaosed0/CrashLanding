using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EndCanvas : MonoBehaviour {
    public OutroShip outro;
    public float fadeInTime = 1.0f;
    public CanvasGroup panel;

    private float fadeInTimer = 0.0f;

	void Awake () {
        outro.OnOutroEnd += OnOutroEnd;
	}

	void Update () {
        fadeInTimer += Time.deltaTime;
        panel.alpha = fadeInTimer / fadeInTime;

        if (fadeInTimer > fadeInTime) {
            this.enabled = false;
        }
	}

    void OnOutroEnd() {
        this.enabled = true;
        GetComponent<Canvas>().enabled = true;
    }

    public void ToMainMenu() {
        Application.LoadLevel("MainMenu");
    }
}
