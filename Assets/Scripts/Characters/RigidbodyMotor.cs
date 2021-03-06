﻿using UnityEngine;
using System.Collections;

[RequireComponent(typeof (Character))]
public class RigidbodyMotor : MonoBehaviour {
    private Rigidbody body;
    private Character character;
    
    public Animator anim;
    public bool canDoubleJump = true;
    public bool isFlying = false;

    public float maxMoveSpeed = 10.0f;
    public float moveForce = 1000.0f;
    public float turnSpeed = 100.0f;
    public float jumpForce = 1000.0f;
    public float dodgeForce = 10000.0f;
    public float distToGround = 2.0f;
    public float dodgeFriction = 15.0f;

    private bool isDodging;
    private Vector3 dodgeDir = new Vector3(0,0,0);
    private bool haveDoubleJump = true;
    private bool isGrounded = false;
    private bool moving = false;

    private Vector3 movement;
    private float yaw;
    private bool jump;
    private bool dodge;

    public delegate void StartMove();
    public event StartMove OnStartMove;

    public delegate void StopMove();
    public event StopMove OnStopMove;

    public delegate void Dodge(bool inAir);
    public event Dodge OnDodge;

    public delegate void StopDodge(bool inAir);
    public event StopDodge OnStopDodge;

    public delegate void Jump(bool inAir);
    public event Jump OnJump;

    public delegate void Land();
    public event Land OnLand;

	private void Start() {
        body = GetComponent<Rigidbody>();
        character = GetComponent<Character>();
        if (!canDoubleJump) {
            haveDoubleJump = false;
        }
	}

	public void Move(Vector3 movement, float yaw) {
        if (movement.magnitude >= 1.0f)
        {
            movement = movement.normalized;
        }

        this.movement = movement;
        this.yaw = yaw;
        this.jump = false;
        this.dodge = false;
	}

	public void Move(Vector3 movement, float yaw, bool jump, bool dodge) {
        if (movement.magnitude >= 1.0f)
        {
            movement = movement.normalized;
        }

        this.movement = movement;
        this.yaw = yaw;
        this.jump = jump;
        this.dodge = dodge;
    }

    void FixedUpdate()
    {
        if (character.isDead()) {
            if (anim) {
                anim.SetFloat("Speed", 0.0f);
                anim.enabled = false;
            }
            return;
        }

        bool moving = movement.magnitude >= 0.01f;
        if (moving != this.moving) {
            if (moving && OnStartMove != null) {
                OnStartMove();
            }
            if (!moving && OnStopMove != null) {
                OnStopMove();
            }
            this.moving = moving;
        }

        RaycastHit hitInfo;
        bool isGrounded = Physics.Raycast(transform.position + new Vector3(0.0f, distToGround, 0.0f), -Vector3.up, out hitInfo, distToGround + 0.25f);

        if (isGrounded && !this.isGrounded) {
            if (OnLand != null) {
                OnLand();
            }

            if (anim != null) {
                anim.SetBool("isJumping", false);
            }
        }

        this.isGrounded = isGrounded;

        if (isGrounded && canDoubleJump && !haveDoubleJump) {
            haveDoubleJump = true;
        }

        Vector3 planarVelocity = new Vector3(body.velocity.x, 0.0f, body.velocity.z);
        if (!isDodging) {
            Vector3 consideredVelocity;
            Vector3 desiredVelocity;
            if (!isFlying)
            {
                consideredVelocity = planarVelocity;
                desiredVelocity = Vector3.Scale(movement, new Vector3(1, 0, 1)).normalized * maxMoveSpeed;
            }
            else
            {
                consideredVelocity = body.velocity;
                desiredVelocity = movement.normalized * maxMoveSpeed;
            }

            Vector3 relativeForce = (desiredVelocity - consideredVelocity) / maxMoveSpeed * moveForce;
            body.AddForce(relativeForce);
        }

        if ((isGrounded || haveDoubleJump) && !isDodging) {
            if (jump) {
                jump = false;
                body.AddForce(jumpForce * Vector3.up, ForceMode.Impulse);

                if (!isGrounded && haveDoubleJump) {
                    haveDoubleJump = false;
                }
                OnJump(!isGrounded);

                if (anim != null) {
                    anim.SetBool("isJumping", true);
                }
            }
        }

        if (isDodging) {
            body.AddForce(-dodgeFriction * new Vector3(Mathf.Sign(planarVelocity.x) * planarVelocity.x * planarVelocity.x, 0.0f, Mathf.Sign(planarVelocity.z) * planarVelocity.z * planarVelocity.z));

            if (body.velocity.magnitude <= maxMoveSpeed * 0.75f) {
                isDodging = false;
                dodgeDir = Vector3.zero;
                if (OnStopDodge != null) {
                    OnStopDodge(!isGrounded);
                }
            }

            if (anim != null) {
                anim.SetFloat("dodgeSpeed", dodgeDir.magnitude);
            }
        }

        if (dodge && (isGrounded || haveDoubleJump) && !isDodging) {
            dodgeDir = Vector3.Scale(movement, new Vector3(1,0,1));
            if (dodgeDir.magnitude > 0.01f) {
                body.AddForce(dodgeDir * dodgeForce, ForceMode.Impulse);

                isDodging = true;
                OnDodge(!isGrounded);
                if (!isGrounded && haveDoubleJump) {
                    haveDoubleJump = false;
                }
            }
        }

        transform.Rotate(0, yaw * turnSpeed * Time.deltaTime, 0);

        if (anim != null) {
            anim.SetFloat("Speed", body.velocity.magnitude);
        }
	}
}
