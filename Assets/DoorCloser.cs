using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorCloser : MonoBehaviour
{

	public GameObject leftDoor, rightDoor;

	private Animator leftAnim, rightAnim;
	private bool doorsAlreadyShut = false;

    // Start is called before the first frame update
    void Start()
    {
        this.leftAnim = leftDoor.GetComponent<Animator>();
        this.rightAnim = rightDoor.GetComponent<Animator>();
    }

    // Update is called once per frame
    void OnTriggerEnter(Collider c) {
    	if (c.attachedRigidbody == null) {
    		return;
    	}

    	Collector bc = c.attachedRigidbody.gameObject.GetComponent<Collector>();
    	if (bc == null) {
    		return;
    	}

    	if (!doorsAlreadyShut) {
    		leftAnim.SetBool("slide", false);
        	rightAnim.SetBool("slide", false);
    		doorsAlreadyShut = true;
    	}
    }
}
