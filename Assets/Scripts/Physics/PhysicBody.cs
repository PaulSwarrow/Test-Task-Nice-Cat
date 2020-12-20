using System;
using UnityEngine;

namespace Physics
{
    public class PhysicBody : MonoBehaviour
    {
        [SerializeField] private Vector3 friction;
        [SerializeField] private float angularDrag;
        [SerializeField] private Vector3 velocity = Vector3.zero;
        
        private Quaternion angularVelocty = Quaternion.identity;
        private Transform _transform;
        public Vector3 AbsoluteVelocity { get; private set; }

        private void Awake()
        {
            _transform = transform;
        }

        public void Accelerate(Vector3 vector)
        {
            velocity += vector * Time.fixedDeltaTime;
        }
        
        private void FixedUpdate()
        {
            var position = _transform.position;
            velocity += UnityEngine.Physics.gravity * Time.fixedDeltaTime;
            //apply friction by axis
            var localVelocity = _transform.InverseTransformDirection(velocity);
            localVelocity -= new Vector3(
                localVelocity.x * friction.x,
                localVelocity.y * friction.y,
                localVelocity.z * friction.z);

            velocity = _transform.TransformDirection(localVelocity);

            //hardcode floor:
            velocity.y = Mathf.Max(velocity.y, -position.y);

            AbsoluteVelocity = velocity/Time.fixedDeltaTime;// write for external usage
            _transform.position = position + velocity;
            angularVelocty = Quaternion.Slerp(angularVelocty, Quaternion.identity, angularDrag * Time.fixedDeltaTime);
            transform.localRotation *= angularVelocty;
        }

        public void AccelerateRotation(Vector3 euler)
        {
            angularVelocty *= Quaternion.Euler(euler * Time.fixedDeltaTime);
        }

        public void SetVelocity(Vector3 vector)
        {
            velocity = vector * Time.fixedDeltaTime;
        }
    }
}