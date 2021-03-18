using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robot3Brain : RobotBrain
{
    public Transform wantGrabObject;
    public Transform holdingObject;
    void GrabTransform(Transform obj){
        wantGrabObject = obj;
    }
}
