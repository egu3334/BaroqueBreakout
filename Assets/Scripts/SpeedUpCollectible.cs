using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUpCollectible : MonoBehaviour
{

/*        void update() {
            GamestateManager GM = FindObjectOfType<GamestateManager>();
            if (GM.is3D && Vector3.Distance(GM.Player3D.transform.position, this.transform.position) <= 10.0f) {
                GM.speedUpCollected();
            	this.gameObject.SetActive(false);
            }

        }
*/

        public void OnTriggerEnter(Collider c) {
        	if (c.attachedRigidbody == null) {
        		return;
        	}

            GamestateManager GM = FindObjectOfType<GamestateManager>();
            GM.speedUpCollected();
            this.gameObject.SetActive(false);
        	// bc.Collect();
        }

}
