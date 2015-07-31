using UnityEngine;
using System.Collections;

[RequireComponent(typeof (Character))]
public class RigidbodyMotor : MonoBehaviour {
    private Rigidbody body;
    private Character character;
    
    public Animator anim;
    public bool canDoubleJump = true;

    public float moveSpeed = 10.0f;
    public float turnSpeed = 100.0f;
    public float jumpSpeed = 5.0f;
    public float dodgeSpeed = 100.0f;
    public float friction = 1000.0f;
    public float distToGround = 2.0f;

    private bool isDodging;
    private float hSpeed = 0.0f;
    private Vector3 dodgeDir = new Vector3(0,0,0);
    private bool haveDoubleJump = true;
    private bool isGrounded = false;
    private bool moving = false;

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
        if (character.isDead()) {
            anim.SetFloat("Speed", 0.0f);
            anim.enabled = false;
            return;
        }

        Vector3 velocity = movement * moveSpeed;

        if (anim != null) {
            anim.SetFloat("Speed", velocity.magnitude);
        }

        if (body.useGravity) {
            float yVel = body.velocity.y;
            Vector3 newVelocity = velocity;
            body.velocity = new Vector3(newVelocity.x, yVel, newVelocity.z);
        } else {
            body.velocity = velocity;
        }

        transform.Rotate(0, yaw * turnSpeed * Time.deltaTime, 0);
	}

	public void Move(Vector3 movement, float yaw, bool jump, bool dodge) {
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

        Vector3 velocity = new Vector3(0,0,0);
        float vSpeed = 0.0f;

        RaycastHit hitInfo;
        bool isGrounded = Physics.Raycast(transform.position + new Vector3(0.0f,distToGround,0.0f),
                -Vector3.up, out hitInfo, distToGround + 0.3f);

        if (isGrounded && !this.isGrounded && OnLand != null) {
            OnLand();
        }
        this.isGrounded = isGrounded;

        if (isGrounded && canDoubleJump && !haveDoubleJump) {
            haveDoubleJump = true;
        }

        if (!isDodging) {
            velocity = movement * moveSpeed;
        }

        if (anim != null) {
            anim.SetFloat("Speed", velocity.magnitude);
        }

        if ((isGrounded || haveDoubleJump) && !isDodging) {
            if (anim != null) {
                anim.SetBool("isJumping", false);
            }
            vSpeed = 0.0f;
            if (jump) {
                vSpeed = jumpSpeed;
                if (!isGrounded && haveDoubleJump) {
                    haveDoubleJump = false;
                }
                OnJump(!isGrounded);

                if (anim != null) {
                    anim.SetBool("isJumping", true);
                }
            }
        }

        if (dodge && (isGrounded || haveDoubleJump) && !isDodging) {
            dodgeDir = Vector3.Scale(movement, new Vector3(1,0,1));
            if (dodgeDir.magnitude > 0.01f) {
                isDodging = true;
                hSpeed = dodgeSpeed;
                OnDodge(!isGrounded);
                if (!isGrounded && haveDoubleJump) {
                    haveDoubleJump = false;
                }
            }
        }

        if (isDodging) {
            /* This overrides all other velocity */
            velocity = dodgeDir * hSpeed;
            hSpeed -= friction * Time.deltaTime;
            if (hSpeed <= 1.0f) {
                isDodging = false;
                if (OnStopDodge != null) {
                    OnStopDodge(!isGrounded);
                }
            }
            if (anim != null) {
                anim.SetFloat("dodgeSpeed", dodgeDir.magnitude);
            }
        }

        if (!isDodging) {
            if (anim != null) {
                anim.SetFloat("dodgeSpeed", 0.0f);
            }
        }

        velocity.y = vSpeed;

        if (body.useGravity && velocity.y <= 0.0f) {
            float yVel = body.velocity.y;
            Vector3 newVelocity = velocity;
            body.velocity = new Vector3(newVelocity.x, yVel, newVelocity.z);
        } else {
            body.velocity = velocity;
        }

        transform.Rotate(0, yaw * turnSpeed * Time.deltaTime, 0);
	}
}
