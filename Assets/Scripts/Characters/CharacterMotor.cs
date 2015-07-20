using UnityEngine;
using System.Collections;

public class CharacterMotor : MonoBehaviour {
    private CharacterController controller;
    
    public Animator anim;

    private bool isDodging = false;
    private int dodgeDir = 0;

    public float moveSpeed = 5.0f;
    public float turnSpeed = 10.0f;
    public float jumpSpeed = 10.0f;
    public float dodgeSpeed = 30.0f;

    public float gravity = 9.8f;
    public float friction = 50.0f;

    private float vSpeed = 0.0f;
    private float hSpeed = 0.0f;

	private void Start() {
        controller = GetComponent<CharacterController>();
	}

	public void Move(Vector3 movement, float yaw, bool jump, bool dodge) {
        Vector3 velocity = new Vector3(0,0,0);

        if (!isDodging) {
            velocity = movement * moveSpeed;
        }

        if (anim != null) {
            anim.SetFloat("Speed", velocity.magnitude);
        }

        if (controller.isGrounded & !isDodging) {
            if (anim != null) {
                anim.SetBool("isJumping", false);
            }
            vSpeed = 0.0f;
            if (jump) {
                vSpeed = jumpSpeed;
                if (anim != null) {
                    anim.SetBool("isJumping", true);
                }
            }
        }

        if (dodge && controller.isGrounded && !isDodging) {
            isDodging = true;
            if (Vector3.Project(movement, transform.right).x > 0.0f) {
                hSpeed = dodgeSpeed;
                dodgeDir = 1;
            } else {
                hSpeed = -dodgeSpeed;
                dodgeDir = -1;
            }
        }

        if (isDodging && (dodgeDir > 0 ? hSpeed < 0.0f : hSpeed > 0.0f)) {
            isDodging = false;
        }

        vSpeed -= gravity * Time.deltaTime;
        velocity.y = vSpeed;

        if (isDodging) {
            hSpeed = hSpeed - dodgeDir * friction * Time.deltaTime;
            velocity += transform.right.normalized * hSpeed;
            if (anim != null) {
                anim.SetFloat("dodgeSpeed", dodgeDir);
            }
        } else {
            if (anim != null) {
                anim.SetFloat("dodgeSpeed", 0.0f);
            }
        }

        controller.Move(velocity * Time.deltaTime);
        transform.Rotate(0, yaw * turnSpeed * Time.deltaTime, 0);
	}
}
