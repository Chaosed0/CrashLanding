using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Character))]
public class DisableOnDeath : MonoBehaviour {
    public MonoBehaviour[] disable;

    void Start() {
        GetComponent<Character>().OnDied += OnDied;
    }

    private void OnDied() {
        for (int i = 0; i < disable.Length; i++) {
            disable[i].enabled = false;
        }
    }
}
