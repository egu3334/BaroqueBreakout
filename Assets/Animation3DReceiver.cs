using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animation3DReceiver : MonoBehaviour
{

	public GameObject player;
	private FreeLookPlayerController flpc;

	void Start() {
		this.flpc = player.GetComponent<FreeLookPlayerController>();
	}
    
    void Step() {
    	if (this.flpc.IsSprinting()) {
    		FindObjectOfType<AudioManager>().PlaySoundEffect("Footstep Loud");
    	} else {
    		FindObjectOfType<AudioManager>().PlaySoundEffect("Footstep Soft");
    	}
    }
}
