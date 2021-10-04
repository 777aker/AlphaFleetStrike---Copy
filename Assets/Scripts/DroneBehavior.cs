using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneBehavior : MonoBehaviour {
    [SerializeField] public Transform player;
    [SerializeField] private float maxCommandShipDistance;
    [SerializeField] private float maxEnemyDistance;
    [SerializeField] private GameObject bullets;
    [SerializeField] private ParticleSystem destroyed;

    [SerializeField] private CircleCollider2D looker;

    [SerializeField] private AudioSource hurt;
    
    private float health = 50;

    private Transform target;
    private void Start() {
        StartCoroutine(followPlayer());
    }

    IEnumerator followPlayer() {
        while (true) {
            if (target != null) {
                StartCoroutine(shoot());
                yield return StartCoroutine(chaseEnemy());
            }
            if (Vector2.Distance(transform.position, player.position) > maxCommandShipDistance) {
                Quaternion myRot = transform.rotation;
                Quaternion rotTowardPlayer = Quaternion.LookRotation(Vector3.forward, player.position - transform.position);
                Quaternion myNewRotation = Quaternion.Slerp(myRot, rotTowardPlayer, Time.deltaTime * 2);
                transform.rotation = myNewRotation;
                transform.position += transform.up * Time.deltaTime;
                yield return null;
            }
            else {
                transform.position += transform.up * Time.deltaTime;
                yield return null;
            }
        }
    }

    IEnumerator chaseEnemy() {
        while (target != null && Vector2.Distance(transform.position, target.position) <= maxEnemyDistance) {
            Quaternion myRot = transform.rotation;

            Quaternion rotTowardEnemy = Quaternion.LookRotation(Vector3.forward, target.position - transform.position);

            Quaternion newRotation = Quaternion.Slerp(myRot, rotTowardEnemy, Time.deltaTime * 2);
        
            transform.rotation = newRotation;
            transform.position += transform.up * Time.deltaTime;

            yield return null;
        }

        looker.enabled = false;
        looker.enabled = true;
    }

    IEnumerator shoot() {
        while (target != null) {
            Instantiate(bullets, transform.position + transform.right/8, transform.rotation);
            Instantiate(bullets, transform.position - transform.right/8, transform.rotation);
            yield return new WaitForSeconds(1);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("EnemyBullet")) {
            health -= 1;
            if(!hurt.isPlaying)
                hurt.Play();
        }

        if (health <= 0) {
            ParticleSystem ps = Instantiate(destroyed, transform.position, transform.rotation);
            ps.Play();
            if(hurt.isPlaying)
                hurt.Stop();
            Destroy(gameObject);
        }
    }

    public void setTarget(Transform other) {
        if (target != null)
            return;
        target = other;
    }
}
