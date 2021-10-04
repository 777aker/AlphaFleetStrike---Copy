using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerResources : MonoBehaviour {
    private float inorganic = 0;
    private float organic = 0;

    [SerializeField] private Text organicBox;
    [SerializeField] private Text inorganicBox;

    [SerializeField] private GameObject drone;
    public void increaseOrganic(float amount) {
        organic += amount;
        organicBox.text = "Organic Matter = " + organic;
    }

    public void decreaseOrganic(float amount) {
        if (amount > organic)
            return;
        organic -= amount;
        gameObject.GetComponent<PlayerMovement>().giveFuel(10);
        organicBox.text = "Organic Matter = " + organic;
    }

    public void increaseInorganic(float amount) {
        inorganic += amount;
        inorganicBox.text = "Organic Matter = " + inorganic;
    }

    public void decreaseInorganic(float amount) {
        if(amount > inorganic)
            return;
        inorganic -= amount;
        if(amount == 10)
            gameObject.GetComponent<PlayerHealth>().increaseHealth(1);
        if(amount == 50)
            makeDrone();
        inorganicBox.text = "Organic Matter = " + inorganic;
    }

    public void makeDrone() {
        GameObject droned = Instantiate(drone, transform.position, transform.rotation);
        droned.GetComponent<DroneBehavior>().player = gameObject.transform;
    }

}
