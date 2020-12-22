﻿using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class PropellerView : MonoBehaviour
    {
        [SerializeField] private Transform propellerRoot;
        [SerializeField] private MeshRenderer propellerRenderer;
        [SerializeField] private SpriteRenderer fx;
        [SerializeField] private float velocity = 10;
        private PlaneEntity engine;

        private void Awake()
        {
            engine = GetComponentInParent<PlaneEntity>(); //not clear prefab architecture
        }

        private void Update()
        {
            var progress = engine.force;
            var propellerSpeed = 2*velocity * Mathf.Sqrt(engine.force);//x2 cause max speed is required on progress =0.5f
            var fxAlpha = Mathf.Pow(4 * Mathf.Clamp(progress - .25f, 0, .25f), 2);
            var fxSpeed = -velocity * engine.force / 2;


            propellerRenderer.enabled = progress < .5f;
            propellerRoot.rotation *= Quaternion.Euler(0, propellerSpeed * Time.deltaTime, 0);
            
                fx.color = new Color(1, 1, 1, fxAlpha);
            fx.transform.rotation *= Quaternion.Euler(0, 0, fxSpeed * Time.deltaTime);
        }
    }
}