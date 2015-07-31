using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Gun))]
public class GunAnimator : MonoBehaviour {
    public Vector3 recoil = new Vector3(-0.5f, 0.0f, 0.0f);
    public Vector3 recoilRot = new Vector3(2.0f, 0.0f, 0.0f);
    public float recoilTime = 0.1f;

    public float bobTime = 0.5f;
    public Transform[] bobPath;

    public RigidbodyMotor motor;

    private float returnTimer = 0.1f;
    private float returnTime = 0.1f;
    private Vector3 returnFromPos;
    private Quaternion returnFromRot;

    private float bobTimer = 0.0f;
    private int bobNode = 0;

    private bool canBob = true;
    private bool bobbing = false;
    private bool startBob = true;
    private Vector3 lastBobPos;
    private Quaternion lastBobRot;

    private Vector3 neutralPosition = Vector3.zero;

    private delegate float EasingFunc(float t);

    private Gun gun;

	void Start () {
        gun = GetComponent<Gun>();
        gun.OnFire += OnFire;

        motor.OnStartMove += OnStartMove;
        motor.OnStopMove += OnStopMove;
        motor.OnJump += DisableGunBob;
        motor.OnDodge += DisableGunBob;
        motor.OnLand += EnableGunBob;
        motor.OnStopDodge += EnableGunBobIfGrounded;
	}

	void Update () {
        if (returnTimer < returnTime) {
            returnTimer += Time.deltaTime;
            float fraction = returnTimer / returnTime;
            transform.localPosition = Vector3.Lerp(returnFromPos, neutralPosition, fraction);
            transform.localRotation = Quaternion.Slerp(returnFromRot, Quaternion.identity, fraction);
        } else if (canBob && bobbing) {
            if (bobTimer == 0.0f && startBob) {
                lastBobPos = transform.localPosition;
                lastBobRot = transform.localRotation;
            }

            bobTimer += Time.deltaTime;
            if (bobTimer >= bobTime) {
                bobNode = (bobNode+1)%bobPath.Length;
                startBob = false;
                bobTimer = 0.0f;
            }

            Vector3 prevPos;
            Quaternion prevRot;
            if (startBob) {
                prevPos = lastBobPos;
                prevRot = lastBobRot;
            } else {
                prevPos = bobPath[(bobNode-1+bobPath.Length)%bobPath.Length].localPosition;
                prevRot = bobPath[(bobNode-1+bobPath.Length)%bobPath.Length].localRotation;
            }

            EasingFunc easingFunc;
            if (bobNode % 2 == 0) {
                easingFunc = Util.easeOutQuad;
            } else {
                easingFunc = Util.easeInQuad;
            }

            float fraction = bobTimer / bobTime;
            transform.localPosition = Vector3.Lerp(prevPos, bobPath[bobNode].localPosition, easingFunc(fraction));
            transform.localRotation = Quaternion.Slerp(prevRot, bobPath[bobNode].localRotation, easingFunc(fraction));
        }
	}

    void OnFire() {
        doRecoil();
        resetBob();
    }

    private void doRecoil() {
        returnTimer = 0.0f;
        returnTime = recoilTime;
        returnFromPos = Vector3.forward * recoil.x + Vector3.up * recoil.y + Vector3.right * recoil.z;
        returnFromRot = Quaternion.Euler(recoilRot.x, recoilRot.y, recoilRot.z);
    }

    private void resetBob() {
        bobTimer = 0.0f;
        bobNode = 0;
        startBob = true;
    }
    
    public void setBobbing(bool bobbing) {
        if (!this.bobbing && bobbing) {
            resetBob();
        }
        this.bobbing = bobbing;
    }

    private void OnStartMove() {
        setBobbing(true);
    }

    private void OnStopMove() {
        setBobbing(false);
    }

    private void EnableGunBobIfGrounded(bool inAir) {
        if (!inAir) {
            canBob = true;
            resetBob();
            setNeutral(Vector3.zero);
        }
    }

    private void EnableGunBob() {
        canBob = true;
        resetBob();
        setNeutral(Vector3.zero);
    }

    private void DisableGunBob(bool inAir) {
        canBob = false;
        if (!inAir) {
            setNeutral(new Vector3(0.0f, -0.05f, -0.1f));
        }
    }

    private void setNeutral(Vector3 neutral) {
        returnFromPos = transform.localPosition;
        returnFromRot = transform.localRotation;
        neutralPosition = neutral;
        returnTimer = 0.0f;
        returnTime = 0.1f;
    }
}
