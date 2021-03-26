using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshCirclePath : MonoBehaviour
{
    [Header("Agent and Targets")]
    public NavMeshAgent agent;
    public List<Transform> targets;
    public float waitingTimeMin = 2f;
    public float waitingTimeMax = 15f;
    [Header("Moving to target")]
    public bool moveCicleTarget = true; //select targets in order if true 
    public bool moveRandomTarget = false; //select random targets if true
    public bool moveTotalRandom = false; //select random targets if true
    public float totalRandomMaxDistance = 20f;
    [Header("Other")]
    public bool moveCicleFromStart = true; //start moving instantly if true
    [Header("Animator")]
    public Animator animator;
    public string movingAnimatorBoolLabel;
    Transform nowTarget;
    [HideInInspector]
    public bool moving; //state
    bool lastMoving; //lastf frame state
    float waitingTimer, waitingTime; //counter-timer in seconds, time goal
    

    void Start()
    {
        moving = false;
        //move to target instantly from the first update call...
        //if moveCicleFromStart is true
        waitingTimer = moveCicleFromStart? waitingTimeMax+1 : 0f;
        //set new waiting time goal;
        SetWaitingTimeRandom();
    }

    void Update()
    {
        //if value change on moving is detected, call animator changes
        if (moving != lastMoving) AnimatorChange();
        lastMoving = moving;

        //assend waiting seconds if not moving
        if (!moving && (moveCicleTarget || moveRandomTarget || moveTotalRandom)){
            waitingTimer += Time.deltaTime;
        }
        //set destination if waiting equal or more that wanting time
        if (waitingTimer >= waitingTime){
            waitingTimer = 0;
            Move();
        }

        //stop and set values if close to target
        if (moving && agent.hasPath && agent.remainingDistance <= agent.stoppingDistance + 0.1f){
            moving = false;
            //set new waiting time goal;
            SetWaitingTimeRandom();
            agent.ResetPath();
        }
    }

    void Move(){
        //set new random destination if true
        if (moveRandomTarget) SetDestinationRandom();
        //set next destination if true
        else if (moveCicleTarget) SetDestinationCicle();
        //set a total random destination if true
        else if (moveTotalRandom) SetDestinationTotalRandom();
    }

    //set agent's destination
    void SetDestination(Vector3 destination){
        agent.SetDestination(destination);
        moving = true;
    }

    void SetDestinationRandom(){
        if (targets.Count == 0) return;

        //set new target
        //get random index
        int index = Random.Range(0, targets.Count);
        if (targets[index] == null) return;
        //get target by index
        nowTarget = targets[index];

        SetDestination(nowTarget.position);
    }

    void SetDestinationCicle(){
        if (targets.Count == 0) return;

        //set new target
        //get nowTarget index and add 1
        int index = GetTargetIndex() + 1;
        //if index out of max bound
        if (index >= targets.Count) index = 0;
        if (targets[index] == null) return;
        //get target by index
        nowTarget = targets[index];

        SetDestination(nowTarget.position);
    }

    void SetDestinationTotalRandom(){
        //get random x,z
        float randomX = Random.Range(-totalRandomMaxDistance, totalRandomMaxDistance);
        float randomZ = Random.Range(-totalRandomMaxDistance, totalRandomMaxDistance);
        //instantiate destination
        Vector3 destination = new Vector3(randomX, transform.position.y, randomZ);
        SetDestination(destination);
    }

    //set waitingTime to a new random value between min and max values;
    void SetWaitingTimeRandom(){
        if (waitingTimeMin <= 0) waitingTimeMin = 0.1f;
        if (waitingTimeMax < waitingTimeMin) waitingTimeMax = waitingTimeMin + 0.1f;
        waitingTime = Random.Range(waitingTimeMin, waitingTimeMax);
    }

    //get nowTarget's index on targets list
    int GetTargetIndex(){
        int index = -1;
        for (int i = 0; i < targets.Count; i++)
        {
            if (targets[i] == nowTarget){
                index = i;
                break;
            }
        }
        return index;
    }

    //update animator values
    void AnimatorChange(){
        //if animator and animator label is not null -> return
        if (animator == null || movingAnimatorBoolLabel == null || movingAnimatorBoolLabel == "")
            return;
        //set moving bool with string name 'movingAnimatorBoolLabel' to moving value
        animator.SetBool(movingAnimatorBoolLabel, moving);
    }
}
