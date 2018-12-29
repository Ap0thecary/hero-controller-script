using System;
using System.Collections.Generic;
using UnityEngine;

public interface ICameraBehavior
{
    void CameraTransition(float yaw, float pitch, float sensitivity);
}
//An Interface might be the wrong thing, here. Not sure this is gonn' work out.
//I could be wrong.

//public class FirstPerson : MonoBehaviour, ICameraBehavior
//{
//    public void CameraTransition(float yaw, float pitch, float sensitivity)
//    {
//        Quaternion rotation = Quaternion.Euler(new Vector3(-pitch,yaw,0) * sensitivity);
//        gameObject.transform.rotation = rotation;
//    } 
//}
//public class ThirdPerson: MonoBehaviour, ICameraBehavior
//{
//    public void CameraTransition(float yaw, float pitch, float sensitivity)
//    {
//        transform.position = transform.parent.position - new Vector3(3,0,0);
//        transform.RotateAround(transform.parent.position,Vector3.up,yaw);
//        transform.RotateAround(transform.parent.position,Vector3.right,-pitch);
//        //Do the Third Person Stuff
//        //Also implement zooming here, it shouldn't be present in First Person anyway
//    }
//}