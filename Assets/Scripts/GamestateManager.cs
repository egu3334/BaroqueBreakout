using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class GamestateManager : MonoBehaviour
{

    public GameObject Player3D;
    public GameObject[] Guards;

    public Text NotificationText;
    public Text SecurityText;
    public Text TimeText;
    public Text HealthUpdateText;
    public Text ShiftCooldownText;

    public AudioManager AudioManager;

    public float guardNearbyDist;
    public float guardAlertedDist;

    int currentHP;
    float lastNotificationTime;
    float lastGuardWarningTime;
    float lastHealthUpdateTime;
    float securityIncreasedTime;
    int money = 0;
    GameObject[] inventory;

    public bool timerOn;
    public float baseTimeLimit;

    [HideInInspector]
    public float timeLeft;

    [HideInInspector]
    public bool is3D;

    private bool justShifted;

    private float twoDTimeStamp;
    public float shiftCooldown;
    private float shiftCooldownRemaining;
    private bool canShift2D;
    private bool startCompleted;

    public enum securityLevel {
        LOW,
        HIGH
    }

    securityLevel security;

    void Awake() {
        startCompleted = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        setNotification("Welcome to Baroque Breakout!");
        currentHP = Player3D.GetComponent<HealthController>().baseHealth;
        lastHealthUpdateTime = -1f;
        lastGuardWarningTime = - 10f;
        securityIncreasedTime = - 15f;
        security = securityLevel.LOW;
        setSecurityLevelText();
        AudioManager.Play("Level Theme");
        timeLeft = baseTimeLimit;

        is3D = true;
        justShifted = false;
        twoDTimeStamp = -5f;
        shiftCooldownRemaining = 0f;
        canShift2D = true;
        // startCompleted = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Player3D.GetComponent<Collector>().win) {
            AudioManager.StopAll();
        }

        if (lastNotificationTime != -1f && Time.time - lastNotificationTime >= 3f) {
            NotificationText.text = "";
            lastNotificationTime = -1f;
        }

        if (lastHealthUpdateTime != -1f && Time.time - lastHealthUpdateTime >= 3f) {
            HealthUpdateText.text = "";
            lastHealthUpdateTime = -1f;
        }

        if (justShifted) {
            justShifted = false;
        }

        if (is3D) {
            threeDHandler();
        } else if (!is3D) {
            twoDHandler();
        }

        if (Player3D.GetComponent<HealthController>().currentHealth != currentHP) {
            string healthChange = Player3D.GetComponent<HealthController>().currentHealth < currentHP ?
                "- " + (currentHP - Player3D.GetComponent<HealthController>().currentHealth) + " health":
                "+ " + (Player3D.GetComponent<HealthController>().currentHealth - currentHP) + " health";
            setHealthUpdateText(healthChange);
            currentHP = Player3D.GetComponent<HealthController>().currentHealth;
        }
        guardInRangeCheck();
        TimerUpdate();
    }

    void setNotification(string notification) {
        NotificationText.text = notification;
        lastNotificationTime = Time.time;
    }

    void setSecurityLevelText() {
        if (security == securityLevel.LOW) {
            SecurityText.text = "Security Level: Low";
        } else {
            SecurityText.text = "Security Level: High";
        }
    }

    void setHealthUpdateText(string healthUpdate) {
        HealthUpdateText.text = healthUpdate;
        if(healthUpdate[0] == '-') {
            HealthUpdateText.color = Color.red;
        } else {
            HealthUpdateText.color = Color.green;
        }
        lastHealthUpdateTime = Time.time;
    }

    void TimerUpdate() {
        if (timerOn) {
            timeLeft -= Time.deltaTime;
            float minLeft = Mathf.Floor((timeLeft + 1.0f) / 60);
            float secLeft = Mathf.Round(Mathf.Ceil(timeLeft) % 60);
            TimeText.text = (secLeft >= 0.0f && secLeft <= 9.01f) ?
            "" + minLeft + ":0" + secLeft:
            "" + minLeft + ":" + secLeft;
            if (timeLeft <= 0.0f) {
                Player3D.GetComponent<HealthController>().GameOver();
            }
        }
    }

    public void JustShifted(int param) {
        justShifted = true;
        ShiftHandler(param);
    }

    void ShiftHandler(int param) {
        if (param == -1) {
            twoDTimeStamp = Time.time - 1.0f;
            AudioManager.PlaySoundEffect("Shift In");
            is3D = false;
        } else if (param == 1){
            if (startCompleted) {
                shiftCooldownRemaining = shiftCooldown;
                canShift2D = false;
                AudioManager.PlaySoundEffect("Shift Out");
            } else {
                startCompleted = true;
            }
            // shiftCooldownRemaining = shiftCooldown;
            // canShift2D = false;
            is3D = true;
        }
    }

    public bool getCanShiftTo2D() {
        return canShift2D;
    }

    void threeDHandler() {
        if (!canShift2D) {
            shiftCooldownRemaining -= Time.deltaTime;
            if (shiftCooldownRemaining <= 0.0f) {
                ShiftCooldownText.text = "Shift Available";
                canShift2D = true;
            } else {
                ShiftCooldownText.text = "Shift Cooldown: " + shiftCooldownRemaining.ToString("F2");
            }
        }
    }

    void twoDHandler() {
        if (Time.time - twoDTimeStamp >= 1.0f) {
            twoDTimeStamp = Time.time;
            Player3D.GetComponent<HealthController>().TakeDamage(3);
        }
    }

    public void speedUpCollected() {
        Player3D.GetComponent<FreeLookPlayerController>().maxSpeed *= 1.25f;
        setNotification("Refresing soda found. Max speed up!");
    }

    void guardInRangeCheck() {
        if (Player3D.activeSelf) {
            Vector3 playerPosition = Player3D.transform.position;
            if (security == securityLevel.LOW) {
                foreach (GameObject guard in Guards) {
                    if (Vector3.Distance(playerPosition, guard.transform.position) <= guardAlertedDist) {
                        security = securityLevel.HIGH;
                        setNotification("Too close to a guard! All guards move faster!");
                        securityIncreasedTime = Time.time;
                        setSecurityLevelText();
                        AudioManager.Stop("Level Theme");
                        AudioManager.Play("Tense Theme");
                        break;
                    }
                }
            }

            if (security == securityLevel.LOW
                    && Time.time >= lastGuardWarningTime + 10) {
                foreach (GameObject guard in Guards) {
                    if (Vector3.Distance(playerPosition, guard.transform.position) <= guardNearbyDist) {
                        setNotification("Caution, guard nearby");
                        lastGuardWarningTime = Time.time;
                    }
                }
            }
        }
        if (security == securityLevel.HIGH) {
            if (securityIncreasedTime == Time.time) {
                foreach (GameObject guard in Guards) {
                    if (guard.GetComponent<NavMeshAgent>() != null) {
                        guard.GetComponent<NavMeshAgent>().speed *= 2;
                    }
                }
            } else {
                if (Time.time >= securityIncreasedTime + 15) {
                    security = securityLevel.LOW;
                    setNotification("Security level lowered");
                    setSecurityLevelText();
                    AudioManager.Stop("Tense Theme");
                    AudioManager.Play("Level Theme");
                    foreach (GameObject guard in Guards) {
                        if (guard.GetComponent<NavMeshAgent>() != null) {
                            guard.GetComponent<NavMeshAgent>().speed /= 2;
                        }
                    }
                }
            }
        }
    }
}
