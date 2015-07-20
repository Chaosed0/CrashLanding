using UnityEngine;
using System.Collections;

public class Interval : MonoBehaviour {
    public float intervalTime = 1.0f;
    public float randRange = 0.0f;
    public bool active = true;

    private float nextInterval = 0.0f;
    private float intervalTimer = 0.0f;

    public delegate void IntervalHit();
    public event IntervalHit OnIntervalHit;

    void Start () {
        nextInterval = intervalTime + Random.Range(-randRange, randRange);
    }

	void Update () {
        if (!active) {
            return;
        }

        intervalTimer += Time.deltaTime;
        if (intervalTimer >= nextInterval) {
            nextInterval = intervalTime + Random.Range(-randRange, randRange);
            intervalTimer = 0.0f;
            OnIntervalHit();
        }
	}
}
