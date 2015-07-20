using UnityEngine;
using System.Collections;

public class Util {
    public static GameObject getPlayer() {
        return GameObject.FindGameObjectsWithTag("Player")[0];
    }

    public static float angleTo(Vector3 v1, Vector3 v2) {
        return Mathf.Atan2(
                Vector3.Dot(Vector3.Cross(v1, v2), Vector3.up),
                Vector3.Dot(v1, v2)) * Mathf.Rad2Deg;
    }
}
