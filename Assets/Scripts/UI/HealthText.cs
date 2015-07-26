using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthText : MonoBehaviour {
    public Text healthText;
    public Color flashOnChange = Color.yellow;
    public float flashTime = 0.5f;

    private float flashTimer = 0.0f;
    private Color originalColor;

    private void Start() {
        originalColor = healthText.color;
    }

    public void OnHealthChanged(int health, int change) {
        healthText.text = health.ToString();
        flashTimer = 0.0f;
    }

    void Update() {
        if (flashTimer < flashTime) {
            flashTimer += Time.deltaTime;
            healthText.color = Color.Lerp(flashOnChange, originalColor, flashTimer/flashTime);
        }
    }

    void OnEnable() {
        flashTimer = 0.0f;
    }
}
