﻿using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace DefaultNamespace
{
    public class PlayerController : IGameSystem
    {
        private PlaneEntity plane;
        private GameManager.Properties properties;
        public Vector3 AimPoint => plane.transform.position + plane.transform.forward * 100;
        public float Altitude => plane.transform.position.y;
        public PlaneEntity Target => plane;

        public void Init(GameManager.Properties properties)
        {
            this.properties = properties;
        }

        public void Start()
        {
            plane = Object.Instantiate(properties.planePrefab, properties.bounds.center, Quaternion.identity);
            properties.camera.target = plane.transform;
            
        }

        public void Update()
        {
            plane.Pitch = Input.GetAxis("Vertical");
            plane.Roll = -Input.GetAxis("Horizontal");
            plane.Yaw = -Input.GetAxis("Yaw");
            plane.Fire = Input.GetButton("Fire1");

            if (Input.GetButton("Run"))
            {
                plane.force = Mathf.Min(plane.force + 0.3f * Time.deltaTime, 1);
            }else if (Input.GetButton("Walk"))
            {
                
                plane.force = Mathf.Max(plane.force - 0.3f * Time.deltaTime, 0);
            }
        }

        public void Stop()
        {
            throw new System.NotImplementedException();
        }
    }
}