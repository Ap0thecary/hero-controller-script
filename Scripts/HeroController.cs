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
    public float jumpStrength = 200f;
    public float jumpResetThreshold = .35f;
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
        int numContacts = groundCheck.contactCount;
        ContactPoint[] cp;
        cp = new ContactPoint[numContacts];
        groundCheck.GetContacts(cp);

        for (int con = 0; con < numContacts; con++)
        {
            //Compare the normal of the point of collision to up, see if it is up
            if (Vector3.Dot(cp[con].normal, Vector3.up) > jumpResetThreshold) isGrounded = true;
            else isGrounded = false;
        }
        Debug.Log("# of contact points: " + numContacts + "\nFirst ContactPoint normal: " + cp[0].normal + "\nDot Product: " + Vector3.Dot(cp[0].normal,Vector3.up));
        //Compare cp[].normal to Vector3.up using Vector3.Dot(cp[x].normal,Vector3.up)
        //FOR-EACH LOOP BINCH
        //Use the ContactPoint Normal direction within a threshold to determine if isGrounded
        //should be reset. Otherwise, the surface is "too steep" and cannot be jumped from.
    }
    void Update()
    {
        if (Input.GetKeyDown(jump) && isGrounded == true)
        { 
            //isGrounded = false;
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

        Vector3 crossMotor = Vector3.Cross(motor, Vector3.up) * walkSpeed;
        motor = motor * walkSpeed;

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
