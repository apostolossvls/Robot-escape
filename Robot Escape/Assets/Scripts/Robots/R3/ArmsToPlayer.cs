using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmsToPlayer : MonoBehaviour
{
    public bool activated = true;
    public Transform target;
    public float targetImportance;
    //public LookAtPlayer lookAtPlayer;
    public Transform armLeft, armRight;
    public Transform leftTargetPoint, rightTargetPoint; 
    public Transform armLeftDefaultTarget, armRightDefaultTarget;
    public float rotationSpeed = 5f;
    public bool grab;
    float armLenght;
    Vector3 leftTargetPointStart, rightTargetPointStart;

    void Start(){
        armLenght = Vector3.Distance(armLeft.position, leftTargetPoint.position);
        if (leftTargetPoint) leftTargetPointStart = leftTargetPoint.localPosition;
        if (rightTargetPoint) rightTargetPointStart = rightTargetPoint.localPosition;
    }   

    void FixedUpdate(){
        //if (lookAtPlayer == null) return;

        Transform targetL;
        Transform targetR;
        if (target != null && grab && targetImportance>0){
            targetL = target;
            targetR = target;
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
            else if (importance == targetImportance && Vector3.Distance(transform.position, trigger.position) < Vector3.Distance(transform.position, target.position)){
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
