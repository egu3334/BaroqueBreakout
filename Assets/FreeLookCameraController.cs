using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FreeLookCameraController : MonoBehaviour
{

    public GameObject follow;
    public GameObject follow2d;
    
    // Start is called before the first frame update
    public float maxFollowDistance = 5;
    public float minFollowDistance = 2;

    float pitch;
    float yaw;
    float targetYaw;

    public float maxPitch;
    public float minPitch;

    public float dotsPerRadian = 1;
    public float sensX;
    public float sensY;

    public float initPitch;
    public float initYaw;

    private Vector3 vZero = Vector3.zero;
    private float fZero = 0.0f;

    public GameObject persistentController;
    private PersistentPlayerController ppc;
    private Vector3 storedDelta = Vector3.zero;

    public GameObject pauseMenu;
    private PauseController pauser;

    public InputAction mouseInput;

    public void Start() {
        this.pitch = this.initPitch;
        this.yaw = this.initYaw;
        if (this.pauseMenu != null) {
            this.pauser = pauseMenu.GetComponent<PauseController>();
        }

        if (this.persistentController != null) {
            this.ppc = this.persistentController.GetComponent<PersistentPlayerController>();
        }
    }


    public void OnEnable() {
    	mouseInput.Enable();
    }

    public void OnDisable() {
    	mouseInput.Disable();
    }


    public float GetPitch() {
    	return this.pitch;
    }


    public float GetYaw() {
    	return this.yaw;
    }


    private bool IsPlayer2D() {
        return this.ppc != null && this.ppc.playerIs2D();
    }


    // Update is called once per frame
    public void Update() {
        if (this.pauser != null && this.pauser.IsPaused()) {
        	return;
        }

        if (IsPlayer2D()) {
            if (this.storedDelta == Vector3.zero) {
                Vector3 baseDelta = follow.transform.position - follow2d.transform.position;
                baseDelta.Normalize();
                float scale = maxFollowDistance + 1;

                this.storedDelta = new Vector3(scale * baseDelta.x, 0.5f, scale * baseDelta.y);
            }
        } else {
            this.storedDelta = Vector3.zero;
        }

    	Vector2 lookDirection = mouseInput.ReadValue<Vector2>();
        float dYaw = lookDirection.x * sensX / dotsPerRadian * Time.deltaTime;
        float dPitch = lookDirection.y * sensY / dotsPerRadian * Time.deltaTime;
        
        float newPitch = this.pitch;
        if (dYaw != 0.0f || dPitch != 0.0f) {
            targetYaw = (this.yaw - dYaw);
            newPitch = this.pitch + dPitch;    
        }
       
        this.yaw = Mathf.SmoothDamp(this.yaw, targetYaw, ref fZero, 0.05f);
	    this.pitch = Mathf.Clamp(newPitch, minPitch, maxPitch); // Mathf.SmoothDamp(this.pitch, newPitch, ref fZero, 0.25f);
        // Debug.Log("pitch = " + this.pitch + ", this.yaw = " + this.yaw);


        // phi = pi / 2 - pitch
        float phi = 1.57079632679f - this.pitch;
        float theta = this.yaw;
        float rho = maxFollowDistance;

        Vector3 delta = new Vector3(rho * Mathf.Sin(phi) * Mathf.Cos(theta),
        	                        rho * Mathf.Cos(phi),
	                                rho * Mathf.Sin(phi) * Mathf.Sin(theta));

        // Vector3 truePosition = follow.transform.position + deltaPosition;
        // Vector3 animate = Vector3.SmoothDamp(this.transform.position, truePosition, ref vZero, 0.25f);
        Vector3 ft = IsPlayer2D() ? (follow2d.transform.position + storedDelta) 
                                  : follow.transform.position;
        Vector3 targetPosition = ft - delta;

        this.transform.position = targetPosition;
        this.transform.LookAt(ft);
    }
}
