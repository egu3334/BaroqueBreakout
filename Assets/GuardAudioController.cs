using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardAudioController : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip clip1;
    public AudioClip clip2;
    public AudioClip clip3;
    
    GameObject player;
    Transform currT;
    public float soundLimit;
    // Start is called before the first frame update
    
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        player = GameObject.Find("PlayerObj3d");
        currT = GetComponent<Transform>();
        soundLimit = 20.0f;
    }

    // Update is called once per frame
    void Step()
    {   
        float dist = Vector3.Distance(currT.position, player.transform.position);
        if (dist <= soundLimit && dist > 0) {
            audioSource.volume = ((soundLimit - dist) / soundLimit) * 0.8f;
        } else if (dist > soundLimit) {
            audioSource.volume = 0f;
        } else {
            audioSource.volume = 0.8f;
        }
        audioSource.PlayOneShot(clip1, audioSource.volume);
    }

    void Grunt()
    {
        audioSource.PlayOneShot(clip2, 1);
    }

    void Collapse()
    {
        audioSource.PlayOneShot(clip3, 1);
    }
}
