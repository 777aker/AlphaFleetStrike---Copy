using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCamera : MonoBehaviour {

    [SerializeField] private Transform player;

    // Update is called once per frame
    void Update() {
        transform.rotation = player.rotation;
    }
}
