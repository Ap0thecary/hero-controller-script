using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroController : MonoBehaviour
{
    Camera heroCamera;
    Rigidbody rbPhysics;
    CapsuleCollider col;
    public bool isThirdPerson = false;
    public bool isLegacy = false;

//Movement Stats
    [Header("Movement Attributes")]
    public float walkSpeed = 5f;
    public float runSpeed = 10f;
    public bool isRunning = false;
    public bool isFlying = false;
    public bool isGrounded = true;

//Control Mappings

    [Header("Control Mappings")]
    public KeyCode forward = KeyCode.W;
    public KeyCode left = KeyCode.A;
    public KeyCode right = KeyCode.D;
    public KeyCode back = KeyCode.S;
    public KeyCode run = KeyCode.LeftShift;

    [Header("Jump Settings")]
    public KeyCode jump = KeyCode.Space;
    public float jumpStrength = 300f;
    public float jumpThreshold = .35f;
    public float legacyMaxJump = 1f;

    [Header("Strafe Settings")]
    [Tooltip("Allows diagonal forwards movement. Should not be enabled without Legacy Mode, especially if you plan to use the Q and E keys for something else.")]
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
        heroCamera = GetComponentInChildren<Camera>();
        rbPhysics = GetComponent<Rigidbody>();
        col = GetComponent<CapsuleCollider>();
    }
    private void OnCollisionEnter(Collision groundCheckEnter)
    {
        if (isGrounded == false)
        {
            int numContacts = groundCheckEnter.contactCount;
            ContactPoint[] cp;
            cp = new ContactPoint[numContacts];
            groundCheckEnter.GetContacts(cp);
            for (int con = 0; con < numContacts; con++)
            {
                float dotComparison = Vector3.Dot(cp[con].normal, Vector3.up);
                //Compare the normal of the point of collision to up, see if it is up
                if (dotComparison > jumpThreshold)
                {
                    isGrounded = true;
                    break;
                }
                else
                {
                    isGrounded = false;
                }
            }
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(jump) && isGrounded == true)
        {
            isGrounded = false;
            rbPhysics.AddForce(Vector3.up * jumpStrength, ForceMode.Impulse);

            //Legacy jump will increase increase the y value of the motor vector until
            //a difference in position equal to the max height is reached, or until the
            //jump hotkey is released.
        } 
        if (transform.position.y < -25)
        {
            transform.position = new Vector3(0, 1, 0);
        }
    }
    void FixedUpdate()
    {
        motor = Vector3.zero;

        cameraYaw = Mathf.Deg2Rad * heroCamera.transform.rotation.eulerAngles.y;
        motor.x = Mathf.Sin(cameraYaw);
        motor.z = Mathf.Cos(cameraYaw);

        Vector3 crossMotor = Vector3.Cross(motor, Vector3.up) * walkSpeed;
        motor = motor * walkSpeed;
        if (Input.GetKey(run))
        {
            motor *= runSpeed;
            crossMotor *= runSpeed;
        }
        if (Input.GetKey(forward))  transform.Translate(motor);
        if (Input.GetKey(back))     transform.Translate(-motor);
        if (Input.GetKey(left))     transform.Translate(crossMotor);
        if (Input.GetKey(right))    transform.Translate(-crossMotor);

        transform.rotation = Quaternion.AngleAxis(cameraYaw, Vector3.up);
        if (isLegacy == true)
        {
            //Stuff to do with the controller
            //Unsync camera from controller
        }
    }
}
