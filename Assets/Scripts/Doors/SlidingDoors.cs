using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingDoors : MonoBehaviour
{
    public GameObject trigger;
    public GameObject leftDoor;
    public GameObject rightDoor;
    public int num;
    public int enter;
    public int exit;

    public GameObject objectiveControllerObj;
    private ObjectiveController objManager;

    public int objectiveIndex;


    Animator leftAnim;
    Animator rightAnim;
    // Start is called before the first frame update
    void Start()
    {
        num = 1;
        Debug.Log("It started");
        leftAnim = leftDoor.GetComponent<Animator>();
        rightAnim = rightDoor.GetComponent<Animator>();
        objManager = objectiveControllerObj.GetComponent<ObjectiveController>();
    }

    void OnTriggerEnter(Collider c)
    {
        //collectables[0].activeSelf;
        //bool active = false;

        if (c.attachedRigidbody == null) {
            return;
        }

        Collector bc = c.attachedRigidbody.gameObject.GetComponent<Collector>();
        if (bc == null) {
            return;
        }

        if (objManager.CurrentObjectiveIndex() == objectiveIndex)
        {

            enter = 1;
            SliderDoors(true);
            Debug.Log(true);

            // BallCollector bc = c.attachedRigidbody.gameObject.GetComponent<Trigger>();


        }
    }

    void OnTriggerExit(Collider c)
    {
        if (c.attachedRigidbody != null)
        {

            exit = 1;
            SliderDoors(false);
            Debug.Log(false);
            // BallCollector bc = c.attachedRigidbody.gameObject.GetComponent<Trigger>();
        }
    }
    // bool ObjectivesCollected(GameObject[] collectables) {
    //     foreach (GameObject theobject in collectables)
    //     {
    //         if (theobject.activeSelf) {
    //             return false;
    //         }
    //     }
    //     return true;
    // }
    void SliderDoors(bool state)
    {
        leftAnim.SetBool("slide", state);
        rightAnim.SetBool("slide", state);
    }
}
