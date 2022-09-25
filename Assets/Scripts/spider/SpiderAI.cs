using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Debug = UnityEngine.Debug;
public class SpiderAI : MonoBehaviour
{
    public Animator anim;
    public GameObject temp;

    public bool _3dactive;

    public Transform player;
    public GameObject[] destination;
    private GameObject targetwaypoint;

    private int currWaypoint = 0;
    private float minDistance = 1.5f;
    private int lastIndex;

    private NavMeshAgent agent;
    public float movementspeed = 1.0f;

    public bool dead = false;
    public float num;
    void Start()
    {
        anim = GetComponent<Animator>();
        lastIndex = destination.Length - 1;
        targetwaypoint = destination[currWaypoint];
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        _3dactive = temp.activeSelf;
        if (Vector3.Distance(player.position, this.transform.position) < 15.0 && _3dactive) {
            chase();
        } else {
            roam();
        }


    }

    private void chase()
    {

            Vector3 direction = player.position - this.transform.position;
            direction.y = 0;

            this.transform.rotation = Quaternion.Slerp(this.transform.rotation,
                Quaternion.LookRotation(direction), 0.1f);

            anim.SetBool("isIdle", false);
            if (direction.magnitude > 5)
            {
                Vector3 walk = new Vector3(player.position.x + 1.5f, player.position.y, player.position.z + 1.5f);
                agent.SetDestination(walk);
                anim.SetBool("isWalking", true);
                anim.SetBool("isAttacking", false);
            }
            else
            {
                anim.SetBool("isAttacking", true);
                anim.SetBool("isWalking", false);
            }
    }


    private void roam()
    {
        anim.SetBool("isWalking", true);
        if (destination.Length != 0)
        {
            float movementstep = movementspeed * Time.deltaTime;
            float distance = Vector3.Distance(transform.position, targetwaypoint.transform.position);
            checkwaypoint(distance);
            agent.SetDestination(destination[currWaypoint].transform.position);
        }
        else { return; }
    }

    private void checkwaypoint(float currentDistance)
    {

        if (currentDistance <= minDistance)
        {
            currWaypoint++;
            setNextWaypoint();
        }
    }
    private void setNextWaypoint()
    {
        if (currWaypoint > lastIndex)
        {
            currWaypoint = 0;
        }
        targetwaypoint = destination[currWaypoint];
    }
}
