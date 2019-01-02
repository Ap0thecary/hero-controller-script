using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroController : MonoBehaviour
{
    HeroViewer heroViewer;
    Camera mainCamera;
    Rigidbody rbPhysics;
    CapsuleCollider col;
    
    [Header("Mode Settings")]
    public bool isLegacy = false;
    public bool isThirdPerson = false;
    public bool adaptiveView = false;
    //Allows transition from first to third person and vice-verse when zooming out in first person,
    //or zooming in past [minDistance] in third person.

//Movement Stats
    [Header("Movement Attributes")]
    public float walkSpeed = .1f;
    public float runSpeedMult = 2f;
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

    [Header("Wall Jumping")]
    public bool canWallJump = false;
    public float hangTime = 5f;

    //Control Vars

    private Vector3 motor;

    void Start()
    {
        heroViewer = GetComponentInChildren<HeroViewer>();
        mainCamera = GetComponent<Camera>();

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

        float cameraYaw = Mathf.Deg2Rad * GetCameraRotation();
        motor.x = Mathf.Sin(cameraYaw);
        motor.z = Mathf.Cos(cameraYaw);

        Vector3 crossMotor = Vector3.Cross(motor, Vector3.up) * walkSpeed;
        motor = motor * walkSpeed;

        if (Input.GetKey(run))
        {
            motor *= runSpeedMult;
            crossMotor *= runSpeedMult;
        }

        if (Input.GetKey(forward))  transform.Translate(motor);
        if (Input.GetKey(back))     transform.Translate(-motor);
        if (Input.GetKey(left))     transform.Translate(crossMotor);
        if (Input.GetKey(right))    transform.Translate(-crossMotor);

        transform.rotation = Quaternion.AngleAxis(cameraYaw, Vector3.up);

    }

    public float GetCameraRotation()
    {
        float yaw;

        if (isThirdPerson) yaw = heroViewer.pivot.transform.rotation.eulerAngles.y;
        else yaw = heroViewer.transform.rotation.eulerAngles.y;

        return yaw;
    }

    public void LegacyMode()
    {

    }
}