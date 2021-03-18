using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotBrain : MonoBehaviour
{
    public List<Transform> sightObjects;
    public List<Transform> grabObjects;
    public Transform sightTarget;
    public Transform grabTarget;
    [Header("Rigibody")]
    public Rigidbody rigidbody;
    [Header("Move")]
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

        AnimationUpdateControl();
    }

    void FixedUpdate(){
        MovementUpdate();
    }

    void SetDestination(Transform destination){
        print("set destination 'TODO'");
        transform.Translate(Vector3.forward * Time.deltaTime);
    }

    void SetDestinationRandomly(){
        moveRandomlyTimer = 0;
        Transform destination = null;
        SetDestination(destination);
    }

    void MovementUpdate(){
        if (moving && movingTargetPosition != null && movingTargetPosition != Vector3.zero){
            // Calculate how fast we should be moving
            var targetVelocity =(movingTargetPosition - transform.position).normalized;
            targetVelocity *= movingSpeed;
            // Apply a force that attempts to reach our target velocity
            var velocity = rigidbody.velocity;
            var velocityChange = (targetVelocity - velocity);
            velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
            velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
            velocityChange.y = 0;
            rigidbody.AddForce(velocityChange, ForceMode.VelocityChange);
            //rigidbody.AddForce(movingTargetPosition.normalized * Time.fixedDeltaTime);
            //transform.Translate(movingTargetPosition * Time.deltaTime);
            //transform.position = Vector3.Slerp(transform.position, transform.position - movingTargetPosition, Time.deltaTime);
        }
        if (moving && Vector3.Distance(transform.position, movingTargetPosition) <= 0.5f){
            moving = false;
            movingTargetPosition = Vector3.zero;
        }
    }

    void AnimationUpdateControl(){
        if (animator == null) return;
        animator.SetBool("Forward", moving);
    }
}
