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

    //I AM GONNA PUT A WALL JUMPING FEATURE IN
    //AND NOBODY CAN TELL ME OTHERWISE
    //GO AHEAD TRY TO STOP ME
    //YOU CANNOT BECAUSE I AM SLIPPERY AND COVERED IN JAM
    public bool canWallJump = false;
    public float hangTime = 5f;
    //Also an angle threshold or something?

    //Control Vars
    private float cameraYaw;
    private Vector3 motor;

    void Start()
    {
        //get all the stuff I need to operate on
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
    void Update()
    {
        if (Input.GetKeyDown(jump) && isGrounded == true)
        { 
            //isGrounded = false;

            //FPS Style jumping seems to be fairly constant, regardless of how long the space bar is held.
            //This of course excludes modes of movement that differ from this norm, such as gliding with space held.
            //Jumping in RPGs seems to be partially dependent on the length of time space is held down, up to a limit.

            rbPhysics.AddForce(Vector3.up * jumpStrength, ForceMode.Impulse);

            //Legacy jump will increase increase the y value of the motor vector until
            //a difference in position equal to the max height is reached, or until the
            //jump hotkey is released.
        } 
    }
    void FixedUpdate()
    {
        motor = Vector3.zero;

        cameraYaw = Mathf.Deg2Rad * heroCamera.transform.rotation.eulerAngles.y;
        motor.x = Mathf.Sin(cameraYaw);
        motor.z = Mathf.Cos(cameraYaw);

        //This seems stupid, I'm just gonna stick it all in one place and do it all at once
        if (Input.GetKey(forward))
        {
            transform.Translate(motor * Time.deltaTime * walkSpeed);
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
