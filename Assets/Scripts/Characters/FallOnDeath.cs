﻿using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Rigidbody))]
[RequireComponent (typeof (Character))]
public class FallOnDeath : MonoBehaviour {
	void Start () {
        GetComponent<Character>().OnDied += OnDied;
	}

    void OnDied() {
        Rigidbody body = GetComponent<Rigidbody>();
        body.freezeRotation = false;
    }
}
