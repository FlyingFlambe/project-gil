using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldScript : MonoBehaviour {

    private LevelManager levelManager;

    public int goldValue;

    void Start()
    {
        levelManager = FindObjectOfType<LevelManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            levelManager.AddGold(goldValue);

            Destroy(gameObject);
        }
    }
}
