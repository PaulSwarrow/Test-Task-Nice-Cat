using System;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Random = UnityEngine.Random;

namespace Physics
{
    [RequireComponent(typeof(Rigidbody))]
    public class Wings : MonoBehaviour
    {
        private const float AverageSpeed = 116f;
        [SerializeField] private Vector3 friction;
        [SerializeField] private Vector3 angularDamp;
        [SerializeField] private float liftForce;
        [SerializeField] private Vector3 flapsDrag;
        [SerializeField] private Vector3 flapsForce;
        [SerializeField] private Vector2 velocityAlignment;
        [SerializeField] private Vector3 lowSpeedNoise;
        private Rigidbody body;
        private Transform _transform;

        public float Roll;
        public float Yaw;
        public float Pitch;
        public float Lifting { get; private set; }

        private void Awake()
        {
            body = GetComponent<Rigidbody>();
            _transform = transform;
        }

        private void FixedUpdate()
        {
            var localVelocity = _transform.InverseTransformDirection(body.velocity);
            var gravity = UnityEngine.Physics.gravity.magnitude;

            //LIFT FORCE
            var effectiveVelocity = localVelocity.z;
            var up = _transform.up;
            Lifting = effectiveVelocity * liftForce;
            var liftingVector = up * Mathf.Min(effectiveVelocity * liftForce, gravity);

            //LINEAR DAMPING
            var damping = -_transform.TransformDirection(new Vector3(
                localVelocity.x * friction.x,
                localVelocity.y * friction.y,
                localVelocity.z * friction.z));

            //APPLY LINEAR FORCES
            body.AddForce(liftingVector + damping, ForceMode.Acceleration);

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
            //applied on high speed
            var hAlignAmount = localVelocity.magnitude / AverageSpeed;
            var hAngle = Vector3.SignedAngle(Vector3.forward, localVelocity, Vector3.up);
            var hAlign = Vector3.up * (Mathf.Clamp(hAngle, -90, 90) * velocityAlignment.x);
            hAlign *= Time.fixedDeltaTime * hAlignAmount;

            //applied on low speed
            var vAlignAmount = Mathf.Clamp(0.6f - Lifting / gravity, 0, 1);
            var vAngle = Vector3.SignedAngle(Vector3.forward, localVelocity, Vector3.right);
            var vAlign = Vector3.right * (Mathf.Clamp(vAngle, -90, 90) * velocityAlignment.y);
            vAlign *= Time.fixedDeltaTime * Mathf.Pow(vAlignAmount, 2);


            //NOISE:
            var noiseAmount = Mathf.Max(1 - Lifting / gravity, 0);
            var noise = new Vector3(
                            lowSpeedNoise.x * noiseAmount * Random.Range(-1f, 1f),
                            lowSpeedNoise.y * noiseAmount * Random.Range(-1f, 1f),
                            lowSpeedNoise.z * noiseAmount * Random.Range(-1f, 1f)) * Time.fixedDeltaTime;
            //APPLY TORQUE
            body.AddRelativeTorque(
                angularAcceleration - angularDamping + hAlign + vAlign + noise,
                ForceMode.Acceleration);
        }
    }
}