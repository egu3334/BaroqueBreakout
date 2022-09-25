using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHumanoidModelController : MonoBehaviour
{

    float velocityX = 0.0f;
    float velocityY = 0.0f;

    float maxVel = 1.0f;
    float minVel = -1.0f;

    public float acceleration = 2.0f;
    public float deceleration = 1.0f;

    // float rotSpeed = 100;
    // float rot = 0f;
    // float gravity = 8;

    // Vector3 moveDir = Vector3.zero;

    CharacterController controller;
    Animator anim;
    PlayerInput controls;
    Keyboard kb;

    int isCrouchingHash;
    int isJumpingHash;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        controls = GetComponent<PlayerInput>();
        kb = InputSystem.GetDevice<Keyboard>();

        isCrouchingHash = Animator.StringToHash("isCrouching");
        isJumpingHash = Animator.StringToHash("isJumping");
    }



    // Update is called once per frame
    void Update() {

        bool crouchPressed = kb.cKey.isPressed;
        bool isCrouching = anim.GetBool(isCrouchingHash);

        bool jumpPressed = kb.vKey.isPressed;
        bool shiftPressed = kb.spaceKey.wasPressedThisFrame;

        bool forwardPressed = kb.wKey.isPressed;
        bool backPressed = kb.sKey.isPressed;
        bool leftPressed = kb.aKey.isPressed;
        bool rightPressed = kb.dKey.isPressed;

        bool noXPressed = !leftPressed && !rightPressed;
        bool noYPressed = !forwardPressed && ! backPressed;
        bool noDirectionPressed = noXPressed && noYPressed;

        //if (controller.isGrounded) {
            //crouching
            if (crouchPressed  && (velocityY > -0.05 && velocityY < 0.05)) {
                anim.SetBool(isCrouchingHash, true);
            } else {
                anim.SetBool(isCrouchingHash, false);
            }

            //jumping
            if (jumpPressed) {
                anim.SetBool(isJumpingHash, true);
            } else {
                anim.SetBool(isJumpingHash, false);
            }

            //acceleration block
            if (forwardPressed && velocityY < maxVel && !isCrouching) {
                velocityY += Time.deltaTime * acceleration;
            }

            if (backPressed && velocityY > minVel && !isCrouching) {
                velocityY -= Time.deltaTime * acceleration;
            }

            if (leftPressed && velocityX > minVel && !isCrouching) {
                velocityX -= Time.deltaTime * acceleration;
            }

            if (rightPressed && velocityX < maxVel && !isCrouching) {
                velocityX += Time.deltaTime * acceleration;
            }

            //deceleration block
            if (!forwardPressed && velocityY > 0.0f) {
                velocityY -= Time.deltaTime *deceleration;
            }

            if (!backPressed && velocityY < 0.0f) {
                velocityY += Time.deltaTime *deceleration;
            }

            if (noYPressed && velocityY != 0.0f && (velocityY > -0.05 && velocityY < 0.05)) {
                velocityY = 0.0f;
            }

            if (!leftPressed && velocityX < 0.0f) {
                velocityX += Time.deltaTime *deceleration;
            }

            if (!rightPressed && velocityX > 0.0f) {
                velocityX -= Time.deltaTime *deceleration;
            }

            if (noXPressed && velocityX != 0.0f && (velocityX > -0.05 && velocityX < 0.05)) {
                velocityX = 0.0f;
            }


        //}

        anim.SetFloat("velx", velocityX);
        anim.SetFloat("vely", velocityY);

    }
}
