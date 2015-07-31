using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour {
    public Transform barrelPoint;
    public int damage = 100;
    public float bulletSpread = 5.0f;
    public Vector3 recoil = new Vector3(-0.5f, 0.0f, 0.0f);
    public Vector3 recoilRot = new Vector3(2.0f, 0.0f, 0.0f);

    public float recoilTime = 0.1f;
    public float bobTime = 0.5f;
    public float cooldown = 0.1f;
    public float sphereCastSize = 0.25f;

    public Bullet bulletPrefab = null;
    public Marker markerPrefab = null;

    public Transform[] bobPath;

    private float recoilTimer = 0.1f;
    private float bobTimer = 0.0f;
    private float cooldownTimer = 0.0f;
    private int bobNode = 0;
    private bool firing = false;

    private bool bobbing = false;
    private bool startBob = true;
    private Vector3 lastBobPos;
    private Quaternion lastBobRot;

    public delegate void FireDelegate();
    public event FireDelegate OnFire;

    private delegate float EasingFunc(float t);

    void Update() {
        cooldownTimer += Time.deltaTime;

        if (recoilTimer < recoilTime) {
            recoilTimer += Time.deltaTime;
            float fraction = (recoilTime - recoilTimer) / recoilTime;
            transform.localPosition = Vector3.forward * recoil.x * fraction + Vector3.up * recoil.y * fraction + Vector3.right * recoil.z * fraction;
            transform.localRotation = Quaternion.Slerp(Quaternion.identity, Quaternion.Euler(recoilRot.x, recoilRot.y, recoilRot.z), fraction);
        } else if (!firing && bobbing) {
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
        
        if (firing && cooldownTimer >= cooldown) {
            Fire();
            resetBob();
        }
    }

    private void resetBob() {
        bobTimer = 0.0f;
        bobNode = 0;
        startBob = true;
    }

    public void setFiring(bool firing) {
        this.firing = firing;
    }
    
    public void setBobbing(bool bobbing) {
        if (!this.bobbing && bobbing) {
            resetBob();
        } else if (!bobbing) {
        }
        this.bobbing = bobbing;
    }

    public void Fire() {
        /* Fire event */
        if (OnFire != null) {
            OnFire();
        }

        /* Spawn the bullet */
        Quaternion bulletRot = randomBulletRot(Camera.main.transform);
        Instantiate(bulletPrefab, barrelPoint.position, bulletRot);
        cooldownTimer = 0.0f;

        /* Recoil */
        recoilTimer = 0.0f;

        /* Check for a hit */
        RaycastHit hitInfo;
        /* Ignore the player */
        int layerMask = ~((1 << 8) | (1 << 11));
        bool hit = Physics.SphereCast(Camera.main.transform.position, sphereCastSize, bulletRot * Vector3.forward, out hitInfo, 500.0f, layerMask);
        if (!hit) {
            return;
        }

        if (markerPrefab != null) {
            Instantiate(markerPrefab, hitInfo.point, Quaternion.identity);
        }

        GameObject hitObject = hitInfo.transform.gameObject;
        Character enemy = hitObject.GetComponent<Character>();
        if (enemy == null) {
            return;
        }

        enemy.damage(damage);
    }

    private Quaternion randomBulletRot(Transform origin) {
        Vector3 fwd = origin.forward;
        Vector3 right = origin.right;
        Vector3 up = origin.up;

        float upRot = Mathf.Deg2Rad * Random.Range(-bulletSpread, bulletSpread);
        float rightRot = Mathf.Deg2Rad * Random.Range(-bulletSpread, bulletSpread);

        return Quaternion.LookRotation(Vector3.RotateTowards(Vector3.RotateTowards(fwd, up, upRot, 0.0f), right, rightRot, 0.0f));
    }
}
