﻿using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Character))]
[RequireComponent (typeof (Expires))]
public class AnimateOnDeath : MonoBehaviour {
    public Animator anim;

    void Start() {
        GetComponent<Character>().OnDied += OnDied;
        GetComponent<Expires>().enabled = false;
    }

    private void OnDied() {
        anim.SetBool("Died", true);
        GetComponent<Expires>().enabled = true;
    }
}
