using System;
using Physics;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace DefaultNamespace
{
    public class PlaneEntity : MonoBehaviour
    {
        [Range(0, 1)] public float force;
        public float maxForce = 20;
        [SerializeField] [Range(0, 1)] private float maneuverability;
        [SerializeField] private float gunCooldownTime = .2f;
        [SerializeField] public float Yaw;
        [SerializeField] private GameObject deathParticles;
        public float Roll;
        public float Pitch;
        public bool Fire;
        public bool Dead;


        public PhysicBody Body { get; private set; }
        public Wings Wings { get; private set; }
        public Transform transform { get; private set; }
        private float gunCooldown = 0;

        private void Awake()
        {
            transform = base.transform;
            Body = GetComponent<PhysicBody>();
            Wings = GetComponent<Wings>();
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

            Wings.Yaw = Yaw;
            Wings.Pitch = Pitch;
            Wings.Roll = Roll;
        }

        private void Shoot()
        {
            GameManager.instance.projectiles.Spawn(
                ((Component) this).transform.position + ((Component) this).transform.forward * 10,
                ((Component) this).transform.forward);
        }

        private void FixedUpdate()
        {
            Body.Accelerate(transform.forward * (force * maxForce));
        }

        public void ReceiveHit()
        {
            if (Dead) return;
            Dead = true;
            deathParticles.SetActive(true);
            force = 0;
        }
    }
}