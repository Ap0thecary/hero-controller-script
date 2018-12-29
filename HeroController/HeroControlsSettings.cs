using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class HeroControlsSettings
{
    public KeyCode forward = KeyCode.W;
    public KeyCode left = KeyCode.A;
    public KeyCode right = KeyCode.D;
    public KeyCode back = KeyCode.S;
    public KeyCode run = KeyCode.LeftShift;

    public bool isLegacy = false;
    public bool isThirdPerson = false;

    public KeyCode jump = KeyCode.Space;
    public float jumpStrength = 300f;
    public float jumpThreshold = .35f;
    public float legacyMaxJump = 1f;

    public bool strafeEnabled = false;
    public KeyCode strafeLeft = KeyCode.Q;
    public KeyCode strafeRight = KeyCode.E;
    
    //Hmmst.
    public KeyCode screenshot = KeyCode.SysReq;
}
