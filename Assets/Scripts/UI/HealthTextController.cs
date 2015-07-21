using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthTextController : MonoBehaviour {
    public Character character;
    public Text healthText;
    public Text maxHealthText;
    public Color flashOnChange = Color.yellow;
    public float flashTime = 0.5f;

    private float flashTimer = 0.0f;
    private Color originalColor;

    private void Start() {
        character.OnHealthChanged += onHealthChanged;
        healthText.text = character.getHealth().ToString();
        maxHealthText.text = character.maxHealth.ToString();
        originalColor = healthText.color;
    }

    void onHealthChanged(int health) {
        healthText.text = health.ToString();
        flashTimer = 0.0f;
    }

    void Update() {
        if (flashTimer < flashTime) {
            flashTimer += Time.deltaTime;
            healthText.color = Color.Lerp(flashOnChange, originalColor, flashTimer/flashTime);
        }
    }
}
