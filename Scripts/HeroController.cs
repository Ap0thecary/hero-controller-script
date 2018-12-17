using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroController : MonoBehaviour
{
    private Camera heroCamera;


    public KeyCode forward = KeyCode.W;
    public KeyCode left = KeyCode.A;
    public KeyCode right = KeyCode.D;
    public KeyCode back = KeyCode.S;

    public KeyCode jump = KeyCode.Space;
    
    public bool strafingEnabled = false;
    public KeyCode strafeLeft = KeyCode.Q;
    public KeyCode strafeRight = KeyCode.E;

    public float walkSpeed = 1f;
    public float runSpeed = 2f;
    public float jumpStrength = 1f;



    void Start()
    {
        heroCamera = GetComponentInChildren<Camera>();

    }

    // Update is called once per frame
    void Update()
    {
        float rotation = Mathf.Deg2Rad * heroCamera.transform.rotation.eulerAngles.y;
        Vector3 motor = Vector3.zero;
        motor.x = Mathf.Sin(rotation);
        motor.z = Mathf.Cos(rotation);
        Debug.Log("Direction: (" + motor.x + " , " + motor.z + ") Rotation: " + rotation);
        if (Input.GetKey(forward))
        {
            gameObject.transform.Translate(motor * Time.deltaTime * walkSpeed);
        }
        if (Input.GetKey(back))
        {
            gameObject.transform.Translate(-motor * Time.deltaTime * walkSpeed);
        }
        if (Input.GetKey(left))
        {
            gameObject.transform.Translate(Vector3.Cross(motor,Vector3.up) * Time.deltaTime * walkSpeed);
        }
        if (Input.GetKey(right))
        {
            gameObject.transform.Translate(-Vector3.Cross(motor,Vector3.up) * Time.deltaTime * walkSpeed); 
        }
        gameObject.transform.rotation = Quaternion.AngleAxis(rotation, Vector3.up);
        if (Input.GetKey(jump))
        {

        }
        //gameObject.transform.rotation = Quaternion.AngleAxis(rotation, Vector3.up);
    }
}
