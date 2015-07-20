using UnityEngine;
using System.Collections;

[RequireComponent(typeof (Interval))]
public class EnemySpawner : MonoBehaviour {
    public Character enemyPrefab;

    private Interval interval;

	void Start () {
        interval = GetComponent<Interval>();
        interval.OnIntervalHit += SpawnEnemy;
	}

    void SpawnEnemy() {
        Instantiate(enemyPrefab, transform.position, transform.rotation);
    }

    void OnDrawGizmos() {
        Gizmos.DrawIcon(transform.position, "macpan.png", true);
    }
}
