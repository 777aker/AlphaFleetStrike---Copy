using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Looker : MonoBehaviour {

    [SerializeField] private String tag;
    [SerializeField] private GameObject parent;
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag(tag)) {
            if (tag == "Enemy")
                parent.GetComponent<DroneBehavior>().setTarget(other.transform);
            if(tag == "Player")
                parent.GetComponent<EnemyLogic>().setTarget(other.transform);
        }
    }
}
