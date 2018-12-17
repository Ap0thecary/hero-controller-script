using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroCamera : MonoBehaviour
{
    Camera firstPerson;

    public float sensitivity = 5f;

    public float yaw;
    float pitch;

    public float thirdPersonDistance = 1;
    public float thirdPersonHeight;
    public float thirdPersonCollisionRadius = 1;

    public bool isCursorLocked = true;
    public bool isThirdPerson = false;

    public bool isLegacy = false;   

    void Start()
    {
        firstPerson = GetComponent<Camera>();
        if (isLegacy == false)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    void Update()
    {
       
        yaw += Input.GetAxis("Mouse X");
        pitch += Input.GetAxis("Mouse Y");

        firstPerson.transform.rotation = Quaternion.Euler(new Vector3(-pitch, yaw, 0) * sensitivity);

        if (isThirdPerson == true)
        {
            //Third Person Adjustments
        }
        if (isLegacy)
        {

        }
    }

}
