using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player3dController : MonoBehaviour
{

    public float playerSpeed = 2;
    public GameObject playerModel;
    public GameObject targetGuard;
    public GameObject[] guardList;
    public GameObject flashLight;

    public float fakeGravity = 0.5f;
    public float range = 3.0f;
    public bool hasLight = false;

    private Animator anim;
    private Rigidbody rb;

    private Vector3 forwardVector = new Vector3(1, 0, 0);
    private float inputX;
    private float inputY;

    public void SetForwardVector(Vector3 direction)
    {
        forwardVector = direction;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        // Debug.Log("moving");

        Vector2 moveVector = context.ReadValue<Vector2>();

        inputX = moveVector.x;
        inputY = moveVector.y;

    }

    void MovePerFrame()
    {
        Vector3 rightDir = -1 * Vector3.Cross(forwardVector, Vector3.up).normalized;
        Vector3 forwardDir = forwardVector.normalized;

        Vector3 leftRightMove = rightDir * inputX * playerSpeed;
        Vector3 forwardBackMove = forwardDir * inputY * playerSpeed;

        Vector3 yMovement = new Vector3(0, rb.velocity.y, 0);

        Vector3 newVel = leftRightMove + forwardBackMove + yMovement;
        rb.velocity = Vector3.Lerp(rb.velocity, newVel, 0.5f);
    }

    void ApplyAngle()
    {
        Vector3 movementDirection = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        Quaternion look = Quaternion.LookRotation(movementDirection, Vector3.up);
        playerModel.transform.rotation = Quaternion.Lerp(playerModel.transform.rotation, look, 0.1f);
    }

    void AnimateMove()
    {
        anim.SetFloat("vely", rb.velocity.magnitude * 2, 1f, 0.1f);
    }

    void ApplyFakeGravity()
    {
        if (!Physics.Raycast(transform.position, Vector3.down, 1))
        {
            rb.velocity -= new Vector3(0, fakeGravity, 0);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        anim = playerModel.GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        guardList = GameObject.FindGameObjectsWithTag("Guard");
        flashLight = transform.GetChild(1).GetChild(2).gameObject;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MovePerFrame();
        AnimateMove();
        if (rb.velocity.magnitude >= playerSpeed * 0.9)
        {
            ApplyAngle();
        }
        ApplyFakeGravity();

        Aim();
    }


    void Aim()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, playerModel.transform.forward, out hit, Mathf.Infinity))
        {
            if (hit.collider.gameObject.layer == 13) {
                Debug.DrawRay(transform.position, playerModel.transform.forward * hit.distance, Color.yellow);

                targetGuard = hit.collider.gameObject;
            }
        }
    }
}
