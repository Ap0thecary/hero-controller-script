using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroCamera : MonoBehaviour
{
    //GameObject player;
    //HeroController heroController;
    
    public float sensitivity;
    public float height;

    public bool isThirdPerson;
    public float distance = 3;

    public float yaw;
    private float pitch;

    //Legacy camera is refering to the style of control used in MMORPGS such as Final Fantasy XIV
    public bool isLegacy = false;
    private int camSynced = 1;
    private int camUnsynced = 0;
    
     
    void Start()
    {
        //player = GetComponentInParent<GameObject>();
        //heroController = GetComponentInParent<HeroController>();
        Cursor.lockState = CursorLockMode.Locked;

    }
    
    void Update()
    {
        if (isLegacy == false)
        {
            pitch += Input.GetAxis("Mouse Y");
            yaw += Input.GetAxis("Mouse X");
            gameObject.transform.rotation = Quaternion.Euler(new Vector3(-pitch, yaw, 0) * sensitivity);
        }
        if (isLegacy && Input.GetMouseButton(camSynced))
        {
            
        }
        if (isLegacy && Input.GetMouseButton(camUnsynced))
        {

        }
    }
}
