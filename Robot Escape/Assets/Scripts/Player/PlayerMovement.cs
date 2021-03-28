using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rig;
    public float moveSpeed = 5f;
    public bool canMove = true;
    public float rotationSpeed = 5f;
    public float maxVelocityChange = 10.0f;
    public Transform cam;
    public Transform head;
    public Transform feet;
    //public Collider col;
    public LayerMask layerMask;
    //public Animator animator;
    Vector3 movement;

    void Start()
    {
        if (rig==null) rig = GetComponentInChildren<Rigidbody>(); 

        canMove = true;
    }

    void Update()
    {
        //get movement direction
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.z = Input.GetAxisRaw("Vertical");
        movement = movement.normalized;

        /*
        if (rig.velocity != Vector3.zero){
            Vector3 dir = rig.velocity;
            dir.y = 0;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * rotationSpeed * dir.magnitude);
        }
        */
    }

    void FixedUpdate(){
        //get movement direction
        if (rig.velocity != Vector3.zero){
            Vector3 dir = rig.velocity;
            dir.y = 0;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * rotationSpeed * dir.magnitude);
        }
        //position
        if (movement != Vector3.zero && canMove)
        {
            //get camera looking direction
            Vector3 targetDirection = new Vector3(movement.x, 0f, movement.z);
            targetDirection = cam.transform.TransformDirection(targetDirection);
            targetDirection.y = 0.0f;
            //Debug.Log("Angle: " + (Vector3.Angle(transform.rotation.eulerAngles, targetDirection)));
            //Debug.DrawRay(transform.position, forward.normalized*10f, Color.black);


            //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(targetDirection), Time.deltaTime * rotationSpeed);
            
            
            //rig.MovePosition(rig.position + new Vector3(forward.x * movement.x, 0, forward.z * movement.z) * moveSpeed * Time.fixedDeltaTime);

            //RaycastHit hit;
            //CharacterController charContr = GetComponent<CharacterController>();
            //Vector3 p1 = transform.position + collider.bounds.center + Vector3.up * -collider.bounds.extents.y * 0.5F;
            //p1 = collider.transform.localScale;
            //Vector3 p2 = p1 + Vector3.up * charContr.height;

            // Cast character controller shape 10 meters forward to see if it is about to hit anything.
            //if (Physics.cast(p1, p2, charContr.radius, targetDirection.normalized, out hit, targetDirection.sqrMagnitude * moveSpeed * Time.deltaTime)){
            //    if (!hit.collider.isTrigger){
            //        return;
            //    }
            //}

            //RaycastHit hit;
            //if (Physics.Raycast(transform.position, targetDirection, out hit, targetDirection.magnitude+0.05f, layerMask)){
            //    return;
            //}
            //Debug

            // Calculate how fast we should be moving
            Vector3 targetVelocity = targetDirection;
            targetVelocity *= moveSpeed;
    
            // Apply a force that attempts to reach our target velocity
            Vector3 velocity = rig.velocity;
            Vector3 velocityChange = (targetVelocity - velocity);
            velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
            velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
            velocityChange.y = 0;
            rig.AddForce(velocityChange, ForceMode.VelocityChange);

            //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(targetDirection), Time.deltaTime * rotationSpeed);




            //rig.MovePosition(rig.position + targetDirection * moveSpeed * Time.fixedDeltaTime);
            Debug.DrawRay(transform.position, new Vector3(cam.forward.x, 0, cam.forward.z).normalized * 10f, Color.blue);
        }
        //Debug.DrawRay(new Vector3(transform.position.x, transform.position.y-distToGround, transform.position.z), -transform.up, Color.red);
    }
}
