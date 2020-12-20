using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

namespace DefaultNamespace
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance { get; private set; }
        [Serializable]
        public class Properties
        {
            public PlaneEntity planePrefab;
            public Projectile bulletPrefab;
            public FollowCamera camera;
            public int npcCount = 5;
            public Bounds bounds;
        }

        [SerializeField] private Properties properties;

        private List<IGameSystem> systems = new List<IGameSystem>();
        public PlayerController playerController{ get; private set; }
        public AiSystem aiSystem{ get; private set; }
        public ProjectilesSystem projectiles { get; private set; }
        public CollidersSystem colliders { get; private set; }

        private void Awake()
        {
            instance = this;
            
            playerController = CreateSystem<PlayerController>();
            aiSystem = CreateSystem<AiSystem>();
            projectiles = CreateSystem<ProjectilesSystem>();
            colliders = CreateSystem<CollidersSystem>();

            foreach (var gameSystem in systems)
            {
                gameSystem.Init(properties);
            }
            foreach (var gameSystem in systems)
            {
                gameSystem.Start();
            }

        }

        private void Update()
        {
            foreach (var gameSystem in systems)
            {
                gameSystem.Update();
            }
        }

        private T CreateSystem<T>() where T : IGameSystem, new()
        {
            var system = new T();
            system.Init(properties);
            systems.Add(system);
            return system;
        }
    }
}