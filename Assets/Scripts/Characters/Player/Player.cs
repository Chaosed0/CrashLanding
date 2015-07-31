using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Character))]
[RequireComponent (typeof (RigidbodyMotor))]
public class Player : MonoBehaviour {
    public bool acceptingInput = false;
    public Gun gun;

    private Character character;
    private RigidbodyMotor motor;
    private bool canBob = true;
    private bool moving = false;

    void Start() {
        GetComponent<Character>().OnDied += OnDied;
        character = GetComponent<Character>();
        motor = GetComponent<RigidbodyMotor>();

        motor.OnJump += DisableGunBob;
        motor.OnDodge += DisableGunBob;
        motor.OnLand += EnableGunBob;
        motor.OnStopDodge += EnableGunBob;
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

    public void setMoving(bool moving) {
        this.moving = moving;
        if (canBob) {
            gun.setBobbing(moving);
        }
    }

    private void EnableGunBob() {
        if (moving) {
            gun.setBobbing(true);
            canBob = true;
        }
    }

    private void DisableGunBob(bool inAir) {
        gun.setBobbing(false);
        canBob = false;
    }

    private void OnDied() {
        acceptingInput = false;
    }
}
