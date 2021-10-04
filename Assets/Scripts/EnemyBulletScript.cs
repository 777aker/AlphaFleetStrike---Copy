using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletScript : MonoBehaviour {
    private Rigidbody2D body;
    // Start is called before the first frame update
    void Start() {
        body = GetComponent<Rigidbody2D>();
        body.AddRelativeForce(Vector2.up * 150);
        GetComponent<AudioSource>().Play();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player"))
            StartCoroutine(destroy());
    }

    IEnumerator destroy() {
        yield return null;
        Destroy(gameObject);
    }
    
}
