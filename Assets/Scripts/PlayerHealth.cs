using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    
    private float health = 100;
    [SerializeField] private float damage = -5;
    [SerializeField] private Slider healthBar;

    [SerializeField] private ParticleSystem[] damagedPS;
    [SerializeField] private ParticleSystem destroyPS;

    [SerializeField] private AudioSource damageSound;
    [SerializeField] private AudioSource destroyedSound;

    private Boolean destroyed = false;
    private void Start() {
        healthBar.maxValue = health;
        healthBar.value = health;
    }

    public void increaseHealth(float tmphealth) {
        health += tmphealth;
        if (health > healthBar.maxValue)
            healthBar.maxValue = health;
        healthBar.value = health;
        if(health > 50)
            foreach (ParticleSystem ps in damagedPS) {
                if(ps.isPlaying)
                    ps.Stop(false, ParticleSystemStopBehavior.StopEmitting);
            }
    }

    public void changeHealth(float tmp) {
        health -= tmp;
        healthBar.value = health;
        if (destroyed)
            return;
        if(!damageSound.isPlaying)
            damageSound.Play();
        if(health < 50)
            foreach (ParticleSystem ps in damagedPS) {
                if(!ps.isPlaying)
                    ps.Play();
            }

        if (health <= 0) {
            destroyed = true;
            foreach (ParticleSystem ps in damagedPS) {
                if(ps.isPlaying)
                    ps.Stop(false, ParticleSystemStopBehavior.StopEmitting);
            }
            if(!destroyPS.isPlaying)
                destroyPS.Play();
            destroyedSound.Play();
            SpriteRenderer player = gameObject.GetComponent<SpriteRenderer>();
            player.enabled = false;
            gameObject.GetComponent<PlayerMovement>().fuellevel(-5);
            gameObject.GetComponent<PlayerHealth>().enabled = false;
        }
        
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("EnemyBullet"))
            changeHealth(damage);
    }
}
