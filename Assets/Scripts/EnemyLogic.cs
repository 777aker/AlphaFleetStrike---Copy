using System;
using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

public class EnemyLogic : MonoBehaviour {
    [SerializeField] private Transform cruiser;
    [SerializeField] private float maxCruiserDistance;
    [SerializeField] private float maxEnemyDistance;
    [SerializeField] private float minEnemyDistace;
    [SerializeField] private GameObject bullets;
    [SerializeField] private ParticleSystem destroyed;
    [SerializeField] private float maxShips;
    [SerializeField] private GameObject lilguys;

    [SerializeField] private float health;
    [SerializeField] private CircleCollider2D looker;

    private Transform target;

    [SerializeField] private AudioSource ouchy;
    [SerializeField] private AudioSource oof;

    private void Start() {
        StartCoroutine(followCruiser());
        StartCoroutine(spawnShips());
    }

    IEnumerator followCruiser() {
        while (true) {
            if (target != null) {
                StartCoroutine(shoot());
                yield return StartCoroutine(chaseEnemy());
            }
            if (cruiser != null && Vector2.Distance(transform.position, cruiser.position) > maxCruiserDistance) {
                Quaternion myRot = transform.rotation;
                Quaternion rotCruiser =
                    Quaternion.LookRotation(Vector3.forward, cruiser.position - transform.position);
                Quaternion newRotation = Quaternion.Slerp(myRot, rotCruiser, Time.deltaTime);
                transform.rotation = newRotation;
                transform.position += transform.up * Time.deltaTime*2;
                yield return null;
            }
            else if(cruiser != null) {
                transform.position += transform.up * Time.deltaTime*2;
                yield return null;
            }
            else {
                transform.position += transform.up * Time.deltaTime / 2;
                yield return null;
            }
        }
    }

    IEnumerator chaseEnemy() {
        while (target != null) {
            if (Vector2.Distance(transform.position, target.position) <= minEnemyDistace) {
                Quaternion myRot = transform.rotation;
                Quaternion rotTowardEnemy =
                    Quaternion.LookRotation(Vector3.forward, target.position - transform.position);
                Quaternion newRotation = Quaternion.Slerp(myRot, rotTowardEnemy, Time.deltaTime * 5);
                transform.rotation = newRotation;
                transform.position += transform.right * Time.deltaTime / 2;
                yield return null;
            } else if (Vector2.Distance(transform.position, target.position) <= maxEnemyDistance) {
                Quaternion myRot = transform.rotation;
                Quaternion rotTowardEnemy =
                    Quaternion.LookRotation(Vector3.forward, target.position - transform.position);
                Quaternion newRotation = Quaternion.Slerp(myRot, rotTowardEnemy, Time.deltaTime * 2);
                transform.rotation = newRotation;
                transform.position += transform.up * Time.deltaTime*3;
                yield return null;
            }
            else {
                target = null;
            }
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
        if (other.CompareTag("FriendlyBullet")) {
            health -= 1;
            if(!ouchy.isPlaying)
                ouchy.Play();
        }

        if (health <= 0) {
            ParticleSystem ps = Instantiate(destroyed, transform.position, transform.rotation);
            ps.Play();
            //if(!oof.isPlaying)
             //   oof.Play();
            if(ouchy.isPlaying)
                ouchy.Stop();
            Destroy(gameObject);
        }
    }

    public void setTarget(Transform other) {
        if (target != null)
            return;
        target = other;
    }
    
    IEnumerator spawnShips() {
        float ships = 0;
        while (ships < maxShips) {
            Instantiate(lilguys, transform.position, transform.rotation);
            ships++;
            yield return new WaitForSeconds(2);
        }
    }
    
}
