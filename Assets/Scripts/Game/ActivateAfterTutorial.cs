using UnityEngine;
using System.Collections;

public class ActivateAfterTutorial : MonoBehaviour {
    public Tutorial tutorial;
    public GameObject[] objects;
    public bool early;

    void Start() {
        if (early) {
            tutorial.OnEarlyTutorialEnd += OnTutorialEnd;
        } else {
            tutorial.OnTutorialEnd += OnTutorialEnd;
        }
    }

    void OnTutorialEnd() {
        for (int i = 0; i < objects.Length; i++) {
            objects[i].SetActive(true);
        }
    }
}
