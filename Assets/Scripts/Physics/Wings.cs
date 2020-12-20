using System;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Physics
{
    [RequireComponent(typeof(PhysicBody))]
    public class Wings : MonoBehaviour
    {
        [SerializeField] private float liftForce;
        [SerializeField] private float velocityAlignmentDeadZone = 3f;
        [SerializeField] private float velocityAlignment;
        private PhysicBody body;
        private Transform _transform;

        private void Awake()
        {
            body = GetComponent<PhysicBody>();
            _transform = transform;
        }

        private void FixedUpdate()
        {
            var velocity = _transform.InverseTransformVector(body.AbsoluteVelocity);
            var effectiveVelocity = velocity.z;
            var up = _transform.up;
            var acceleration = up *
                               Mathf.Min(effectiveVelocity * liftForce, UnityEngine.Physics.gravity.magnitude);

            body.Accelerate(acceleration);

            //some hardcore behaviour for better control
            if (body.AbsoluteVelocity.magnitude > 0)
            {
                _transform.rotation = Quaternion.Slerp(
                    _transform.rotation,
                    Quaternion.LookRotation(body.AbsoluteVelocity, _transform.up),
                    velocityAlignment * Time.fixedDeltaTime);
            }
        }
    }
}