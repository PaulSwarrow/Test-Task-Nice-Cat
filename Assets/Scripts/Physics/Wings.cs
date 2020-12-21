using System;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Physics
{
    [RequireComponent(typeof(Rigidbody))]
    public class Wings : MonoBehaviour
    {
        [SerializeField] private Vector3 friction;
        [SerializeField] private Vector3 angularDamp;
        [SerializeField] private float liftForce;
        [SerializeField] private Vector3 flapsDrag;
        [SerializeField] private Vector3 flapsForce;
        [SerializeField] private float velocityAlignmentDeadZone = 3f;
        [SerializeField] private float velocityAlignment;
        private Rigidbody body;
        private Transform _transform;

        public float Roll;
        public float Yaw;
        public float Pitch;

        private void Awake()
        {
            body = GetComponent<Rigidbody>();
            _transform = transform;
        }

        private void FixedUpdate()
        {
            var localVelocity = _transform.InverseTransformDirection(body.velocity);

            //LIFT FORCE
            var effectiveVelocity = localVelocity.z;
            var up = _transform.up;
            var lifting = up * Mathf.Min(
                              effectiveVelocity * liftForce,
                              UnityEngine.Physics.gravity.magnitude);

            //LINEAR DAMPING
            var damping = -_transform.TransformDirection(new Vector3(
                localVelocity.x * friction.x,
                localVelocity.y * friction.y,
                localVelocity.z * friction.z));

            //APPLY LINEAR FORCES
            body.AddForce(lifting + damping, ForceMode.Acceleration);

            //ANGULAR DAMPING
            var localAngularVelocity = transform.InverseTransformDirection(body.angularVelocity);
            var angularDamping = new Vector3(
                localAngularVelocity.x * (angularDamp.x + (1 - Pitch) * flapsDrag.x),
                localAngularVelocity.y * (angularDamp.y + (1 - Yaw) * flapsDrag.y),
                localAngularVelocity.z * (angularDamp.z + (1 - Roll) * flapsDrag.z)
            );

            //ANGULAR ACCELERATION:
            var angularAcceleration = new Vector3(
                Pitch * flapsForce.x,
                Yaw * flapsForce.y,
                Roll * flapsForce.z
            );

            //ALIGN TO VELOCITY VECTOR
            var vectorDelta = Vector3.SignedAngle(Vector3.forward, localVelocity, Vector3.up);
            var align = Vector3.up * (Mathf.Clamp(vectorDelta, -90, 90) * velocityAlignment);
            align *= Time.fixedDeltaTime;

            //APPLY TORQUE
            body.AddRelativeTorque(angularAcceleration - angularDamping + align, ForceMode.Acceleration);

        }
    }
}