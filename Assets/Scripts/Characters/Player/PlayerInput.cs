using UnityEngine;
using System.Collections;

[RequireComponent(typeof (Player))]
[RequireComponent(typeof (Character))]
public class PlayerInput : MonoBehaviour {
    private Transform parentTransform;
    private CharacterMotor motor;
    private RigidbodyMotor rigidMotor;
    private Player player;
    private Character character;

    public Camera playerCamera;
    public bool fps = false;
    public bool invert = false;

    private const float lookSpeed = 50.0f;
    private float yaw = 0.0f;

	private void Start () {
        motor = GetComponent<CharacterMotor>();
        rigidMotor = GetComponent<RigidbodyMotor>();
        player = GetComponent<Player>();
        character = GetComponent<Character>();

        GameObject sharedObject = GameObject.Find("SharedLevelObject");
        if (sharedObject) {
            invert = sharedObject.GetComponent<SharedLevelObject>().invertMouse;
        }

        lockCursor();
	}

	private void Update () {
        float hmove = Input.GetAxis("Horizontal");
        float vmove = Input.GetAxis("Vertical");
        float hlook = Input.GetAxis("Mouse X");
        float vlook = Input.GetAxis("Mouse Y");
        bool jump = (bool)Input.GetButtonDown("Jump");
        bool dodge = (bool)Input.GetButtonDown("Dodge");
        bool startFire = (bool)Input.GetButtonDown("Fire1");
        bool stopFire = (bool)Input.GetButtonUp("Fire1");

        if (!player.isAcceptingInput()) {
            if (character.isDead() && startFire) {
                /* This really is the worst place for this, oh well */
                GameObject.Find("SharedLevelObject").GetComponent<SharedLevelObject>().nextLevel = "Dungeon";
                Application.LoadLevel("LoadingScene");
            }
            hmove = vmove = hlook = vlook = 0.0f;
            jump = startFire = false;
            stopFire = true;
        }

        if (startFire) {
            lockCursor();
        }

        Vector3 forward = Vector3.Scale(transform.forward, new Vector3(1,0,1)).normalized;
        Vector3 sideways = Vector3.Scale(transform.right, new Vector3(1,0,1)).normalized;
        Vector3 movement = forward * vmove + sideways * hmove;

        if (motor != null) {
            motor.Move(movement, hlook, jump, dodge);
        } else {
            rigidMotor.Move(movement, hlook, jump, dodge);
        }

        if (invert) {
            vlook = -vlook;
        }

        float newYaw = Mathf.Clamp(yaw + vlook * lookSpeed * Time.deltaTime, -80, 80);
        float change = newYaw - yaw;

        if (fps) {
            playerCamera.transform.RotateAround(playerCamera.transform.position, playerCamera.transform.right, -change);
        } else {
            playerCamera.transform.RotateAround(transform.position, transform.right, -change);
        }

        yaw = newYaw;

        if (startFire) {
            player.setFiring(true);
        } else if (stopFire) {
            player.setFiring(false);
        }
	}

    private void lockCursor () {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void unlockCursor () {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
