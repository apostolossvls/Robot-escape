using UnityEngine;

namespace DitzelGames.FastIK
{
    public class FastIKLook : MonoBehaviour
    {
        /// <summary>
        /// Look at target
        /// </summary>
        public Transform Target;

        /// <summary>
        /// Initial direction
        /// </summary>
        protected Vector3 StartDirection;

        /// <summary>
        /// Initial Rotation
        /// </summary>
        protected Quaternion StartRotation;
        //public bool rotationOnY;
        public Rigidbody rigidbody;

        void Awake()
        {
            if (Target == null)
                return;

            StartDirection = Target.position - transform.position;
            StartRotation = transform.rotation;
            
            rigidbody = GetComponent<Rigidbody>();
        }

        void FixedUpdate()
        {
            if (Target == null)
                return;
            
            //if (rotationOnY){
            //   Vector3 v = Target.position - transform.position;
            //    Quaternion q = Quaternion.FromToRotation(StartDirection, v) * StartRotation;
            //    q.x = 0;
            //    transform.rotation = q;
            //}
            //else{
                Vector3 x = Vector3.Cross(transform.position.normalized, Target.position.normalized);
                float theta = Mathf.Asin(x.magnitude);
                Debug.Log(theta);
                Vector3 w = x.normalized * theta / Time.fixedDeltaTime;

                Quaternion q = transform.rotation * rigidbody.inertiaTensorRotation;
                Vector3 T = q * Vector3.Scale(rigidbody.inertiaTensor, (Quaternion.Inverse(q) * w));
                rigidbody.AddTorque(T, ForceMode.Impulse);

                //transform.rotation = Quaternion.FromToRotation(StartDirection, Target.position - transform.position) * StartRotation;
            //}
        }
        /*
        void Update()
        {
            if (Target == null)
                return;
            
            if (rotationOnY){
                Vector3 v = Target.position - transform.position;
                Quaternion q = Quaternion.FromToRotation(StartDirection, v) * StartRotation;
                q.x = 0;
                transform.rotation = q;
            }
            else{
                transform.rotation = Quaternion.FromToRotation(StartDirection, Target.position - transform.position) * StartRotation;
            }
        }
        */
    }
}
