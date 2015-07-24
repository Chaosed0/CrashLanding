using UnityEngine;
using System.Collections;

public class ActivateAfterTutorial : MonoBehaviour {
    public Tutorial tutorial;
    public GameObject[] objects;

    void Start() {
        tutorial.OnTutorialEnd += OnTutorialEnd;
    }

    void OnTutorialEnd() {
        for (int i = 0; i < objects.Length; i++) {
            objects[i].SetActive(true);
        }
    }
}
