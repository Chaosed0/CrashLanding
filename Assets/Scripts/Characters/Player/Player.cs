using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Character))]
public class Player : MonoBehaviour {
    private bool acceptingInput = false;

    public IntroShip intro;
    public Gun gun;

    void Start() {
        GetComponent<Character>().OnDied += OnDied;
        intro.OnIntroOver += startAcceptInput;
    }

    private void startAcceptInput() {
        acceptInput(true);
    }

    private void acceptInput(bool accept) {
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
