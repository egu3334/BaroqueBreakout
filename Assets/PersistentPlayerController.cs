using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PersistentPlayerController : MonoBehaviour
{
    public GameObject player3d;
    public GameObject player2d;
    public GameObject camObj;
    public GameObject flashLight;

    private GameObject enteredPainting;
    private CameraManagerController cam;
    private Player2dController control2d;
    private Vector3 offset;
    private int swapTimer;

    public GameObject killHud;

    enum PlayerState
    {
        THREE_D, TWO_D
    }
    private PlayerState pState;

    public bool playerIs3D()
    {
        return pState == PlayerState.THREE_D;
    }

    public bool playerIs2D()
    {
        return pState == PlayerState.TWO_D;
    }

    private void SetTo2D(int angle)
    {
        if (player3d.activeInHierarchy && FindObjectOfType<GamestateManager>().getCanShiftTo2D())
        {
            offset = AngleToVector(angle);
            Debug.Log(angle);
            player3d.SetActive(false);
            player2d.SetActive(true);
            player2d.transform.eulerAngles = new Vector3(
                player2d.transform.eulerAngles.x,
                angle,
                player2d.transform.eulerAngles.z
            );
            // Set2DCamera(angle);
            control2d.SetOrientation(offset);
            control2d.SetPainting(enteredPainting);
            pState = PlayerState.TWO_D;


            FindObjectOfType<GamestateManager>().JustShifted(-1);
            //cam.SwapTo2D(angle);

        }
    }

    private void SetTo3D()
    {
        if (player2d.activeInHierarchy)
        {
            player2d.SetActive(false);
            player3d.SetActive(true);
            player3d.transform.position -= offset;
            // cam.SetTarget(player3d);
            // cam.SetTopDown();
            pState = PlayerState.THREE_D;
            FindObjectOfType<GamestateManager>().JustShifted(1);
            //cam.SwapTo3D();
        }
    }

    private Vector3 AngleToVector(int angle)
    {
        Vector3 directionVector = new Vector3(0, 0, 0);
        switch (angle)
        {
            case 0:
                directionVector = new Vector3(0, 0, 1);
                break;
            case 90:
                directionVector = new Vector3(1, 0, 0);
                break;
            case 180:
                directionVector = new Vector3(0, 0, -1);
                break;
            case 270:
                directionVector = new Vector3(-1, 0, 0);
                break;
            default:
                directionVector = new Vector3(-1, 0, 0);
                break;
        }
        return directionVector;
    }

    private bool IsTherePainting(int angle)
    {
        int mask = LayerMask.GetMask("Painting");
        Vector3 directionVector = AngleToVector(angle);
        RaycastHit hit;
        bool isthere = Physics.Raycast(player3d.transform.position, directionVector, out hit, 2f, mask);
        if (isthere)
        {
            // Debug.Log(hit.transform.position);
            enteredPainting = hit.transform.gameObject;
            if (directionVector.x != 0)
            {
                player2d.transform.position = new Vector3(
                    hit.transform.position.x,
                    player2d.transform.position.y,
                    player2d.transform.position.z
                );
            }
            if (directionVector.z != 0)
            {
                player2d.transform.position = new Vector3(
                    player2d.transform.position.x,
                    player2d.transform.position.y,
                    hit.transform.position.z
                );
            }
        }

        // Debug.Log("Is there a painting? " + isthere);
        return isthere;
    }

    public void Swap()
    {
        if (playerIs3D())
        {
            player2d.transform.position = player3d.transform.position;
            if (IsTherePainting(0))
            {
                // Debug.Log("0");
                SetTo2D(0);
            }
            else if (IsTherePainting(90))
            {
                // Debug.Log("90");
                SetTo2D(90);
            }
            else if (IsTherePainting(180))
            {
                // Debug.Log("180");
                SetTo2D(180);
            }
            else if (IsTherePainting(270))
            {
                // Debug.Log("270");
                SetTo2D(270);
            }
            else
            {
                return;
            }
        }
        else if (playerIs2D())
        {
            player3d.transform.position = player2d.transform.position;
            SetTo3D();
        }
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if (swapTimer == 0 && context.performed)
        {
            Swap();
            swapTimer = 30;
        }
    }

    public void OnKill(InputAction.CallbackContext context)
    {
        if (!playerIs3D()) {
            return;
        }

        if (player3d.GetComponent<FreeLookPlayerController>().targetGuard != null)
        {
            player3d.GetComponent<FreeLookPlayerController>().targetGuard.GetComponent<GuardController>().alive = false;
        }
    }

    public void OnLight(InputAction.CallbackContext context)
    {
        if (!playerIs3D()) {
            return;
        }

        // GameObject flashLight = player3d.transform.GetChild(1).GetChild(2).gameObject;
        if (player3d.GetComponent<FreeLookPlayerController>().hasLight)
        // {
            if (flashLight.activeSelf == false)
            {
                flashLight.SetActive(true);
            }
            else {
                flashLight.SetActive(false);
            }
        // }
        // if (player3d.GetComponent<Player3dController>().targetGuard != null)
        // {
        //     player3d.GetComponent<Player3dController>().targetGuard.GetComponent<GuardController>().alive = false;
        // }
    }

    // Start is called before the first frame update
    void Start()
    {
        offset = new Vector3(0, 0, 0);
        swapTimer = 0;
        player2d = GameObject.Find("Player2d");
        player3d = GameObject.Find("PlayerObj3d");
        /*camObj = GameObject.Find("CameraManager");
        if (camObj)
        {
            cam = camObj.GetComponent<CameraManagerController>();
        }*/
        if (player2d)
        {
            control2d = player2d.GetComponent<Player2dController>();
        }
        SetTo3D();
    }

    // Update is called once per frame
    void Update()
    {
        if (swapTimer > 0)
        {
            swapTimer -= 1;
        }

        if (killHud != null) {
            killHud.SetActive(playerIs3D() && player3d.GetComponent<FreeLookPlayerController>().targetGuard != null);
        }
    }
}