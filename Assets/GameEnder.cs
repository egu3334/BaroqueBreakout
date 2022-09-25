using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEnder : MonoBehaviour
{

	public GameObject gameOverHud;
	private GameOverHUDController hudController;

    // Start is called before the first frame update
    void Start()
    {
        this.hudController = gameOverHud.GetComponent<GameOverHUDController>();
    }

    public void OnTriggerEnter(Collider c) {
    	if (c.attachedRigidbody == null) {
    		return;
    	}

    	Collector bc = c.attachedRigidbody.gameObject.GetComponent<Collector>();
    	if (bc == null) {
    		return;
    	}

		FindObjectOfType<AudioManager>().StopAll();
    	this.hudController.PresentWithText("You won! :)");
    }
}
