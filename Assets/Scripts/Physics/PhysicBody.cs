using System;
using UnityEngine;

namespace Physics
{
    [RequireComponent(typeof(Rigidbody))]
    public class PhysicBody : MonoBehaviour
    {
        [SerializeField] private float angularDrag;
        
        private Quaternion angularVelocty = Quaternion.identity;
        private Transform _transform;
        private Rigidbody _body;
        public Vector3 AbsoluteVelocity => _body.velocity;

        private void Awake()
        {
            _body = GetComponent<Rigidbody>();
            _transform = transform;
        }

        public void Accelerate(Vector3 vector)
        {
            _body.AddForce(vector, ForceMode.Acceleration);
        }
        
        private void FixedUpdate()
        {
            var position = _transform.position;
            //apply friction by axis
            


            // angularVelocty = Quaternion.Slerp(angularVelocty, Quaternion.identity, angularDrag * Time.fixedDeltaTime);
            // transform.localRotation *= angularVelocty;
        }

        public void AccelerateRotation(Vector3 euler)
        {
            // angularVelocty *= Quaternion.Euler(euler * Time.fixedDeltaTime);
            _body.AddRelativeTorque(euler);
        }

        public void SetVelocity(Vector3 vector)
        {
            _body.velocity = vector;
        }
    }
}