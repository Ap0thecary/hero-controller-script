using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Screenshot : MonoBehaviour
{
    public KeyCode takeScreenshot = KeyCode.Print;
    public int supersampling = 2;
    public string filePath = "C:\\Users\\%USERNAME%\\Documents\\MyGames\\Screenshots\\";
    public string filePrefix = "Screenshot_";
    public string fileName = "";
    string file;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        file = filePath + filePrefix + fileName;
        if(Input.GetKey(takeScreenshot))
        {
            ScreenCapture.CaptureScreenshot(file, supersampling);
            Debug.Log("Screenshot saved to" + file);
        }
    }
}
