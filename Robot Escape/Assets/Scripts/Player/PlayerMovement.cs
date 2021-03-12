using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rig;
    public float moveSpeed = 5f;
    public bool canMove = true;
    public float rotationSpeed = 5f;
    public Transform cam;
    public Transform head;
    public Transform feet;
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
    }

    void FixedUpdate(){
        //position
        if (movement != Vector3.zero && canMove)
        {
            //get camera looking direction
            Vector3 targetDirection = new Vector3(movement.x, 0f, movement.z);
            targetDirection = cam.transform.TransformDirection(targetDirection);
            targetDirection.y = 0.0f;
            //Debug.Log("Angle: " + (Vector3.Angle(transform.rotation.eulerAngles, targetDirection)));
            //Debug.DrawRay(transform.position, forward.normalized*10f, Color.black);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(targetDirection), Time.deltaTime * rotationSpeed);
            //rig.MovePosition(rig.position + new Vector3(forward.x * movement.x, 0, forward.z * movement.z) * moveSpeed * Time.fixedDeltaTime);
            rig.MovePosition(rig.position + targetDirection * moveSpeed * Time.fixedDeltaTime);
            Debug.DrawRay(transform.position, new Vector3(cam.forward.x, 0, cam.forward.z).normalized * 10f, Color.blue);
        }
        //Debug.DrawRay(new Vector3(transform.position.x, transform.position.y-distToGround, transform.position.z), -transform.up, Color.red);
    }
}
