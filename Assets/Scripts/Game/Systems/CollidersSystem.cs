using System.Collections.Generic;
using Physics;
using UnityEngine;

namespace DefaultNamespace
{
    public class CollidersSystem : IGameSystem
    {
        public void Init(GameManager.Properties properties)
        {
        }

        public void Start()
        {
        }

        public void Update()
        {
        }

        public void Stop()
        {
        }

        private HashSet<VirtualCollider> colliders = new HashSet<VirtualCollider>();

        public bool Raycast(Vector3 origin, Vector3 direction, out HitInfo hit)
        {
            var ray = new Ray(origin, direction);
            foreach (var collider in colliders)
            {
                if (collider.HitTest(ray, out var distance))
                {
                    hit = new HitInfo
                    {
                        collider = collider,
                        point = origin + direction * distance
                    };
                    return true;
                }
            }

            hit = default;
            return false;
        }

        public void RegisterCollider(VirtualCollider collider)
        {
            colliders.Add(collider);
        }

        public void UnregisterCollider(VirtualCollider collider)
        {
            colliders.Remove(collider);
        }
    }
}