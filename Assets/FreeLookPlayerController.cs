using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class FreeLookPlayerController : MonoBehaviour {

    public float fakeGravity;

    public InputAction movementInput;

    public GameObject camera;
    public GameObject player;
    public GameObject playerModel;
    public GameObject pauseMenu;
    public GameObject sprintMeter;

    public bool hasLight = false;
    public GameObject light;

    private float inputRotation;
    private float currentRelativeRotation;

    private bool isPlayerWalking = false;
    private float inputSpeed = 0.0f;
    private float currentSpeed = 0.0f;
    public float maxSpeed = 3.0f;
    public float sprintSpeedModifier = 2.0f;

    private bool isSprintKeyHeld = false;
    private bool isSprinting = false;
    private float sprintResource;
    private float sprintAvailableTime = -1.0f;

    public float sprintTimeLimit = 5.0f;
    public float sprintRegenPerSec = 0.75f;
    public float sprintCooldown = 2.0f;
    public float sprintMeterWidth = 200.0f;
    public float sprintMeterHeight = 6.0f;
    private Image sprintMeterImage;

    private FreeLookCameraController cameraControl;
    private Rigidbody rb;
    private Animator anim;

    [HideInInspector]
    public GameObject targetGuard;

    void Start() {
        Cursor.visible = false;

        this.cameraControl = camera.GetComponent<FreeLookCameraController>();
        this.rb = GetComponent<Rigidbody>();
        this.anim = playerModel.GetComponent<Animator>();

        this.sprintResource = sprintTimeLimit;

        if (this.sprintMeter != null) {
            this.sprintMeterImage = this.sprintMeter.GetComponent<Image>();
        }

        light = transform.GetChild(1).GetChild(2).gameObject;
    }

    public void Move(InputAction.CallbackContext context) {
        Vector2 dir = context.ReadValue<Vector2>();
        this.inputSpeed = Mathf.Min(1.0f, Mathf.Sqrt(dir.x * dir.x + dir.y * dir.y));
        this.inputRotation = Mathf.Atan2(-dir.x, dir.y);

        this.isPlayerWalking = this.inputSpeed > 0.0f;
    }

    public void Sprint(InputAction.CallbackContext context) {
        if (context.started) {
            this.isSprintKeyHeld = true;
        } else if (context.canceled) {
            this.isSprintKeyHeld = false;
        }
    }

    public bool IsSprinting() {
        return this.isSprinting;
    }

    private bool IsGrounded() {
        return Physics.Raycast(transform.position, Vector3.down, 1);
    }

    private bool CanSprint() {
        if (sprintAvailableTime > 0.0f && Time.fixedTime < sprintAvailableTime) {
            return false;
        }

        sprintAvailableTime = -1.0f;
        return sprintResource > 0.0f;
    }

    void Update() {
        float cameraRot = this.cameraControl.GetYaw();
        float movementAngle = cameraRot + inputRotation;
        float targetRot = 90 - ((movementAngle * 57.2957795131f) % 360.0f);

        float currDirection = player.transform.localEulerAngles.y;
        float lerpDirection = Mathf.LerpAngle(currDirection, targetRot, 0.01f);
        player.transform.localEulerAngles = new Vector3(0, lerpDirection, 0);

        this.isSprinting = false;
        if (this.isSprintKeyHeld && this.isPlayerWalking && this.CanSprint()) {
            this.inputSpeed = maxSpeed * sprintSpeedModifier;
            this.isSprinting = true;
        } else if (this.isPlayerWalking) {
            this.inputSpeed = maxSpeed;
        } else {
            this.inputSpeed = 0.0f;
        }

        this.currentSpeed = Mathf.Lerp(this.currentSpeed, this.inputSpeed, 0.25f);
        anim.SetFloat("vely", this.currentSpeed, 1.0f, 0.1f);
        rb.velocity = new Vector3(
            this.currentSpeed * Mathf.Cos(movementAngle),
            rb.velocity.y,
            this.currentSpeed * Mathf.Sin(movementAngle)
        );

        if (!IsGrounded()) {
            rb.velocity += new Vector3(0, fakeGravity, 0);
        }

        if (isSprinting) {
            this.sprintResource = Mathf.Max(0.0f, this.sprintResource - Time.deltaTime);
            if (this.sprintResource == 0.0f) {
                this.sprintAvailableTime = Time.fixedTime + this.sprintCooldown;
            }
        } else {
            this.sprintResource = Mathf.Min(this.sprintTimeLimit,
                this.sprintResource + (Time.deltaTime * this.sprintRegenPerSec));
        }

        if (this.sprintMeterImage != null) {
            float width = (this.sprintResource / this.sprintTimeLimit)
                        * this.sprintMeterWidth;

            (this.sprintMeterImage.transform as RectTransform).sizeDelta
                = new Vector2(width, sprintMeterHeight);

            if (this.CanSprint()) {
                this.sprintMeterImage.color = new Color32(255, 255, 255, 100);
            } else {
                this.sprintMeterImage.color = new Color32(127, 127, 127, 100);
            }
        }

        RaycastHit hit;
        if (Physics.Raycast(transform.position, playerModel.transform.forward, out hit, Mathf.Infinity)) {
            if (hit.collider.gameObject.layer == 13) {
                Debug.DrawRay(transform.position, playerModel.transform.forward * hit.distance, Color.yellow);
                this.targetGuard = hit.collider.gameObject;
            } else {
                this.targetGuard = null;
            }
        }
    }

    public void LightToggle(InputAction.CallbackContext context)
    {
        if (light.activeSelf == false)
            {
                light.SetActive(true);
            }
            else {
                light.SetActive(false);
            }
    }

}
