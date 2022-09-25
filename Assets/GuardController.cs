using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.AI;
using Debug = UnityEngine.Debug;

public class GuardController : MonoBehaviour
{
    // global var
    public enum State
    {
        PATROL,
        PURSUE,
        INVESTIGATE,
        LOST,
        DEAD
    };
    UnityEngine.AI.NavMeshAgent agent;
    Animator anim;
    public State state;
    public bool alive;
    public bool isAlarmOn = false;
    public float popUptime = 0;
    private bool _3dactive;
    public GameObject player;
    public GameObject light;

    // patrol var
    public GameObject[] waypoints;
    public int currWaypoint = 0;
    private float patrolSpeed = 0.7f;

    // pursue var
    public GameObject target;
    private float chaseSpeed = 1f;

    // investigate var
    public Vector3 invSpot;
    public float timer = 0;
    private float curTime = 0;
    private float waitTime = 10;

    // sight var
    private float heightMult;
    private float sightDist = 13;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        player = GameObject.Find("PlayerObj3d");
        light = transform.GetChild(3).GetChild(0).gameObject;
        state = State.PATROL;
        alive = true;
        heightMult = 1.0f;
        // StartCoroutine("FSM");
    }

    void Update ()
    {
        if (alive)
        {
            switch (state)
            {
                case State.PATROL:
                    Patrol();
                    break;
                case State.PURSUE:
                    Pursue();
                    break;
                case State.INVESTIGATE:
                    Investigate();
                    break;
                case State.LOST:
                    Lost();
                    break;
                default:
                    break;
            }
        } else {
            popUptime += Time.deltaTime;

            state = State.DEAD;
            anim.SetFloat("vely", 0);
            anim.SetBool("alive", false);
            agent.speed = 0;
            agent.SetDestination(agent.transform.position);
            
            light.SetActive (false);

            
            if (popUptime >= 1)
            {
                GameObject popup = GameObject.Find("Popup");
                popup.GetComponent<PopupController>().OpenPopup();
                if (player.GetComponent<FreeLookPlayerController>().hasLight == false)
                {
                    player.GetComponent<FreeLookPlayerController>().hasLight = true;
                }
                transform.GetChild(6).gameObject.SetActive(false);
            }
        }
        timer += Time.deltaTime;
    }

    void Patrol()
    {
        Search();
        agent.speed = patrolSpeed;
        if (Vector3.Distance(this.transform.position, waypoints[currWaypoint].transform.position) >= 1.5)
        {
            agent.SetDestination(waypoints[currWaypoint].transform.position);
            anim.SetFloat("vely", 0.5f);
            anim.speed = patrolSpeed;
        }
        else if (Vector3.Distance(this.transform.position, waypoints[currWaypoint].transform.position) < 1.5)
        {
            currWaypoint += 1;
            if (currWaypoint >= waypoints.Length || currWaypoint < 0)
            {
                currWaypoint = 0;
            }
        }
        else
        {
            anim.SetFloat("vely", 0);
        }
    }

    void Pursue()
    {
        _3dactive = player.activeSelf;
        if (_3dactive)
        {
            agent.speed = chaseSpeed;
            agent.SetDestination(target.transform.position);
            anim.SetFloat("vely", 1);
            anim.speed = chaseSpeed;
        }
        else{
            target = null;
            state = State.LOST;
        }
        
    }

    void Investigate()
    {   
        Search();
        agent.SetDestination(invSpot);
        anim.SetFloat("vely", 1);
        anim.speed = 0.8f;
        // transform.LookAt(invSpot);

        if (Vector3.Distance(this.transform.position, invSpot) < 1.5)
        {
            state = State.LOST;
        }
    }

    void Lost()
    {
        if (curTime == 0) curTime = timer;

        Search();
        agent.SetDestination(this.transform.position);
        anim.SetFloat("vely", 0);
        anim.speed = 3.0f;
        
        if (timer - curTime >= waitTime)
        {
            state = State.PATROL;
            curTime = 0;

            if (isAlarmOn) {
                KlaxonController kc = GameObject.Find("Klaxon").GetComponent<KlaxonController>();
                kc.StopKlaxon();
                isAlarmOn = false;
            }
        }
    }

    void Search()
    {
        RaycastHit hit;
        int rayCount = 40;
        float aInc = 2.0f / rayCount;
        for (int i = 0; i < rayCount; i++) {
            Debug.DrawRay(transform.position + Vector3.up * heightMult, (transform.forward + transform.right * (1.0f - aInc * i)).normalized * sightDist, Color.green);
            if (Physics.Raycast (transform.position + Vector3.up * heightMult, (transform.forward + transform.right * (1.0f - aInc * i)).normalized, out hit, sightDist))
            {
                if (hit.collider.gameObject.tag == "Player")
                {
                    state = State.PURSUE;
                    target = hit.collider.gameObject;
                }
            }

            // Debug.DrawRay(transform.position + Vector3.up * 0.1f, (transform.forward + transform.right * (1.0f - aInc * i)).normalized * sightDist * 3, Color.yellow);
            // if (Physics.Raycast (transform.position + Vector3.up * heightMult, (transform.forward + transform.right * (1.0f - aInc * i)).normalized, out hit, sightDist * 3, 14))
            // {
            //     if (hit.collider.gameObject.tag == "Light")
            //     {
            //         state = State.PURSUE;
            //         target = hit.collider.gameObject;
            //     }
            // }
        }
    }
}
