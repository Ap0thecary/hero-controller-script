using System;
using System.Collections.Generic;
using UnityEngine;

//Revised version of HeroCamera. Will eventually replace it with this, probably.

public class HeroViewer : MonoBehaviour
{

    private Camera heroCamera;
    private HeroController heroController;
    private GameObject pivot;

    private bool isThirdPerson;
    private bool isLegacy;
    private bool isAdaptive;

    public float sensitivity = 1;
    public float cameraHeight = .75f;

    [HideInInspector]
    public float yaw;
    private float pitch;

    public float zoomDistance;
    public float minDistance = 1f;
    public float maxDistance = 5f;

    private bool isCursorLocked;
    private bool isTransitioning = false;

    private Vector3 parentPosition;

    void Start()
    {
        heroCamera = GetComponent<Camera>();
        heroController = GetComponentInParent<HeroController>();

        pivot = new GameObject("Pivot");
        pivot.transform.parent = heroController.transform;
        pivot.transform.localPosition = Vector3.zero;
    }
    
    void LateUpdate()
    {
        modeCheck();
        
        transform.localRotation = Quaternion.Euler(Vector3.zero);

        zoomDistance += -Input.GetAxis("Mouse ScrollWheel");
        zoomDistance = Mathf.Clamp(zoomDistance,minDistance,maxDistance);

        yaw += Input.GetAxis("Mouse X");
        pitch += Input.GetAxis("Mouse Y");
        pitch = Mathf.Clamp(pitch, -85, 85);

        Quaternion rotation = Quaternion.Euler(new Vector3(-pitch,yaw,0) * sensitivity);

        if (isThirdPerson) ThirdPersonTransform(rotation);
        else FirstPersonTransform(rotation);

        if(isLegacy) Cursor.lockState = CursorLockMode.Confined;
        else Cursor.lockState = CursorLockMode.Locked;
    }

    public void modeCheck()
    {
        //Check HeroController for cameraModes
        isThirdPerson = heroController.isThirdPerson;
        isLegacy = heroController.isLegacy;
        isAdaptive = heroController.adaptiveView;
    }

    public void FirstPersonTransform(Quaternion rotation)
    {
        if (transform.parent != heroController.transform)
        {
            transform.parent = heroController.transform;
        }
        transform.localPosition = new Vector3(0,cameraHeight,0);
        transform.rotation = rotation;
    }
    public void ThirdPersonTransform(Quaternion rotation)
    {
        if (transform.parent != pivot.transform)
        {
            transform.parent = pivot.transform;
        }
        transform.localPosition = Vector3.zero;
        transform.localPosition += new Vector3(0,cameraHeight,0);
        transform.localPosition += new Vector3(0,0,-zoomDistance);

        pivot.transform.rotation = rotation;
    }
    
    public void modeTransition()
    {
        //Make things happen
        if(isThirdPerson)
        {

        }
        else
        {

        }
    }
}

//First, we set the camera's position the same as the controller/parent.
//Ok so for now lets say that distance from the camera is CONSTANT. We'll set it, as such, on the first step.
//Set the camera's position to the default zoom distance.
//Legacy mode can be implemented by reducing the motion vectors to zero unless the requisite mousebutton is held down
//May need to tidy that up in it's own function. All good. 