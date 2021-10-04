using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlanetResources : MonoBehaviour {
    [SerializeField] private float inorganicMatter;
    [SerializeField] private float organicMatter;
    [SerializeField] private float organicRatio;
    [SerializeField] private float growthRate;
    private float maxOrganic;

    [SerializeField] private Slider organicsSlider;
    [SerializeField] private Slider deadSlider;

    [SerializeField] private GameObject organButton;
    [SerializeField] private GameObject deadButton;

    [SerializeField] private GameObject player;
    
    private void Start() {
        updateMaxOrganic();
        StartCoroutine(growOrganics());
        deadSlider.maxValue = inorganicMatter;
        deadSlider.value = inorganicMatter;
        organButton.SetActive(false);
        deadButton.SetActive(false);
    }

    void updateMaxOrganic() {
        maxOrganic = organicRatio * inorganicMatter;
        organicsSlider.maxValue = maxOrganic;
        if (organicMatter > maxOrganic)
            organicMatter = maxOrganic;
        organicsSlider.value = organicMatter;
    }

    IEnumerator growOrganics() {
        while (true) {
            organicMatter = (float) (Math.Pow(organicMatter, growthRate));
            if (organicMatter > maxOrganic)
                organicMatter = maxOrganic;
            organicsSlider.value = organicMatter;
            
            yield return new WaitForSeconds(5);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) { //changed to game object
        if (other.gameObject == player.gameObject) {
            organButton.SetActive(true);
            deadButton.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject == player.gameObject) {
            organButton.SetActive(false);
            deadButton.SetActive(false);
        }
    }

    public void collectOrganic() {
        var tmp = organicMatter;
        organicMatter -= maxOrganic/10;
        if (organicMatter <= 0)
            organicMatter = 0;
        tmp -= organicMatter;
        player.GetComponent<PlayerResources>().increaseOrganic(tmp);
        organicsSlider.value = organicMatter;
    }

    public void collectInorganic() {
        inorganicMatter -= deadSlider.maxValue / 10;
        player.GetComponent<PlayerResources>().increaseInorganic(deadSlider.maxValue/10);
        deadSlider.value = inorganicMatter;
        updateMaxOrganic();
    }
}
