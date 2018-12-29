using System;
using System.Collections.Generic;
using UnityEngine;

//Revised version of HeroCamera. Will eventually replace it with this, probably.

public class HeroViewer : MonoBehaviour
{
    private Camera heroCamera;
    private HeroController heroController;
    private GameObject pivot = new GameObject();

    public float sensitivity = 1;

    [HideInInspector]
    public float yaw;
    private float pitch;

    public float zoomDistance;
    public float minDistance = 1f;
    public float maxDistance = 5f;

    private bool isThirdPerson;
    private bool isLegacy;
    private bool isAdaptive;

    private bool isCursorLocked;
    private bool isTransitioning = false;

    private Vector3 parentPosition;

    void Start()
    {
        heroCamera = GetComponent<Camera>();
        heroController = GetComponentInParent<HeroController>();
        pivot.transform.position = Vector3.zero;
    }
    
    void LateUpdate()
    {
        modeCheck();
        
        zoomDistance += -Input.GetAxis("Mouse ScrollWheel");
        zoomDistance = Mathf.Clamp(zoomDistance,minDistance,maxDistance);

        if (isThirdPerson) ThirdPersonTransition(sensitivity,zoomDistance);
        else FirstPersonTransition(sensitivity);

        if(isLegacy) Cursor.lockState = CursorLockMode.Confined;
        else Cursor.lockState = CursorLockMode.Locked;
    }

    public void modeCheck()
    {
        //Check heroController for cameraModes
        isThirdPerson = heroController.isThirdPerson;
        isLegacy = heroController.isLegacy;
        isAdaptive = heroController.adaptiveView;
    }

    public void FirstPersonTransition(float sensitivity)
    {
        yaw += Input.GetAxis("Mouse X");
        pitch += Input.GetAxis("Mouse Y");
        pitch = Mathf.Clamp(pitch, -85, 85);

        Quaternion rotation = Quaternion.Euler(new Vector3(-pitch,yaw,0) * sensitivity);
        transform.rotation = rotation;
    }

    public void ThirdPersonTransition(float sensitivity, float zoomDistance)
    {
        //The problem seems to lie in that it builds the rotation in small increments instead of keeping track of the actual rotation of the object
        yaw += Input.GetAxis("Mouse X");
        pitch += Input.GetAxis("Mouse Y");

        //Fuck this I'll just make it work based on a pivot
        //Beauty in simplicity
        //heroCamera.transform.position = transform.parent.position;
        //heroCamera.transform.Translate(new Vector3(0,0,-zoomDistance));
        //heroCamera.transform.RotateAround(transform.parent.position,Vector3.up,yaw);
        ////heroCamera.transform.rotation = Quaternion.LookRotation(transform.parent.position,Vector3.up);
        ////Vector3 horizontalRotation = new Vector3(transform.rotation.eulerAngles.x,0,transform.rotation.eulerAngles.z);
        ////Debug.Log(horizontalRotation);
        //Vector3 pitchAxis = Vector3.Cross(new Vector3(transform.localPosition.x,0,transform.localPosition.z),Vector3.up);
        //Debug.Log(pitchAxis);
        //heroCamera.transform.RotateAround(transform.parent.position,pitchAxis,-pitch);
        //transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x,transform.rotation.eulerAngles.y,0);

    }
}







//First, we set the camera's position the same as the controller/parent.
//Ok so for now lets say that distance from the camera is CONSTANT. We'll set it, as such, on the first step.
//Set the camera's position to the default zoom distance.

//Legacy mode can be implemented by reducing the motion vectors to zero unless the requisite mousebutton is held down
//May need to tidy that up in it's own function. All good. 