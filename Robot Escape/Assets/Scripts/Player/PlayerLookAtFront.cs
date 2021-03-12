using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class PlayerLookAtFront : MonoBehaviour
{
    public Transform head;
    public Transform cam;
    public RotationConstraint constraint;
    float power = 1;
    float prevRotY;
    float rotationWeight;
    public float returnSpeed = 0.5f;

    void Start()
    {
        prevRotY = cam.rotation.y;
        power = 1;
    }

    void Update()
    {
        if (Mathf.Sign(cam.rotation.y) != Mathf.Sign(prevRotY)){
            power = 0;
            print("Rot found!");
        }
        else {
            Quaternion rot = cam.rotation;
            rot.x = 0;
            rot.y = rot.y * power;
            rot.z = 0;
            head.rotation = rot;
        }

        if (power < 1){
            power = Mathf.Clamp(power + returnSpeed * Time.deltaTime, 0, 1);
        }

        prevRotY = cam.rotation.y;

        print(cam.rotation.y);
    }
}
