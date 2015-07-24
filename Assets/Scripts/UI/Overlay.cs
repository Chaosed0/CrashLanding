using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Overlay : MonoBehaviour {
    public Character player;
    public IntroShip intro;
    public Image flashImage;

    private float flashTime = 0.0f;
    private float flashTimer = 0.0f;
    private Color flashColor;
    private float flashStartAlpha;

	void Start () {
		GetComponent<Canvas>().enabled = false;
        player.OnHealthChanged += OnHealthChanged;
        intro.OnIntroOver += OnIntroOver;
	}

	void Update () {
        if (flashTimer < flashTime) {
            flashTimer += Time.deltaTime;
            flashColor.a = flashStartAlpha * (flashTime - flashTimer) / flashTime;
            flashImage.color = flashColor;
        }
	}

    void Flash(float time, Color color) {
        flashTime = time;
        flashTimer = 0.0f;
        flashColor = color;
        flashStartAlpha = color.a;
    }

    void OnHealthChanged(int health, int change) {
        if (change >= 0) {
            return;
        }

        Color color = new Color(1.0f, 0.3f, 0.1f, -change * 0.1f);
        Flash(-change/10.0f, color);
    }

    void OnIntroOver() {
		GetComponent<Canvas>().enabled = true;
        Flash(4.0f, Color.white);
    }
}
