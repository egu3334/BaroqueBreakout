using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioToggler : MonoBehaviour
{
    // Start is called before the first frame update

    void Start()
    {

    }

    public void ToggleAudio() {
        FindObjectOfType<AudioManager>().ToggleMusic();
    }
}
