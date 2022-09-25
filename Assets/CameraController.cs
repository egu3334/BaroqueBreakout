using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    GameObject target;
    enum Mode
    {
        TopDown, Dolly0, Dolly90, Dolly180, Dolly270
    };
    Mode cameraMode;
    Vector3 followPosition;
    Vector3 lookAtPosition;
    private Vector3 zeroVel = Vector3.zero;

    public void SetTarget(GameObject obj)
    {
        target = obj;
    }

    void CalculateTargets()
    {
        if (target && target.activeInHierarchy)
        {
            Vector3 positionOffset = new Vector3(0, 0, 0);
            Vector3 targetPosition = target.transform.position;
            if (cameraMode == Mode.TopDown)
            {
                positionOffset = new Vector3(0, 5, -5);
            }
            else if (cameraMode == Mode.Dolly0)
            {
                positionOffset = new Vector3(0, 2, -10);
            }
            else if (cameraMode == Mode.Dolly90)
            {
                positionOffset = new Vector3(-10, 2, 0);
            }
            else if (cameraMode == Mode.Dolly180)
            {
                positionOffset = new Vector3(0, 2, 10);
            }
            else if (cameraMode == Mode.Dolly270)
            {
                positionOffset = new Vector3(10, 2, 0);
            }
            lookAtPosition = targetPosition + new Vector3(0, 0, 0);
            followPosition = targetPosition + positionOffset;
        }
    }

    public void SetTopDown()
    {
        cameraMode = Mode.TopDown;
    }

    public void SetDolly(int direction)
    {
        switch (direction)
        {
            case 0: 
                cameraMode = Mode.Dolly0;
                break;
            case 90:
                cameraMode = Mode.Dolly90;
                break;
            case 180:
                cameraMode = Mode.Dolly180;
                break;
            case 270:
                cameraMode = Mode.Dolly270;
                break;
            default:
                cameraMode = Mode.Dolly0;
                break;
        }
    }

    void LerpTargets()
    {
        transform.position = Vector3.SmoothDamp(transform.position, followPosition, ref zeroVel, 0.25f);
        // Vector3 directionToTarget = (lookAtPosition - transform.position).normalized;
        // Debug.Log(directionToTarget);
        transform.LookAt(lookAtPosition);
    }

    // Start is called before the first frame update
    void Start()
    {
        SetTopDown();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        CalculateTargets();
        LerpTargets();
    }
}
