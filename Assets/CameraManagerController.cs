using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraManagerController : MonoBehaviour
{

    enum CameraState
    {
        OverShoulder, Dolly0, Dolly90, Dolly180, Dolly270
    }


    // The object or camera being controlled by this script.
    //   For debug purposes, you can pass in a dummy camera.
    public GameObject cameraObject;
    public GameObject dummyObject;
    public bool controlDummy = false;
    private Player3dController receiver;

    public float fov3d = 60.0f;
    public float fov2d = 60.0f;

    private GameObject targetObj;
    private GameObject playerControl;

    private CameraState state;

    // Stores the current rotation of the controllable object
    //   on the X-Z (horizontal) plane
    private float xzRot;

    public float ySensitivity = 1.0f;
    public float xSensitivity = 0.8f;

    // These values are sent by the event handler 
    //   whenever the mouse moves. They represent 
    //   the distance travelled by the mouse in the 
    //   current frame. Multiply these by Time.deltaTime 
    //   in order to get the correct framerate-independent values!
    private float FPSdeltaX;
    private float FPSdeltaY;


    Vector3 followPosition;
    Vector3 lookAtPosition;
    private Vector3 zeroVel = Vector3.zero;

    // These event handlers connect to the Player Input component
    //   and trigger only when the mouse's velocity has changed.
    //   This includes when the mouse slows to a stop.
    public void OnRotateX(InputAction.CallbackContext context)
    {
        FPSdeltaX = context.ReadValue<float>();
    }
    public void OnRotateY(InputAction.CallbackContext context)
    {
        FPSdeltaY = context.ReadValue<float>();
    }

    void SetTarget(string name)
    {
        targetObj = GameObject.Find(name);
    }

    public void SwapTo2D(int angle)
    {
        /* switch(angle)
        {
            case 0:
                state = CameraState.Dolly0;
                break;
            case 90:
                state = CameraState.Dolly90;
                break;
            case 180:
                state = CameraState.Dolly180;
                break;
            case 270:
                state = CameraState.Dolly270;
                break;
            default:
                break;
        } */

        SetTarget("Player2d");

        cameraObject.GetComponent<Camera>().fieldOfView = fov2d;
    }

    public void SwapTo3D()
    {
        SetTarget("Player Model");
        playerControl = GameObject.Find("PlayerObj3d");
        if (playerControl)
        {
            receiver = playerControl.GetComponent<Player3dController>();
        }
        state = CameraState.OverShoulder;

        cameraObject.GetComponent<Camera>().fieldOfView = fov3d;
    }

    void Start()
    {
        FPSdeltaX = 0.0f;
        FPSdeltaY = 0.0f;
        xzRot = 0.0f;
        SwapTo3D();
    }

    void ApplyRotation()
    {
        float deltaX = FPSdeltaX * Time.deltaTime;
        float deltaY = FPSdeltaY * Time.deltaTime;

        GameObject controllable = controlDummy ? dummyObject : cameraObject;

        if (targetObj)
        {
            Vector3 forwardVector = cameraObject.transform.forward;
            Vector3 ninetyDegreeOff =  -1 * Vector3.Cross(forwardVector, Vector3.up).normalized;
            Vector3 shoulderOffset = ninetyDegreeOff * 0.3f;
            Vector3 basePos = targetObj.transform.position + shoulderOffset;
            basePos.y = targetObj.transform.position.y + 1.55f;
    
            float viewDistance = 1.5f;

            if (controllable)
            {
                xzRot -= deltaX * xSensitivity * 0.2f;
                float xOffset = Mathf.Cos(xzRot);
                float yOffset = Mathf.Sin(xzRot);
                Vector3 offsetVector = new Vector3(xOffset, 0, yOffset);
                Vector3 offset = offsetVector.normalized * viewDistance;
                // Debug.Log("xzRot: " + xzRot + ", offset: " + offset);

                Vector3 cameraPos = basePos + offset;
                Vector3 lookPos = basePos;

                Vector3 lookVector = lookPos - cameraPos;
                Quaternion lookQuat = Quaternion.LookRotation(lookVector);

                Quaternion appliedRotation = controllable.transform.rotation;
                Vector3 appliedPosition = controllable.transform.position;

                controllable.transform.rotation = Quaternion.Lerp(appliedRotation, lookQuat, 0.5f);
                controllable.transform.position = Vector3.Lerp(appliedPosition, cameraPos, 0.5f);
            }
        }
        
    }

    void SendYawValue()
    {
        if (receiver)
        {
            Vector3 yaw = cameraObject.transform.forward;
            receiver.SetForwardVector(yaw);
        }
    }

    void CalculateDollyOffsets()
    {
        if (targetObj && targetObj.activeInHierarchy)
        {
            Vector3 positionOffset = new Vector3(0, 0, 0);
            Vector3 targetPosition = targetObj.transform.position;
            if (state == CameraState.Dolly0)
            {
                positionOffset = new Vector3(0, 1.5f, -5);
            }
            else if (state == CameraState.Dolly90)
            {
                positionOffset = new Vector3(-5, 1.5f, 0);
            }
            else if (state == CameraState.Dolly180)
            {
                positionOffset = new Vector3(0, 1.5f, 5);
            }
            else if (state == CameraState.Dolly270)
            {
                positionOffset = new Vector3(5, 1.5f, 0);
            }
            lookAtPosition = targetPosition + new Vector3(0, 0, 0);
            followPosition = targetPosition + positionOffset;
        }
    }

    void ApplyDollyCam()
    {
        cameraObject.transform.position = Vector3.SmoothDamp(cameraObject.transform.position, followPosition, ref zeroVel, 0.05f);
        // Vector3 directionToTarget = (lookAtPosition - transform.position).normalized;
        // Debug.Log(directionToTarget);
        cameraObject.transform.LookAt(lookAtPosition);
    }

    void FixedUpdate()
    {
        if (state == CameraState.OverShoulder)
        {          
            ApplyRotation();
            SendYawValue();
        }
        else 
        {
            CalculateDollyOffsets();
            ApplyDollyCam();
        }
    }
}
