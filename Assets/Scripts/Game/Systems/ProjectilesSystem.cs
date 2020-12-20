using System.Collections.Generic;
using Tools;
using UnityEngine;

namespace DefaultNamespace
{
    public class ProjectilesSystem : IGameSystem
    {
        private Projectile prefab;

        private List<Projectile> pool = new List<Projectile>();
        private List<Projectile> projectiles = new List<Projectile>();
        public float Velocity => prefab.velocity;

        public void Init(GameManager.Properties properties)
        {
            prefab = properties.bulletPrefab;
        }

        public void Spawn(Vector3 position, Vector3 direction)
        {
            var bullet = Create();
            bullet.Spawn(position, direction);
            projectiles.Add(bullet);
        }


        public void Start()
        {
        }

        public void Update()
        {
            for (var i = projectiles.Count - 1; i >= 0; i--)
            {
                var projectile = projectiles[i];
                if (!projectile.Work())
                {
                    Remove(projectile);
                }
            }
        }


        private Projectile Create()
        {
            return pool.Count > 0 ? pool.Shift() : Object.Instantiate(prefab);
        }

        private void Remove(Projectile projectile)
        {
            projectile.Deactivate();
            projectiles.Remove(projectile);
            pool.Add(projectile);
        }

        public void Stop()
        {
        }
    }
}