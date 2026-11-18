using UnityEngine;

namespace ChaosCar
{
    [RequireComponent(typeof(Rigidbody))]
    public class ArcadeCarController : MonoBehaviour
    {
        [Header("Movimiento")]
        public float motorForce = 1800f;
        public float brakeForce = 2600f;
        public float maxSteerAngle = 32f;
        public float nitroMultiplier = 1.75f;

        [Header("Ruedas")]
        public WheelCollider frontLeft;
        public WheelCollider frontRight;
        public WheelCollider rearLeft;
        public WheelCollider rearRight;
        public Transform frontLeftMesh;
        public Transform frontRightMesh;
        public Transform rearLeftMesh;
        public Transform rearRightMesh;

        [Header("Estado")]
        public float health = 100f;
        public float nitro = 100f;
        public int wantedLevel;

        private Rigidbody rb;
        private Vector3 spawnPosition;
        private Quaternion spawnRotation;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            spawnPosition = transform.position;
            spawnRotation = transform.rotation;
            rb.centerOfMass = new Vector3(0f, -0.45f, 0f);
        }

        private void FixedUpdate()
        {
            float throttle = Input.GetAxis("Vertical");
            float steer = Input.GetAxis("Horizontal");
            bool useNitro = Input.GetKey(KeyCode.LeftShift) && nitro > 0f;
            float currentMotor = motorForce * (useNitro ? nitroMultiplier : 1f);

            if (useNitro)
                nitro = Mathf.Max(0f, nitro - 25f * Time.fixedDeltaTime);
            else
                nitro = Mathf.Min(100f, nitro + 8f * Time.fixedDeltaTime);

            frontLeft.steerAngle = maxSteerAngle * steer;
            frontRight.steerAngle = maxSteerAngle * steer;

            rearLeft.motorTorque = currentMotor * throttle;
            rearRight.motorTorque = currentMotor * throttle;

            float braking = Input.GetKey(KeyCode.Space) ? brakeForce : 0f;
            frontLeft.brakeTorque = braking;
            frontRight.brakeTorque = braking;
            rearLeft.brakeTorque = braking;
            rearRight.brakeTorque = braking;

            UpdateWheel(frontLeft, frontLeftMesh);
            UpdateWheel(frontRight, frontRightMesh);
            UpdateWheel(rearLeft, rearLeftMesh);
            UpdateWheel(rearRight, rearRightMesh);

            if (transform.position.y < -20f)
                ResetCar();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
                ResetCar();

            health = Mathf.Clamp(health, 0f, 100f);
            if (health <= 0f)
                ResetCar();
        }

        private static void UpdateWheel(WheelCollider collider, Transform wheelMesh)
        {
            if (wheelMesh == null) return;
            collider.GetWorldPose(out Vector3 position, out Quaternion rotation);
            wheelMesh.position = position;
            wheelMesh.rotation = rotation;
        }

        public void TakeDamage(float amount)
        {
            health -= amount;
        }

        public void ResetCar()
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            transform.SetPositionAndRotation(spawnPosition, spawnRotation);
            health = 100f;
            nitro = 100f;
        }
    }
}
