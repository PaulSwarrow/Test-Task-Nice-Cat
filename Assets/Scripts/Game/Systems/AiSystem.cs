using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DefaultNamespace
{
    public class AiSystem : IGameSystem
    {
        public List<PlaneNpc> list = new List<PlaneNpc>();
        private GameManager.Properties properties;
        private PlayerController playerController;

        public void Init(GameManager.Properties properties)
        {
            this.properties = properties;
        }

        public void Start()
        {
            playerController = GameManager.instance.playerController;
            for (var i = 0; i < properties.npcCount; i++)
            {
                list.Add(CreateNPC(properties.planePrefab));
            }
        }

        public void Update()
        {
            list.ForEach(UpdateNpc);
        }

        public void Stop()
        {
        }

        private PlaneNpc CreateNPC(PlaneEntity prefab)
        {
            var rotation = Quaternion.Euler(Vector3.up * Random.Range(0, 360));
            var npc = new PlaneNpc
            {
                plane = Object.Instantiate(prefab, GetRandomPosition(), rotation),
                followTarget = GetRandomPosition()
            };
            return npc;
        }

        private void UpdateNpc(PlaneNpc npc)
        {
            if (npc.plane.Dead) return;
            //behaviour
            var targetDistance =
                Vector3.Distance(npc.plane.transform.position, playerController.Target.transform.position);
            switch (npc.task)
            {
                case NpcTask.idle:
                    npc.attackCooldown -= Time.deltaTime;
                    if (Vector3.Distance(npc.followTarget, npc.plane.transform.position) < 20)
                    {
                        npc.followTarget = GetRandomPosition();
                    }
                    else if (targetDistance < 400 && npc.attackCooldown <= 0)
                    {
                        npc.attackTime = 0;
                        npc.task = NpcTask.attack;
                    }

                    break;
                case NpcTask.attack:
                    npc.attackTime += Time.deltaTime;
                    if (npc.attackTime > 2 || targetDistance < 30)
                    {
                        npc.attackCooldown = 5;
                        npc.task = NpcTask.idle;
                        npc.plane.Fire = false;
                    }
                    else
                    {
                        npc.followTarget = Aim(
                            npc.plane.transform.position,
                            GameManager.instance.projectiles.Velocity,
                            GameManager.instance.playerController.Target);

                        npc.plane.Fire = targetDistance < 200;
                    }

                    break;
            }


            //some hardcode ai input
            var aimVector = npc.followTarget - npc.plane.transform.position;
            var yawDelta = Vector3.SignedAngle(npc.plane.transform.forward, aimVector, Vector3.up);

            npc.plane.transform.rotation = Quaternion.Slerp(npc.plane.transform.rotation,
                Quaternion.LookRotation(
                    aimVector,
                    Vector3.up + npc.plane.transform.right * Mathf.Clamp(yawDelta, -90, 90) / 8),
                1 * Time.deltaTime);
        }


        private Vector3 GetRandomPosition()
        {
            return new Vector3(
                Random.Range(properties.bounds.min.x, properties.bounds.max.x),
                Random.Range(properties.bounds.min.y, properties.bounds.max.y),
                Random.Range(properties.bounds.min.z, properties.bounds.max.z));
        }

        private Vector3 Aim(Vector3 position, float projectileVelocity, PlaneEntity target)
        {
            var targetPos = target.transform.position;
            var targetV = target.Body.AbsoluteVelocity;

            var t = (targetPos - position).magnitude / (projectileVelocity - targetV.magnitude);

            var aimPoint = targetPos + t * targetV;

            return aimPoint;
        }
    }
}