using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroController : MonoBehaviour
{
    Camera heroCamera;
    Rigidbody rbPhysics;
    CapsuleCollider col;
    public bool isThirdPerson = false;

//Movement Stats

    public float walkSpeed = 5f;
    public float runSpeed = 10f;
    public bool isRunning = false;
    public bool isFlying = false;
    public bool isGrounded = true;

//Control Mappings

//May be migrated to a separate script at a later date
    public KeyCode forward = KeyCode.W;
    public KeyCode left = KeyCode.A;
    public KeyCode right = KeyCode.D;
    public KeyCode back = KeyCode.S;
    public KeyCode run = KeyCode.LeftShift;

    public KeyCode jump = KeyCode.Space;
    public float jumpStrength = 1f;
    public float legacyMaxJump = 1f;

    public bool strafeEnabled = false;

    public KeyCode strafeLeft = KeyCode.Q;
    public KeyCode strafeRight = KeyCode.E;

    public float jumpHeightMax = 1f;

    //Control Vars
    private float cameraYaw;
    private Vector3 motor;

    Vector3 forwardDir;
    void Start()
    {
        heroCamera = GetComponentInChildren<Camera>();
        rbPhysics = GetComponent<Rigidbody>();
        col = GetComponent<CapsuleCollider>();
    }
    private void OnCollisionEnter(Collision groundCheck)
    {
        //Use to reset isGrounded to true

        //Use the ContactPoint Normal direction within a threshold to determine if isGrounded
        //should be reset. Otherwise, the surface is "too steep" and cannot be jumped from.
    }
    void FixedUpdate()
    //Use FixedUpdate to prevent object translation from being out of sync with physics frames
    {
        motor = Vector3.zero;

        cameraYaw = Mathf.Deg2Rad * heroCamera.transform.rotation.eulerAngles.y;

        motor.x = Mathf.Sin(cameraYaw);
        motor.z = Mathf.Cos(cameraYaw);

        if (Input.GetKey(forward))
        {
            transform.Translate(motor * Time.deltaTime * walkSpeed);
            //rbPhysics.MovePosition(motor);
            //If clipping is due to physics, attempt this
        }
        if (Input.GetKey(back))
        {
            transform.Translate(-motor * Time.deltaTime * walkSpeed);
        }
        if (Input.GetKey(right))
        {
            transform.Translate(-Vector3.Cross(motor, Vector3.up) * Time.deltaTime * walkSpeed);
        }
        if (Input.GetKey(left))
        {
            transform.Translate(Vector3.Cross(motor, Vector3.up) * Time.deltaTime * walkSpeed);
        }
        transform.rotation = Quaternion.AngleAxis(cameraYaw, Vector3.up);
        if (Input.GetKeyDown(jump) && isGrounded == true)
        {
            isGrounded = false;
            //FPS Style jumping seems to be fairly constant, regardless of how long the space bar is held.
            //This of course excludes modes of movement that differ from this norm, such as gliding with space held.
            //Jumping in RPGs seems to be partially dependent on the length of time space is held down, up to a limit.

            rbPhysics.AddForce(Vector3.up * jumpStrength, ForceMode.Impulse);
            //Impulse based movement is used to create a consistant jump height generally expected of an FPS character.

            //Legacy jump will increase increase the y value of the motor vector until
            //a difference in position equal to the max height is reached, or until the
            //jump hotkey is released.
            
        }
        /*LEGACY:
         * if(rmb down)
         * {
         *  lock cursor
         *  rotate view
         *  rotate character to face view
         * }
         * if(lmb down)
         * {
         *  lock cursor
         *  rotate only the view 
         * }
         */
    }
}
