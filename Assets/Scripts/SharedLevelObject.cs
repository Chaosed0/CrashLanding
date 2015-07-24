using UnityEngine;
using System.Collections;

public class SharedLevelObject : MonoBehaviour {
    public string nextLevel;
    public bool skipTutorial = false;
    public bool invertMouse = false;

    void Start() {
        Object.DontDestroyOnLoad(gameObject);
    }
}
