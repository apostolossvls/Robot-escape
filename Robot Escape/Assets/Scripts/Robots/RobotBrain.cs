using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RobotBrain : MonoBehaviour
{
    public List<Transform> sightObjects;
    public List<Transform> grabObjects;
    public Transform sightTarget;
    public Transform grabTarget;
    [Header("Rigibody")]
    public Rigidbody rig;
    [Header("Move")]
    public NavMeshAgent agent;
    public bool moving;
    public Vector3 movingTargetPosition;
    public float movingSpeed = 1f;
    float maxVelocityChange = 10f;
    public bool moveRandomly;
    public float moveRandomlyTime = 10f;
    float moveRandomlyTimer;
    [Header("Animation")]
    public Animator animator;
    void Start()
    {
        moving = false;
        moveRandomlyTimer = 0;

        Collider[] col = transform.GetChild(0).GetComponentsInChildren<Collider>() as Collider[];
        for (int i = 1; i < col.Length; i++)
        {
            Destroy(col[i]);
        }
    }

    void Update()
    {
        if (!moving && moveRandomly){
            moveRandomlyTimer += Time.deltaTime;
        }
        if (moveRandomlyTimer >= moveRandomlyTime){
            SetDestinationRandomly();
        }

        MovementUpdate();

        AnimationUpdateControl();
    }

    void FixedUpdate(){
        //MovementUpdate();
    }

    void SetDestination(Vector3 destination){
        print("set destination 'TODO'");
        agent.SetDestination(destination);
        moving = true;
        //transform.Translate(Vector3.forward * Time.deltaTime);
    }

    void SetDestinationRandomly(){
        moveRandomlyTimer = 0;
        Vector3 v = new Vector3(transform.position.x + Random.Range(-10f, 10f), transform.position.y, transform.position.z  + Random.Range(-10f, 10f));
        SetDestination(v);
    }

    Vector3 lastPos;
    float lastPosTimer;
    public bool movingForced = false;
    public float movingCheckDistance = 0.1f;
    public float lastPosTick = 0.2f;
    void MovementUpdate(){
        
        if (moving && agent.hasPath && agent.remainingDistance <= agent.stoppingDistance + 0.1f){
            moving = false;
            agent.ResetPath();
        }

        //moving 'experimental
        if (lastPosTimer >= lastPosTick){
            lastPosTimer = 0;
            lastPos = transform.position;
        }
        else lastPosTimer += Time.deltaTime;
        //moving 'experimental
        if (Vector3.Distance(lastPos, transform.position) >= movingCheckDistance){
            movingForced = true;
        }
        else movingForced = false;
        
        /*
        if (moving && movingTargetPosition != null && movingTargetPosition != transform.position){
            // Calculate how fast we should be moving
            var targetVelocity =(movingTargetPosition - transform.position).normalized;
            targetVelocity *= movingSpeed;
            //Apply a force that attempts to reach our target velocity
            var velocity = rig.velocity;
            var velocityChange = (targetVelocity - velocity);
            velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
            velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
            velocityChange.y = 0;
            rig.AddForce(velocityChange, ForceMode.VelocityChange);
            
            //rigidbody.AddForce(movingTargetPosition.normalized * Time.fixedDeltaTime);
            //transform.Translate(movingTargetPosition * Time.deltaTime);
            //transform.position = Vector3.Slerp(transform.position, transform.position - movingTargetPosition, Time.deltaTime);
        }
        if (moving && Vector3.Distance(transform.position, movingTargetPosition) <= 0.5f){
            moving = false;
            movingTargetPosition = Vector3.zero;
        }
        */
    }

    void AnimationUpdateControl(){
        if (animator == null) return;
        animator.SetBool("Forward", moving || movingForced);
    }
}
