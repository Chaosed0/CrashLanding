using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthSliderController : MonoBehaviour {
    public Character character;
    private Slider slider;

    private void Start() {
        character.OnHealthChanged += onHealthChanged;
        slider = GetComponent<Slider>();
        slider.value = character.getHealth();
    }

    void onHealthChanged(int health, int change) {
        slider.value = health;
    }
}
