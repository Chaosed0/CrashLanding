using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Character))]
[RequireComponent (typeof (RigidbodyMotor))]
public class Player : MonoBehaviour {
    public bool acceptingInput = false;
    public Gun gun;

    private Character character;

    void Start() {
        GetComponent<Character>().OnDied += OnDied;
        character = GetComponent<Character>();
    }

    public void winGame() {
        setAcceptInput(false);
        gun.gameObject.SetActive(false);
        character.invincible = true;
    }

    public void setAcceptInput(bool accept) {
        acceptingInput = accept;
    }

    public bool isAcceptingInput() {
        return acceptingInput;
    }

    public void setFiring(bool firing) {
        gun.setFiring(firing);
    }

    private void OnDied() {
        acceptingInput = false;
    }
}
