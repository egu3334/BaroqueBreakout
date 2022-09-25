using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KlaxonController : MonoBehaviour
{
    
    public GameObject audioManagerObj;

    private AudioManager audioManager;
    public float volume = 0.25f;

    // Start is called before the first frame update
    void Start()
    {
        this.audioManager = audioManagerObj.GetComponent<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TriggerKlaxon() {
    	// klaxon.wav https://freesound.org/people/Lalks/sounds/316841/
    	// klaxon2.wav https://freesound.org/people/kwahmah_02/sounds/245025/
        audioManager.Stop("Level Theme");
    	audioManager.PlaySoundEffect("klaxon");
        audioManager.Play("Tense Theme");
    }

    public void StopKlaxon() {
        audioManager.Play("Level Theme");
    	audioManager.Stop("klaxon");
        audioManager.Stop("Tense Theme");
    }

}
