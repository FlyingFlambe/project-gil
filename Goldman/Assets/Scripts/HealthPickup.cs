using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour {

    LevelManager levelManager;

    public int healthValue;
    
	void Start () {
        levelManager = FindObjectOfType<LevelManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            levelManager.AddHealth(healthValue);

            Destroy(gameObject);
        }
    }
}
