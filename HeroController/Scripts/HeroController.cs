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
    [HideInInspector] public bool isLegacy = false;
    public bool isThirdPerson = false;
    public bool adaptiveView = false;
    //Allows transition from first to third person and vice-verse when zooming out in first person,
    //or zooming in past [minDistance] in third person.

    [Header("Movement Attributes")]
    public float walkSpeed = .1f;
    public float runSpeedMult = 2f;
    public bool isRunning = false;
    public bool isFlying = false;
    public bool isGrounded = true;

//Camera Attributes
    [Header("Camera Attributes")]
    public float cameraHeight = .125f;
    public float sensitivity = 1;
    public float maxZoomDistance = .5f;
    public float minZoomDistance = 2f;
    
    [Header("Control Mappings")]
    public KeyCode forward = KeyCode.W;
    public KeyCode left = KeyCode.A;
    public KeyCode right = KeyCode.D;
    public KeyCode back = KeyCode.S;
    public KeyCode run = KeyCode.LeftShift;
    public KeyCode crouch = KeyCode.LeftControl;

    [Header("Jump Settings")]
    public KeyCode jump = KeyCode.Space;
    public float jumpStrength = 300f;
    [Tooltip("This threshold is tested against to determine if a surface is horizontal enough to qualify as \'grounded\'. A value of zero will cause all surfaces to be considered ground, while a value of one will only consider perfectly horizontal surfaces to be ground.")]
    public float jumpThreshold = .35f;
    //public float legacyMaxJump = 1f;

    [Header("Strafe Settings")]
    [Tooltip("Allows diagonal forwards movement. Should not be enabled without Legacy Mode, especially if you plan to use the Q and E keys for something else.")]
    public bool strafeEnabled = false;
    public KeyCode strafeLeft = KeyCode.Q;
    public KeyCode strafeRight = KeyCode.E;

    [Header("Wall Jumping")]
    public bool canWallJump = false;
    public float hangTime = 2f;

    void Start()
    {
        heroViewer = GetComponentInChildren<HeroViewer>();
        mainCamera = GetComponent<Camera>();

        rbPhysics = GetComponent<Rigidbody>();
        col = GetComponent<CapsuleCollider>();
    }

    private void OnCollisionEnter(Collision groundCheckEnter)
    {
        //Only check if you're not on the ground
        if (isGrounded == false)
        {
            int numContacts = groundCheckEnter.contactCount;
            ContactPoint[] cp = new ContactPoint[numContacts];
            //Get all contact points from the collision
            groundCheckEnter.GetContacts(cp);
            //Test them. Each of them. TEST ALL OF THEM.
            for (int con = 0; con < numContacts; con++)
            {
                //Create a comparison between the normal of the contact point and the up-direction.
                //Spherical worlds not yet supported.
                float dotComparison = Vector3.Dot(cp[con].normal, Vector3.up);
                if (dotComparison > jumpThreshold)
                {
                    isGrounded = true;
                    break;
                    //Break from the for statement, a point has been found!
                }
                else isGrounded = false;
                //Or just stay here. We don't mind. We enjoy the company.
                //Unless that was the last contact point, in which case, get out.
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(jump) && isGrounded == true)
        {
            isGrounded = false;
            rbPhysics.AddForce(Vector3.up * jumpStrength, ForceMode.Impulse);
        } 
        if (transform.position.y < -25)
        {
            transform.position = new Vector3(0, 1, 0);
        }
    }
    void FixedUpdate()
    {
        Vector3 motor = Vector3.zero;   

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
        
        if(strafeEnabled)
        {
            Vector3 lStrafeDir = Vector3.Slerp(motor,crossMotor,.5f);
            Vector3 rStrafeDir = Vector3.Slerp(motor,-crossMotor,.5f);
            if (Input.GetKey(strafeLeft)) transform.Translate(lStrafeDir);
            if (Input.GetKey(strafeRight)) transform.Translate(rStrafeDir);
        }

        transform.rotation = Quaternion.AngleAxis(cameraYaw, Vector3.up);
    }

    public float GetCameraRotation()
    {
        float yaw;

        if (isThirdPerson) yaw = heroViewer.pivot.transform.rotation.eulerAngles.y;
        else yaw = heroViewer.transform.rotation.eulerAngles.y;

        return yaw;
    }
    public void WallJump(float threshold, float duration, float minHeight)
    {
        //facing normal = forward * rigidbody rotation
        //wall normal dot facing normal
        //if above threshold, lock movement for a mo', and reset isGrounded to true for duration
        //Test against minHeight from the ground, may be masked off.

    }

}