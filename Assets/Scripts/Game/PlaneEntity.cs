using System;
using Physics;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace DefaultNamespace
{
    public class PlaneEntity : MonoBehaviour
    {
        [SerializeField] private float force;
        [SerializeField] [Range(0, 1)] private float maneuverability;
        [SerializeField] private float gunCooldownTime = .2f;
        [SerializeField] private float yaw;
        [SerializeField] private GameObject deathParticles;
        public float Roll;
        public float Pitch;
        public bool Fire;
        public bool Dead;


        public PhysicBody Body { get; private set; }
        public Transform transform { get; private set; }
        private float gunCooldown = 0;

        private void Awake()
        {
            transform = base.transform;
            Body = GetComponent<PhysicBody>();
            Body.SetVelocity(((Component) this).transform.forward * 20); //hardcode startup
            deathParticles.SetActive(false);
        }

        private void Update()
        {
            if (gunCooldown <= 0)
            {
                if (Fire)
                {
                    gunCooldown = gunCooldownTime;
                    Shoot();
                }
            }
            else
            {
                gunCooldown -= Time.deltaTime;
            }

        }

        private void Shoot()
        {
            GameManager.instance.projectiles.Spawn(
                ((Component) this).transform.position + ((Component) this).transform.forward * 10,
                ((Component) this).transform.forward);
        }

        private void FixedUpdate()
        {
            Body.Accelerate(transform.forward * force);

            var euler = new Vector3(Pitch, yaw, Roll) * (maneuverability);
            Body.AccelerateRotation(euler);
        }

        public void ReceiveHit()
        {
            if(Dead) return;
            Dead = true;
            deathParticles.SetActive(true);
            force = 0;
        }
    }
}