using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderBiteController : MonoBehaviour
{
    
    public float attackInterval;
    public int attackDamage;
    public GameObject playerCharacter;

    private float lastAttack;
    private HealthController playerHealth;

    // Start is called before the first frame update
    void Start()
    {
        this.playerHealth = playerCharacter.GetComponent<HealthController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void OnTriggerEnter(Collider c) {
        if (c == null || c.attachedRigidbody == null
            || c.attachedRigidbody.gameObject == null) {
            Debug.Log("Skipping");
            return;
        }

        GameObject trigger = c.attachedRigidbody.gameObject;
        Debug.Log("Trigger entered!");
    	if (GameObject.ReferenceEquals(trigger, playerCharacter)
    		|| trigger.transform.parent == playerCharacter) {
            if (Time.time - this.lastAttack >= attackInterval) {
            	this.playerHealth.TakeDamage(attackDamage);
            	this.lastAttack = Time.time;
            }
    	}
    }
}
