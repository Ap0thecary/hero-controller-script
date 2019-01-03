﻿using System;
using System.Collections.Generic;
using UnityEngine;

//Revised version of HeroCamera. Will eventually replace it with this, probably.

public class HeroViewer : MonoBehaviour
{

    private Camera heroCamera;
    private HeroController heroController;

    [HideInInspector]
    public GameObject pivot;

    private bool isThirdPerson;
    private bool isLegacy;
    private bool isAdaptive;

    public float sensitivity = 1;
    public float cameraHeight = .5f;

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
        ModeCheck();
        
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

    public void ModeCheck()
    {
        //Check HeroController for cameraModes
        isThirdPerson = heroController.isThirdPerson;
        isLegacy = heroController.isLegacy;
        isAdaptive = heroController.adaptiveView;
    }

    public void ModeTransition()
    {
        //Probably just transformations
        if(isThirdPerson)
        {
            //Shift camera to zoomDistance over a short duration
        }
        else
        {
            //Shift camera back to the origin and height, removing the pivot
        }
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
        RaycastHit hit;
        if(Physics.Linecast(pivot.transform.position,transform.position,out hit))
        {
            transform.position = hit.point;
        }
        pivot.transform.rotation = rotation;
    }
}