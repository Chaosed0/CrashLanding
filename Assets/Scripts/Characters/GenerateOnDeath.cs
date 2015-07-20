using UnityEngine;
using System.Collections;

[RequireComponent(typeof (Character))]
public class GenerateOnDeath : MonoBehaviour {
    public Transform[] prefabs;
    public bool removeThisOnDeath = true;

	void Start () {
        GetComponent<Character>().OnDied += generatePrefab;
	}
    
    public void generatePrefab() {
        if (prefabs != null && prefabs.Length > 0) {
            Instantiate(prefabs[(int)Random.Range(0.0f, prefabs.Length-1.0f)], transform.position, Quaternion.identity);
        }
        if (removeThisOnDeath) {
            Destroy(gameObject);
        }
    }
}
