using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LoadingBar : MonoBehaviour {
    private SharedLevelObject shared;
    private AsyncOperation loadAsync;
    private Slider slider;

	void Start () {
        slider = GetComponent<Slider>();
        shared = GameObject.Find("SharedLevelObject").GetComponent<SharedLevelObject>();
        loadAsync = Application.LoadLevelAsync(shared.nextLevel);
        slider.value = 0.0f;
	}

	void Update () {
        slider.value = loadAsync.progress;
        Debug.Log(loadAsync.progress);
        if (loadAsync.progress >= 1) {
            Application.LoadLevel(shared.nextLevel);
        }
	}
}
