using UnityEngine;
using System.Collections;

[RequireComponent(typeof (Player))]
public class PlayerInput : MonoBehaviour {
    private Transform parentTransform;
    private CharacterMotor motor;
    private RigidbodyMotor rigidMotor;
    private Player player;

    public bool fps = false;

    private const float lookSpeed = 50.0f;

	private void Start () {
        motor = GetComponent<CharacterMotor>();
        rigidMotor = GetComponent<RigidbodyMotor>();
        player = GetComponent<Player>();

        lockCursor();
        player.OnStartAcceptingInput += lockCursor;
	}

	private void Update () {
        float hmove = Input.GetAxis("Horizontal");
        float vmove = Input.GetAxis("Vertical");
        float hlook = Input.GetAxis("Mouse X");
        float vlook = Input.GetAxis("Mouse Y");
        bool jump = (bool)Input.GetButtonDown("Jump");
        bool dodge = (bool)Input.GetButtonDown("Dodge");
        bool activate = (bool)Input.GetButtonDown("Use");
        bool fire = (bool)Input.GetButton("Fire1");

        if (!player.isAcceptingInput()) {
            hmove = vmove = hlook = vlook = 0.0f;
            jump = activate = fire = false;
        }

        if (fire) {
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

        if (fps) {
            Camera.main.transform.RotateAround(Camera.main.transform.position, Camera.main.transform.right, -vlook * lookSpeed * Time.deltaTime);
        } else {
            Camera.main.transform.RotateAround(transform.position, transform.right, -vlook * lookSpeed * Time.deltaTime);
        }

        if (fire) {
            player.fireGun();
        }

        if (activate) {
            unlockCursor();
            player.activate();
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
