using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class LookAtPlayer : MonoBehaviour
{
    public bool activated = true;
    public Transform faceController;
    public Transform target;
    public Transform[] defaultTargets;
    float targetImportance;
    public float rotationSpeed = 5f;
    public float defaultTime = 5;
    float defaultTimer;

    void Start()
    {
        target = null;
        targetImportance = 0;
        defaultTimer = 0;
    }

    void Update()
    {
        if (target != null){
            Vector3 dir = (target.position - faceController.position).normalized;
            Quaternion rot = Quaternion.Slerp(faceController.rotation, Quaternion.LookRotation(dir), Time.deltaTime * rotationSpeed / (targetImportance>0? 1 : 5));
            faceController.rotation = rot;
        }

        if (targetImportance <= 0){
            defaultTimer +=  Time.deltaTime;
        }
        else {
            defaultTimer = 0;
        }

        if (defaultTimer >= defaultTime && targetImportance <= 0){
            defaultTimer = 0;
            GetDefaultTarget();
        }
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
            else if (importance == targetImportance && Vector3.Distance(faceController.position, trigger.position) < Vector3.Distance(faceController.position, target.position)){
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
            GetDefaultTarget();
        }
    }

    Transform GetDefaultTarget(){
        targetImportance = 0;
        for (int i = 0; i < defaultTargets.Length; i++)
        {
            int r = Random.Range(0, defaultTargets.Length);
            target = defaultTargets[r];
        }
        return null;
    }
}
