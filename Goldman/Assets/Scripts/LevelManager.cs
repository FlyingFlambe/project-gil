using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour {

    public PlayerController player;
    public SpriteRenderer playerSprite;

    public float waitToRespawn;

    public int goldCount;
    public int maxHealth;
    public int healthCount;

    public bool invincible;

    private bool respawning;

    // UI Elements
    public Text goldText;

    public Image playerHealth;

    public Sprite hp30;
    public Sprite hp29;
    public Sprite hp28;
    public Sprite hp27;
    public Sprite hp26;
    public Sprite hp25;
    public Sprite hp24;
    public Sprite hp23;
    public Sprite hp22;
    public Sprite hp21;
    public Sprite hp20;
    public Sprite hp19;
    public Sprite hp18;
    public Sprite hp17;
    public Sprite hp16;
    public Sprite hp15;
    public Sprite hp14;
    public Sprite hp13;
    public Sprite hp12;
    public Sprite hp11;
    public Sprite hp10;
    public Sprite hp9;
    public Sprite hp8;
    public Sprite hp7;
    public Sprite hp6;
    public Sprite hp5;
    public Sprite hp4;
    public Sprite hp3;
    public Sprite hp2;
    public Sprite hp1;
    public Sprite hpsw1;
    public Sprite hpsw0;
    public Sprite hp0;

    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        playerSprite = FindObjectOfType<SpriteRenderer>();

        //goldText.text = "GOLD: " + goldCount;                                                 // ****Add back in once gold is shown in the scene****

        healthCount = maxHealth;
    }

    void Update()
    {
        HealthManager();
    }

    // Health & HP BAR
    public void HealthManager()
    {
        if (healthCount <= 0 && !respawning)
        {
            Respawn();
            respawning = true;
        }

        if (healthCount > maxHealth)
        {
            healthCount = maxHealth;
            UpdateHeartMeter();
        }
    }

    // Respawn player
    public void Respawn()
    {
        StartCoroutine("RespawnCo");
    }

    public IEnumerator RespawnCo()
    {
        player.gameObject.SetActive(false);

        //play death animation here

        yield return new WaitForSeconds(waitToRespawn);

        healthCount = maxHealth;
        respawning = false;
        UpdateHeartMeter();

        player.transform.position = player.respawnPosition;
        player.gameObject.SetActive(true);
    }

    // Gold Collection
    public void AddGold(int goldToAdd)
    {
        goldCount += goldToAdd;
        goldText.text = "Gold: " + goldCount;
    }

    // Health Pickups
    public void AddHealth(int healthToAdd)
    {
        healthCount += healthToAdd;
        UpdateHeartMeter();
    }

    // Player Health
    public void HurtPlayer(int damageTaken)
    {
        if (!invincible)
        {
            healthCount -= damageTaken;
            UpdateHeartMeter();

            player.Knockback();
        }
    }

    public void UpdateHeartMeter()
    {
        switch (healthCount)
        {
            case 30:
                playerHealth.sprite = hp30;
                return;

            case 29:
                playerHealth.sprite = hp29;
                return;

            case 28:
                playerHealth.sprite = hp28;
                return;

            case 27:
                playerHealth.sprite = hp27;
                return;

            case 26:
                playerHealth.sprite = hp26;
                return;

            case 25:
                playerHealth.sprite = hp25;
                return;

            case 24:
                playerHealth.sprite = hp24;
                return;

            case 23:
                playerHealth.sprite = hp23;
                return;

            case 22:
                playerHealth.sprite = hp22;
                return;

            case 21:
                playerHealth.sprite = hp21;
                return;

            case 20:
                playerHealth.sprite = hp20;
                return;

            case 19:
                playerHealth.sprite = hp19;
                return;

            case 18:
                playerHealth.sprite = hp18;
                return;

            case 17:
                playerHealth.sprite = hp17;
                return;

            case 16:
                playerHealth.sprite = hp16;
                return;

            case 15:
                playerHealth.sprite = hp15;
                return;

            case 14:
                playerHealth.sprite = hp14;
                return;

            case 13:
                playerHealth.sprite = hp13;
                return;

            case 12:
                playerHealth.sprite = hp12;
                return;

            case 11:
                playerHealth.sprite = hp11;
                return;

            case 10:
                playerHealth.sprite = hp10;
                return;

            case 9:
                playerHealth.sprite = hp9;
                return;

            case 8:
                playerHealth.sprite = hp8;
                return;

            case 7:
                playerHealth.sprite = hp7;
                return;

            case 6:
                playerHealth.sprite = hp6;
                return;

            case 5:
                playerHealth.sprite = hp5;
                return;

            case 4:
                playerHealth.sprite = hp4;
                return;

            case 3:
                playerHealth.sprite = hp3;
                return;

            case 2:
                playerHealth.sprite = hp2;
                return;

            case 1:
                playerHealth.sprite = hp1;
                return;

            default:
                playerHealth.sprite = hp0;
                return;
        }
    }
}
