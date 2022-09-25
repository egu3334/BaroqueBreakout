using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    

    public void OnTriggerEnter(Collider c) {
    	if (c.attachedRigidbody == null) {
    		return;
    	}

    	Collector bc = c.attachedRigidbody.gameObject.GetComponent<Collector>();
    	if (bc == null) {
    		return;
    	}

    	this.gameObject.SetActive(false);
    	// bc.Collect();
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
