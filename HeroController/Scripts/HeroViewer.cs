using System;
using System.Collections.Generic;
using UnityEngine;

//Revised version of HeroCamera. Will eventually replace it with this, probably.
// TODO: Move parameters into HeroController

public class HeroViewer : MonoBehaviour
{

    private Camera heroCamera;
    private HeroController heroController;

    [HideInInspector] public GameObject pivot;

    private bool isThirdPerson;
    private bool isLegacy;
    public bool isAdaptive;

    private float sensitivity = 1;
    private float transitionSpeed;
    private float cameraHeight;

    [HideInInspector] public float yaw;
    private float pitch;

    private float zoomDistance = .75f;
    private float minDistance = .5f;
    private float maxDistance = 2f;

    private bool isCursorLocked;
    private bool isTransitioning = false;

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
        ControllerCheck();
        
        transform.localRotation = Quaternion.Euler(Vector3.zero);

        zoomDistance += -Input.GetAxis("Mouse ScrollWheel");
        zoomDistance = Mathf.Clamp(zoomDistance,minDistance,maxDistance);

        yaw += Input.GetAxis("Mouse X");
        pitch += Input.GetAxis("Mouse Y");
        pitch = Mathf.Clamp(pitch, -85, 85);

        Quaternion rotation = Quaternion.Euler(new Vector3(-pitch,yaw,0) * sensitivity);

        if (isThirdPerson) ThirdPersonTransform(rotation);
        else FirstPersonTransform(rotation);
        
        if (isAdaptive)
        {
            FirstPersonTransition();
            ThirdPersonTransition();
        }

        if(isLegacy) Cursor.lockState = CursorLockMode.Confined;
        else Cursor.lockState = CursorLockMode.Locked;
    }

    public void ControllerCheck()
    {
        //Check HeroController for cameraModes
        isThirdPerson = heroController.isThirdPerson;
        isLegacy = heroController.isLegacy;
        isAdaptive = heroController.adaptiveView;

        //Check for Settings
        sensitivity = heroController.sensitivity;
        cameraHeight = heroController.cameraHeight;
        maxDistance = heroController.maxZoomDistance;
        minDistance = heroController.minZoomDistance;
    }

    public void ThirdPersonTransition()
    {
        //Are you already at the minimum distance, and still trying to zoom in?
        if (isThirdPerson == true && zoomDistance == minDistance && Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            //Then try NEW! ThirdPersonTransition, for all your view-switching needs
            transform.position = Vector3.Slerp(transform.position,pivot.transform.position,Time.deltaTime);
            isThirdPerson = false;
        }
    }

    public void FirstPersonTransition()
    {
        //In First Person? Need to see more? Trying, desperately, to zoom out, desipite knowing your efforts are futile?
        //Then submit.
        if (isThirdPerson == false && Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            //OR, try NEW! FirstPersonTransition, For peeking around corners unnecessarily
            //Lerp to [minDistance]
            Vector3 minPosition = new Vector3(transform.localPosition.x,transform.localPosition.y, minDistance);
            transform.position = Vector3.Slerp(transform.position,minPosition,Time.deltaTime);
            isThirdPerson = true;
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
        if(Physics.Linecast(pivot.transform.position,transform.position,out hit,9))
        {
            transform.position = hit.point;
        }

        pivot.transform.rotation = rotation;
    }
}