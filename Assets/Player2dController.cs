using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player2dController : MonoBehaviour
{
    private Vector3 orientation;
    private GameObject enteredPainting;
    private Rigidbody rb;
    private float playerSpeed;
    private float moveXY;
    private float fakeGravity;

    private Vector3 fakeGravityDirection;
    private float jump;

    public void Jump(InputAction.CallbackContext context)
    {
        if (IsGrounded() && context.performed)
        {
            rb.AddForce(new Vector3(0, jump, 0), ForceMode.Impulse);
        }
    }
    public void Move(InputAction.CallbackContext context)
    {
        Debug.Log("I have moved.");
        moveXY = context.ReadValue<float>();
        float move = moveXY * playerSpeed;
        Vector3 moveVec = new Vector3(
            move * orientation.z, 
            rb.velocity.y, 
            -1 * move * orientation.x
            );
        Debug.Log(orientation);
        // Debug.Log(moveVec);
        rb.velocity = moveVec;
        
    }

    public void SetOrientation(Vector3 directionVector)
    {
        orientation = directionVector;
    }

    public void SetPainting(GameObject painting)
    {
        enteredPainting = painting;
        Vector3 paintingAngle = enteredPainting.transform.eulerAngles;
        // transform.eulerAngles = paintingAngle;
        Debug.Log(enteredPainting.transform.eulerAngles);
    }

    void InitPhysics()
    {
        playerSpeed = 4;
        jump = 15;
        rb = GetComponent<Rigidbody>();
        moveXY = 0;
        fakeGravity = -1.5f;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (orientation == null)
        {
            orientation = new Vector3(0, 0, 0);
        }
        InitPhysics();
    }

    private bool IsGrounded()
    {
        int mask = ~LayerMask.GetMask("Painting");
        bool isGrounded = Physics.Raycast(transform.position, Vector3.down, 1, mask);
        Debug.Log(isGrounded);
        return isGrounded;
    }

    private bool IsTherePainting()
    {
        int mask = LayerMask.GetMask("Painting");
        Vector3 xyVelocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        Vector3 origin = transform.position + xyVelocity.normalized * 0.6f;
        Vector3 directionVector = -xyVelocity.normalized * 0.6f;
        bool isthere = Physics.Raycast(origin, directionVector, 0.5f, mask);
        return isthere;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Debug.Log(orientation);
        // if (!IsGrounded())
        {
            rb.velocity += new Vector3(0, fakeGravity, 0);
        }
        if (IsTherePainting())
        {
            moveXY = 0;
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
        }
        else 
        {
            // float move = moveXY * playerSpeed;
            // rb.velocity = new Vector3(
            //     move * orientation.z, 
            //     rb.velocity.y, 
            //     -1 * move * orientation.x
            //     );
        }
    }
}
