using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Character))]
[RequireComponent (typeof (RigidbodyMotor))]
public class Player : MonoBehaviour {
    public bool acceptingInput = false;
    public Gun gun;

    private Character character;
    private RigidbodyMotor motor;
    private bool moving = false;

    void Start() {
        GetComponent<Character>().OnDied += OnDied;
        character = GetComponent<Character>();
        motor = GetComponent<RigidbodyMotor>();

        motor.OnJump += DisableGunBob;
        motor.OnDodge += DisableGunBob;
        motor.OnLand += EnableGunBob;
        motor.OnStopDodge += EnableGunBobIfGrounded;
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
        gun.setBobbing(moving);
    }

    private void EnableGunBobIfGrounded(bool inAir) {
        if (!inAir) {
            EnableGunBob();
        }
    }

    private void EnableGunBob() {
        gun.pullBack(false);
        if (moving) {
            gun.setBobbing(true);
        }
    }

    private void DisableGunBob(bool inAir) {
        gun.pullBack(true);
    }

    private void OnDied() {
        acceptingInput = false;
    }
}
