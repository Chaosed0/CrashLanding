using UnityEngine;
using System.Collections;

[RequireComponent(typeof (Character))]
public class RigidbodyMotor_Old : MonoBehaviour {
    private Rigidbody body;
    private Character character;
    
    public Animator anim;
    public bool canDoubleJump = true;

    public float moveSpeed = 10.0f;
    public float moveAcceleration = 100.0f;
    public float moveFriction = 50.0f;
    public float turnSpeed = 100.0f;
    public float jumpSpeed = 5.0f;
    public float dodgeSpeed = 100.0f;
    public float friction = 1000.0f;
    public float distToGround = 2.0f;

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

        Vector3 velocityChange = new Vector3(0,0,0);

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

        if (!isDodging) {
            Vector3 planarVelocity = new Vector3(body.velocity.x, 0.0f, body.velocity.z);

            if (movement.sqrMagnitude >= 0.01f)
            {
                Vector3 accel = movement * moveAcceleration * Time.deltaTime;
                Vector3 projectedPlanarVelocity = Vector3.Project(planarVelocity, accel);
                Vector3 proposedVelocity = projectedPlanarVelocity + accel;
                proposedVelocity = Vector3.ClampMagnitude(proposedVelocity, moveSpeed);

                Vector3 proposedAccel = (proposedVelocity - projectedPlanarVelocity);
                // Make sure the proposed acceleration still points in the same way as the original acceleration
                if (Vector3.Dot(accel.normalized, proposedAccel.normalized) > 0.0f) {
                    velocityChange += proposedAccel;
                }
            }

            // Apply friction
            velocityChange -= Vector3.ClampMagnitude(planarVelocity, moveFriction * Time.deltaTime);
        }

        if ((isGrounded || haveDoubleJump) && !isDodging) {
            if (jump) {
                velocityChange.y = jumpSpeed;
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
            velocityChange = Vector3.ClampMagnitude(body.velocity, body.velocity.magnitude - friction * Time.deltaTime) - body.velocity;
            if (body.velocity.magnitude <= 3.0f) {
                isDodging = false;
                body.useGravity = true;
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
                isDodging = true;
                body.useGravity = false;
                velocityChange = dodgeDir * dodgeSpeed;
                OnDodge(!isGrounded);
                if (!isGrounded && haveDoubleJump) {
                    haveDoubleJump = false;
                }
            }
        }

        body.AddForce(velocityChange, ForceMode.VelocityChange);

        transform.Rotate(0, yaw * turnSpeed * Time.deltaTime, 0);

        if (anim != null) {
            anim.SetFloat("Speed", body.velocity.magnitude);
        }
	}
}
