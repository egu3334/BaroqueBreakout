using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HealthController : MonoBehaviour
{

    public int baseHealth;
    public float regenDelay;
    public bool immortal = false;

    public Text healthText;
    public GameObject gameOverHUD;

    [HideInInspector]
    public int currentHealth;
    private float damageTime;
    private float lastRegen;

    private GameOverHUDController gameOverController;


    // Start is called before the first frame update
    void Start() {
        this.SetHealth(baseHealth);
        this.gameOverController = gameOverHUD.GetComponent<GameOverHUDController>();
    }

    // Update is called once per frame
    void Update() {
        if (damageTime == -1) {
            Regen();
        	return;
        }

        if (Time.time - damageTime >= regenDelay) {
        	this.damageTime = -1;
        }
    }

    void Regen() {
        if (Time.time - lastRegen >= 1 && currentHealth != baseHealth) {
            this.SetHealth(Mathf.Min(baseHealth, this.currentHealth + 5));
            lastRegen = Time.time;
        }
    }

    void SetHealth(int newHealth) {
    	this.currentHealth = newHealth;
        this.healthText.text = currentHealth + " / " + baseHealth + " HP";
    	this.damageTime = -1;
    }

    public void TakeDamage(int damage) {
    	if (!this.immortal) {
            this.SetHealth(Mathf.Max(0, this.currentHealth - damage));
            this.damageTime =  Time.time;

            if (this.currentHealth == 0) {
                GameOver();
            }
        }
    }

    public void GameOver() {
        this.gameOverController.SetNextLevelActionAndScene("Try Again",
            SceneManager.GetActiveScene().name, false);
        FindObjectOfType<AudioManager>().PlaySoundEffect("Game Over");
        this.gameOverController.SetNextLevelActionAndScene("Try Again",
            SceneManager.GetActiveScene().name);
        this.gameOverController.PresentWithText("Game over! :(");
    }


}
