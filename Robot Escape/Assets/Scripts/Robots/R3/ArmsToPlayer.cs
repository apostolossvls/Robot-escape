using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmsToPlayer : MonoBehaviour
{
    public bool activated = true;
    public Transform target;
    public float targetImportance;
    public Transform armLeft, armRight;
    public Transform handLeft, handRight;
    public Transform leftTargetPoint, rightTargetPoint; 
    public Transform armLeftDefaultTarget, armRightDefaultTarget;
    public Transform face;
    public float rotationSpeed = 5f;
    public float grabHandleDistande = 0.4f;
    public float grabSpeed = 5;
    public bool grab;
    Quaternion handLeftDefaultRotation, handRightDefaultRotation;
    float armLenght;
    Vector3 leftTargetPointStart, rightTargetPointStart;
    Handle leftHandle, rightHandle;

    void Start(){
        leftHandle = null;
        rightHandle = null;
        armLenght = Vector3.Distance(armLeft.position, leftTargetPoint.position);
        if (leftTargetPoint) leftTargetPointStart = leftTargetPoint.localPosition;
        if (rightTargetPoint) rightTargetPointStart = rightTargetPoint.localPosition;
        if (handLeft) handLeftDefaultRotation = handLeft.rotation;
        if (handRight) handRightDefaultRotation = handRight.rotation;
    }   

    void FixedUpdate(){
        Transform targetL;
        Transform targetR;
        Handle handle = null;
        if (target != null && grab && targetImportance>0){
            handle = target.GetComponent<Handle>();
            if (handle != null){
                targetL = handle.pointsLeft != null ? handle.pointsLeft : target;
                if (handle.pointsRight != null)
                    targetR = handle.pointsRight;
                else targetR = handle.pointsLeft != null ? handle.pointsLeft : target;
                float dot = Vector3.Dot(transform.right, target.right);
                if (dot < 0){
                    Transform temp = targetL;
                    targetL = targetR;
                    targetR = temp;
                }
            }
            else {
                targetL = target;
                targetR = target;
            }
        }
        else {
            targetL = armLeftDefaultTarget;
            targetR = armRightDefaultTarget;
        }

        float distL = armLenght + 1;
        float distR = armLenght + 1;
        if (armLeft) distL = Vector3.Distance(armLeft.position, targetL.position);
        if (armRight) distR = Vector3.Distance(armRight.position, targetR.position);

        if (armLeft){
            Vector3 dirL = (targetL.position - armLeft.position).normalized;
            Quaternion rotL = Quaternion.Slerp(armLeft.rotation, Quaternion.LookRotation(dirL), Time.fixedDeltaTime * rotationSpeed);
            armLeft.rotation = rotL;
            if (distL < armLenght){
                leftTargetPoint.position = targetL.position;
            }
            else {
                leftTargetPoint.localPosition = leftTargetPointStart;
            }

            bool handlegrab = false;
            Quaternion newRot = handLeftDefaultRotation;
            if (handle != null){
                if (handle.pointsLeft != null){
                    float handleDist = Vector3.Distance(handLeft.position, targetL.position);
                    if (handleDist <= grabHandleDistande){
                        handlegrab = true;
                        newRot = targetL.rotation;
                        //handLeft.rotation = Quaternion.Slerp(handLeft.rotation, handle.pointsLeft.localRotation, grabSpeed * Time.fixedDeltaTime);
                    }
                }
            }
            if (handlegrab){
                handLeft.rotation = Quaternion.Slerp(handLeft.rotation, newRot, grabSpeed * Time.fixedDeltaTime);
            }
            else {
                handLeft.localRotation = Quaternion.Slerp(handLeft.localRotation, newRot, grabSpeed * Time.fixedDeltaTime);
            }
            
            /*
            if (handle != null){
                if (handle.pointsLeft != null){
                    Vector3 dirHandleL = (handle.pointsLeft.position - leftTargetPoint.position).normalized;
                    Quaternion rotHandleL = Quaternion.Slerp(leftTargetPoint.rotation, Quaternion.LookRotation(dirHandleL), Time.fixedDeltaTime * rotationSpeed);
                    leftTargetPoint.rotation = rotHandleL;
                }
            }
            */
        }

        if (armRight){
            Vector3 dirR = (targetR.position - armRight.position).normalized;
            Quaternion rotR = Quaternion.Slerp(armRight.rotation, Quaternion.LookRotation(dirR), Time.fixedDeltaTime * rotationSpeed);
            armRight.rotation = rotR;
            if (distL < armLenght){
                rightTargetPoint.position = targetR.position;
            }
            else {
                rightTargetPoint.localPosition = rightTargetPointStart;
            }
            
            bool handlegrab = false;
            Quaternion newRot = handRightDefaultRotation;
            if (handle != null){
                if (handle.pointsRight != null){
                    float handleDist = Vector3.Distance(handRight.position, targetR.position);
                    if (handleDist <= grabHandleDistande){
                        handlegrab = true;
                        newRot = targetR.rotation;
                        //handRot = Quaternion.Slerp(handRight.rotation, handle.pointsRight.localRotation, grabSpeed * Time.fixedDeltaTime);
                    }
                }
            }
            if (handlegrab){
                handRight.rotation = Quaternion.Slerp(handRight.rotation, newRot, grabSpeed * Time.fixedDeltaTime);
            }
            else {
                handRight.localRotation = Quaternion.Slerp(handRight.localRotation, newRot, grabSpeed * Time.fixedDeltaTime);
            }
        }
        //Vector3 dirL = (targetL.position - armLeft.position).normalized;
        //Vector3 dirR = (targetR.position - armRight.position).normalized;
        //Quaternion rotL = Quaternion.Slerp(armLeft.rotation, Quaternion.LookRotation(dirL), Time.deltaTime * rotationSpeed);
        //Quaternion rotR = Quaternion.Slerp(armRight.rotation, Quaternion.LookRotation(dirR), Time.deltaTime * rotationSpeed);
        //armLeft.rotation = rotL;
        //armRight.rotation = rotR;
    }

    void OnTriggerStay(Collider other)
    {
        if (this.enabled){
            CheckTriggerTransform(other.transform);
        }
    }

    void CheckTriggerTransform(Transform trigger){
        if (trigger.gameObject.activeInHierarchy){
            float importance = 0;
            switch (trigger.tag)
            {
                case "Player":
                    importance = 1;
                    break;
                default:
                    return;
            }

            if (target == null){
                target = trigger;
                targetImportance = importance;
            }
            //if target is closer and more important
            if (importance > targetImportance){
                target = trigger;
                targetImportance = importance;
            }
            else if (importance == targetImportance && Vector3.Distance(face.position, trigger.position) < Vector3.Distance(face.position, target.position)){
                target = trigger;
                targetImportance = importance;
            }
        }
    }

    //on exit, remove if it is the target
    void OnTriggerExit(Collider other)
    {
        if (other.transform == target){
            target = null;
            targetImportance = 0;
        }
    }
}
