using UnityEngine;

namespace ChaosCar
{
    [RequireComponent(typeof(Rigidbody))]
    public class PoliceChaser : MonoBehaviour
    {
        public Transform target;
        public float acceleration = 900f;
        public float maxSpeed = 18f;
        public float turnSpeed = 3f;
        public float bumpDamage = 8f;

        private Rigidbody rb;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            if (target == null) return;

            Vector3 toTarget = target.position - transform.position;
            toTarget.y = 0f;
            if (toTarget.sqrMagnitude < 0.1f) return;

            Quaternion desiredRotation = Quaternion.LookRotation(toTarget);
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, desiredRotation, turnSpeed * Time.fixedDeltaTime));

            Vector3 force = transform.forward * acceleration;
            rb.AddForce(force, ForceMode.Force);
            rb.linearVelocity = Vector3.ClampMagnitude(rb.linearVelocity, maxSpeed);
        }

        private void OnCollisionEnter(Collision collision)
        {
            ArcadeCarController car = collision.collider.GetComponentInParent<ArcadeCarController>();
            if (car != null)
                car.TakeDamage(bumpDamage);
        }
    }
}
