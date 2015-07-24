using UnityEngine;
using System.Collections;

[RequireComponent (typeof(AudioSource))]
public class AmpAfterTutorial : MonoBehaviour {
    public Tutorial tutorial;
    public float fadeInTime = 2.0f;
    public float target = 0.8f;

    private AudioSource audioSource;
    private float fadeInTimer = 0.0f;
    private float originalVolume; 
    private bool fadeIn = false;

	void Start() {
        audioSource = GetComponent<AudioSource>();
        tutorial.OnTutorialEnd += OnTutorialEnd;
        originalVolume = audioSource.volume;
	}

    void Update() {
        if (!fadeIn) {
            return;
        }

        fadeInTimer += Time.deltaTime;
        audioSource.volume = originalVolume + (target - originalVolume) * (1.0f - (fadeInTime - fadeInTimer) / fadeInTime);

        if (fadeInTimer > fadeInTime) {
            fadeIn = false;
        }
    }

    void OnTutorialEnd() {
        fadeIn = true;
        fadeInTimer = 0.0f;
        originalVolume = audioSource.volume;
    }
}
