using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroController : MonoBehaviour
{
    Camera heroCamera;
    Rigidbody rbPhysics;
    public bool isThirdPerson = false;

//Movement Stats

    public float walkSpeed = 5f;
    public float runSpeed = 10f;
    public float jumpStrength = 1f;
    public bool isRunning = false;
    public bool isFlying = false;
    public bool isGrounded = true;

//Control Mappings

    public KeyCode forward = KeyCode.W;
    public KeyCode left = KeyCode.A;
    public KeyCode right = KeyCode.D;
    public KeyCode back = KeyCode.S;
    public KeyCode run = KeyCode.LeftShift;

    public KeyCode jump = KeyCode.Space;
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
            //FPS Style jumping seems to be fairly constant, regardless of how long the space bar is held
            //This of course excludes modes of movement that differ from this norm, such as gliding with space held

            //Jumping in RPGs seems to be partially dependent on the length of time space is held down, up to a limit.

            //Add lift & velocity
            //Get the position you would jump to
            //Vector3 relativePosition = rbPhysics.velocity;
            //rbPhysics.MovePosition(relativePosition);
            //Alternatively, add motion to a certain maximum
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
