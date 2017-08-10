using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour {
    public Transform barrelPoint;
    public int damage = 100;
    public float bulletSpread = 5.0f;
    public float cooldown = 0.1f;
    public float sphereCastSize = 0.25f;

    public Bullet bulletPrefab = null;
    public Marker markerPrefab = null;

    private bool firing = false;
    private float cooldownTimer = 0.0f;

    public delegate void FireDelegate();
    public event FireDelegate OnFire;

    void LateUpdate() {
        cooldownTimer += Time.deltaTime;
        
        if (firing && cooldownTimer >= cooldown) {
            Fire();
        }
    }

    public void setFiring(bool firing) {
        this.firing = firing;
    }

    public void Fire() {
        /* Fire event */
        if (OnFire != null) {
            OnFire();
        }

        /* Spawn the bullet */
        Quaternion bulletRot = randomBulletRot(Camera.main.transform);
        Bullet bullet = Instantiate<Bullet>(bulletPrefab, barrelPoint.position, bulletRot);

        cooldownTimer = 0.0f;

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
        float angle = Random.Range(-bulletSpread, bulletSpread);
        float axisAngle = Random.Range(0, Mathf.PI*2);
        Vector3 axis = new Vector3(Mathf.Cos(axisAngle), Mathf.Sin(axisAngle), 0);

        return origin.transform.rotation * Quaternion.AngleAxis(angle, axis);
    }
}
