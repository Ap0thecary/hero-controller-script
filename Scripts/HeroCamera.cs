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

    [Tooltip("An old-fashioned RPG-style control scheme")]
    public bool isLegacy = false;   

    void Start()
    {
        heroCamera = GetComponent<Camera>();
        heroController = GetComponentInParent<HeroController>();
        if (isLegacy == false)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    void Update()
    {
        parentPosition = transform.parent.position;
        
        yaw += Input.GetAxis("Mouse X");
        pitch += Input.GetAxis("Mouse Y");

        zoomDistance += Input.GetAxis("Mouse ScrollWheel");

        if (isThirdPerson == false)
        {
            heroCamera.transform.rotation = Quaternion.Euler(new Vector3(-pitch, yaw, 0) * sensitivity);
        }
        if (isThirdPerson == true)
        {
            if (Input.GetAxis("Mouse ScrollWheel") != 0f)
            {
                thirdPersonDistance = Mathf.Clamp(thirdPersonDistance + zoomDistance, tpMinDistance, tpMaxDistance);
                transform.position = parentPosition - new Vector3(thirdPersonDistance, 0, 0);
            }
            pitch = Mathf.Clamp(pitch, -180, 180);
            heroCamera.transform.Rotate(Vector3.up,yaw);
            heroCamera.transform.Rotate(Vector3.right, pitch);
        }
        if (isLegacy)
        {
            //That legacy shite
        }
    }

}
