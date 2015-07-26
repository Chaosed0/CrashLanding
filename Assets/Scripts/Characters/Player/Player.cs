using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Character))]
public class Player : MonoBehaviour {
    private bool acceptingInput = false;
    private Character character;

    public IntroShip intro;
    public GameRules gameRules;
    public Gun gun;

    void Start() {
        GetComponent<Character>().OnDied += OnDied;
        intro.OnIntroOver += startAcceptInput;
        gameRules.OnWin += winGame;
        character = GetComponent<Character>();
    }

    private void startAcceptInput() {
        acceptInput(true);
    }

    private void winGame() {
        acceptInput(false);
        gun.gameObject.SetActive(false);
        character.invincible = true;
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
