using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    public GameObject[] collectables;
    public bool tr = false;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        tr = true;
        if (other.CompareTag("collectable"))
            {
                other.gameObject.SetActive(false);
            }
    }
    
}
