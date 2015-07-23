using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour {
    public Transform recoilReturn;
    public Transform barrelPoint;
    public int damage = 100;
    public float bulletSpread = 5.0f;
    public Vector3 recoil = new Vector3(-0.1f, 0.0f, 0.0f);
    public Vector3 recoilRot = new Vector3(1.0f, 0.0f, 0.0f);
    public float recoilTime = 0.1f;
    public float cooldown = 0.1f;
    public float sphereCastSize = 0.25f;
    public Bullet bulletPrefab = null;
    public Marker markerPrefab = null;

    public AudioSource gunAudio;

    private float recoilTimer = 0.1f;
    private float cooldownTimer = 0.0f;
    private bool firing = false;

    void Update() {
        cooldownTimer += Time.deltaTime;

        if (recoilTimer < recoilTime) {
            recoilTimer += Time.deltaTime;
            float fraction = (recoilTime - recoilTimer) / recoilTime;
            transform.position = recoilReturn.position + recoilReturn.forward * recoil.x * fraction + recoilReturn.up * recoil.y * fraction + recoilReturn.right * recoil.z * fraction;
            transform.rotation = Quaternion.Slerp(recoilReturn.rotation, recoilReturn.rotation * Quaternion.Euler(recoilRot.x, recoilRot.y, recoilRot.z), fraction);
        }
        
        if (firing && cooldownTimer >= cooldown) {
            Fire();
            gunAudio.Play();
        }
    }

    public void setFiring(bool firing) {
        this.firing = firing;
    }

    public void Fire() {
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
