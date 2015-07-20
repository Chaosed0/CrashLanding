using UnityEngine;
using System.Collections;

public class SharedLevelObject : MonoBehaviour {
    public string nextLevel;

    void Start() {
        Object.DontDestroyOnLoad(gameObject);
    }
}
