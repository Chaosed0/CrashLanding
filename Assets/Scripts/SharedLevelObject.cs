using UnityEngine;
using System.Collections;

public class SharedLevelObject : MonoBehaviour {
    public string nextLevel;
    public bool skipTutorial = false;

    void Start() {
        Object.DontDestroyOnLoad(gameObject);
    }
}
