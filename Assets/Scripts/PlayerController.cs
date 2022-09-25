using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    CameraController cam;
    Rigidbody rb;
    private float playerSpeed;
    private Vector2 moveVector;
    private float faceDir;
    private float fakeGravity;

    public GameObject target;

    public void Move(InputAction.CallbackContext context)
    {
        moveVector = context.ReadValue<Vector2>();
        rb.velocity = new Vector3(
            moveVector.x * playerSpeed,
            rb.velocity.y,
            moveVector.y * playerSpeed);
    }

    void InitModel()
    {
        faceDir = 90;
    }

    void InitPhysics()
    {
        playerSpeed = 6;
        rb = GetComponent<Rigidbody>();
        moveVector = new Vector2(0.0f, 0.0f);
        fakeGravity = -0.5f;
    }

    void InitCamera()
    {
        GameObject camObj = GameObject.FindWithTag("MainCamera");
        if (camObj)
        {
            cam = camObj.GetComponent<CameraController>();
            if (cam)
            {
                cam.SetTarget(this.gameObject);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        InitCamera();
        InitPhysics();
        InitModel();
    }

    void DrawModel()
    {
        GameObject playerModel = GameObject.FindWithTag("PlayerModel");
        if (playerModel)
        {
            if (moveVector.x > 0) 
            {
                if (moveVector.y > 0)
                {
                    faceDir = 45;
                }
                else if (moveVector.y < 0)
                {
                    faceDir = 135;
                }
                else
                {
                    faceDir = 90;
                }
            }
            else if (moveVector.x < 0) 
            {
                if (moveVector.y > 0)
                {
                    faceDir = 315;
                }
                else if (moveVector.y < 0)
                {
                    faceDir = 225;
                }
                else
                {
                    faceDir = 270;
                }
            }
            else 
            {
                if (moveVector.y > 0)
                {
                    faceDir = 0;
                }
                else if (moveVector.y < 0)
                {
                    faceDir = 180;
                }
            }
            float currDir = playerModel.transform.localEulerAngles.y;
            float lerpDir = Mathf.LerpAngle(currDir, faceDir, 0.333f);
            playerModel.transform.localEulerAngles = new Vector3(0, lerpDir, 0);
        }
    }
    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 1);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        DrawModel();
        if (!IsGrounded())
        {
            rb.velocity += new Vector3(0, fakeGravity, 0);
        }

        GameObject playerModel = GameObject.FindWithTag("PlayerModel");
        RaycastHit hit;
        int rayCount = 20;
        float aInc = 2.0f / rayCount;
        for (int i = 0; i < rayCount; i++) {
            Debug.DrawRay(transform.position, (transform.forward + transform.right * (1.0f - aInc * i)).normalized * 5.0f, Color.green);
            if (Physics.Raycast (transform.position, (transform.forward + transform.right * (1.0f - aInc * i)).normalized, out hit, 5.0f))
            {
                if (hit.collider.gameObject.tag == "Guard")
                {
                    target = hit.collider.gameObject;
                } else {
                    target = null;
                }
            }
        }
    }
}
