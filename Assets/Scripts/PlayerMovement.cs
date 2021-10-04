using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour {
    // position of left and right engine to apply
    // a force at a position
    [SerializeField] private Transform leftEngine;
    [SerializeField] private Transform rightEngine;

    // objects rigidbody
    private Rigidbody2D body;

    // speed of the various engines
    private float mainEngine = 400;
    private float reverseEngine = 200;
    private float smallEngines = 20;
    
    // particle systems for cool effect
    [SerializeField] private ParticleSystem[] mainParticleSystems;
    [SerializeField] private ParticleSystem[] reverseParticleSystems;
    [SerializeField] private ParticleSystem leftParticleSystem;
    [SerializeField] private ParticleSystem rightParticleSystem;

    // engine
    [SerializeField] private AudioSource engineHum;
    
    //fuel
    private double fuel = 1000;
    [SerializeField] private Slider fuelUI;
    public void Start() {
        body = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate() {
        if (fuel <= 0) {
            foreach (ParticleSystem ps in mainParticleSystems) {
                ps.Stop(false, ParticleSystemStopBehavior.StopEmitting);
            }
            foreach (ParticleSystem ps in reverseParticleSystems) {
                ps.Stop(false, ParticleSystemStopBehavior.StopEmitting);
            }
            leftParticleSystem.Stop(false, ParticleSystemStopBehavior.StopEmitting);
            rightParticleSystem.Stop(false,ParticleSystemStopBehavior.StopEmitting);
            if(engineHum.isPlaying)
                engineHum.Stop();
            return;
        }

        if (Input.GetKey(KeyCode.W)) {
            foreach (ParticleSystem ps in mainParticleSystems) {
                if(!ps.isEmitting)
                    ps.Play();
            }
            body.AddRelativeForce(Vector2.up * mainEngine * Time.fixedDeltaTime);
            fuel -= 2 * Time.fixedDeltaTime;
        }
        else {
            foreach (ParticleSystem ps in mainParticleSystems) {
                ps.Stop(false, ParticleSystemStopBehavior.StopEmitting);
            }
        }

        if (Input.GetKey(KeyCode.S)) {
            foreach (ParticleSystem ps in reverseParticleSystems) {
                if(!ps.isEmitting)
                    ps.Play();
            }
            body.AddRelativeForce(Vector2.down * reverseEngine * Time.fixedDeltaTime);
            fuel -= Time.fixedDeltaTime;
        }
        else {
            foreach (ParticleSystem ps in reverseParticleSystems) {
                ps.Stop(false, ParticleSystemStopBehavior.StopEmitting);
            }
        }
        
        if (Input.GetKey(KeyCode.D)) {
            if(!leftParticleSystem.isEmitting)
                leftParticleSystem.Play();
            body.AddForceAtPosition(leftEngine.up * smallEngines * Time.fixedDeltaTime, leftEngine.position);
            fuel -= Time.fixedDeltaTime;
        }
        else {
            leftParticleSystem.Stop(false, ParticleSystemStopBehavior.StopEmitting);
        }
        
        if (Input.GetKey(KeyCode.A)) {
            if(!rightParticleSystem.isEmitting)
                rightParticleSystem.Play();
            body.AddForceAtPosition(rightEngine.up * smallEngines * Time.fixedDeltaTime, rightEngine.position);
            fuel -= Time.fixedDeltaTime;
        }
        else {
            rightParticleSystem.Stop(false, ParticleSystemStopBehavior.StopEmitting);
        }

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.W)
                                    || Input.GetKey(KeyCode.S) 
                                    || Input.GetKey(KeyCode.D)) {
            //Debug.Log("yo");
            if (!engineHum.isPlaying) {
                //Debug.Log("hello");
                engineHum.Play();
            }
        }
        else {
            if (engineHum.isPlaying) {
                //Debug.Log("bye");
                engineHum.Stop();
            }
        }

        fuelUI.value = (float) fuel;
    }

    public void fuellevel(float fule) {
        fuel = fule;
        fuelUI.value = (float) fuel;
    }

    public void giveFuel(float amount) {
        fuel += amount;
        if (fuel > fuelUI.maxValue)
            fuel = fuelUI.maxValue;
        fuelUI.value = (float) fuel;
    }

}
