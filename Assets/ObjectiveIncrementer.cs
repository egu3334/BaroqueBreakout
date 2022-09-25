using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveIncrementer : MonoBehaviour
{

	public int index;
	public GameObject objectiveControllerObject;

	private ObjectiveController objManager;

    // Start is called before the first frame update
    void Start()
    {
        this.objManager = objectiveControllerObject.GetComponent<ObjectiveController>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnTriggerEnter(Collider c) {
    	if (c.attachedRigidbody == null) {
    		return;
    	}

    	Collector bc = c.attachedRigidbody.gameObject.GetComponent<Collector>();
    	if (bc == null) {
    		return;
    	}

      // Debug.Log("Incrementing objective " + index);
    	objManager.IncrementObjectiveIfActive(index);
      FindObjectOfType<AudioManager>().PlaySoundEffect("Objective Complete");
    	this.gameObject.SetActive(false);
    }

}
