using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class Projectile: MonoBehaviour
    {
        [SerializeField] private float lifeTime = 2;
        [SerializeField] public float velocity = 70;

        private float timeLeft = 0;
        private Transform _transform;


        private void Awake()
        {
            _transform = transform;
        }

        public void Spawn(Vector3 position, Vector3 direction)
        {
            _transform.position = position;
            _transform.forward = direction;
            gameObject.SetActive(true);
            timeLeft = lifeTime;
        }


        public void Deactivate()
        {
            gameObject.SetActive(false);
        }
        
        public bool Work()
        {
            if (timeLeft < 0)
            {
                return false;

            }

            timeLeft -= Time.deltaTime;

            var step = _transform.forward * (velocity * Time.deltaTime);
            // continue;
            if (GameManager.instance.colliders.Raycast(_transform.position, step, out var hit))
            {
                //spawn fx
                //dispatch
                if (hit.collider.TryGetComponent<PlaneEntity>(out var target))
                {
                    target.ReceiveHit();
                }

                return false;
            }

            _transform.position += step;
            return true;
        }
    }
}