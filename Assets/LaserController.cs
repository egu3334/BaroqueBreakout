using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserController : MonoBehaviour
{
    
	public GameObject klaxon;
	private KlaxonController kc;

	// Guard interaction var
	private GameObject[] guards;

    void Start()
    {
        guards = GameObject.FindGameObjectsWithTag("Guard");
    }

    // Update is called once per frame
    void Update()
    {
        this.kc = klaxon.GetComponent<KlaxonController>();
    }


    void OnTriggerEnter(Collider c) {
    	if (c.attachedRigidbody == null) {
    		return;
    	}

    	TripwireBreaker breaker = c.attachedRigidbody.gameObject.GetComponent<TripwireBreaker>();
    	if (breaker == null) {
    		return;
    	}

    	// Set off alarm noise
    	if (kc != null && c.gameObject.tag != "Light") {
    		kc.TriggerKlaxon();
    	}

		// Alert the guards
		if (c.gameObject.tag == "Player") {
			// find the closest guard to assign for investigation
			GameObject closestGuard = guards[0];
			foreach (GameObject guard in guards) {
				float curDist = Vector3.Distance(c.gameObject.transform.position, closestGuard.transform.position);
				float tempDist = Vector3.Distance(c.gameObject.transform.position, guard.transform.position);
				if (curDist > tempDist) {
					closestGuard = guard;
				}
			}
			// send to closest guard to investigate
			GuardController guardController = closestGuard.GetComponent<GuardController>();
			guardController.isAlarmOn = true;
			guardController.invSpot = c.gameObject.transform.position;
			guardController.state =  GuardController.State.INVESTIGATE;
		}
    }

}
