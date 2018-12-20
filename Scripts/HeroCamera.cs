using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroCamera : MonoBehaviour
{
    Camera heroCamera;
    Vector3 parentPosition;
    HeroController heroController;
    public float sensitivity = 5f;

    public float yaw;
    private float pitch;
    private float zoomDistance;

    public float thirdPersonDistance = 1;
    public float thirdPersonHeight;
    [Tooltip("Collision detection radius for third-person mode")]
    public float thirdPersonCollisionRadius = 1;

    public float tpMaxDistance = 3;
    public float tpMinDistance = 1;

    public bool isCursorLocked = true;
    public bool isThirdPerson = false;

    private Quaternion rotary;
    //rotary Vector used to rotate the camera in First-Person modes
    [Tooltip("An old-fashioned RPG-style control scheme")]
    public bool isLegacy = false;   
    //Need to consolidate settings that affect both camera and controller to one script

    void Start()
    {
        heroCamera = GetComponent<Camera>();
        heroController = GetComponentInParent<HeroController>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        parentPosition = transform.parent.position;
        
        yaw += Input.GetAxis("Mouse X");
        pitch += Input.GetAxis("Mouse Y");
        pitch = Mathf.Clamp(pitch, -90, 90);
        //Pitch clamp is out here now. Vertigo problem solved!

        rotary = Quaternion.Euler(new Vector3(-pitch, yaw, 0) * sensitivity);
        //I moved the quaternion magic out here so now it only needs to be done once.

        zoomDistance += Input.GetAxis("Mouse ScrollWheel");

        if (isThirdPerson == false)
        {
            heroCamera.transform.rotation = rotary;
        }
        if (isThirdPerson == true)
        {
            if (Input.GetAxis("Mouse ScrollWheel") != 0f)
            {
                //Make sure you're within a reasonable distance to the player
                thirdPersonDistance = Mathf.Clamp(thirdPersonDistance + zoomDistance, tpMinDistance, tpMaxDistance);
                transform.position = parentPosition - new Vector3(thirdPersonDistance, 0, 0);
            }

            heroCamera.transform.Rotate(Vector3.up,yaw);
            //Eldritch magicks are required here that I have yet to understand
            heroCamera.transform.Rotate(Vector3.right, pitch);
        }
        //LEGACY TIME bOIS
        if (isLegacy == true)
        {
            //Reset cursor to default so you can click stuff
            Cursor.lockState = CursorLockMode.None;
            //Do the stuff that controls the camera
            if (Input.GetMouseButton(0)) //Left Mouse
            {
                Cursor.lockState = CursorLockMode.Locked;
                heroCamera.transform.rotation = rotary;
            }
            if (Input.GetMouseButton(1)) //Right Mouse
            {
                //Tell the HeroController to rotate to the current direction of the camera
                Cursor.lockState = CursorLockMode.Locked;
                heroCamera.transform.rotation = rotary;
            }
            //The following is for my own benefit:
            //IN BOTH THIRD AND FIRST PERSON MODE, THE ROTATION OF THE PLAYER WILL BE ENTIRELY MOUSE-DRIVEN
            //LEFT AND RIGHT DIRECTIONAL INPUT SHALL ONLY CONTROL POSIITON.
        }
    }

}
