using System;
using System.Collections.Generic;
using Physics;
using UnityEngine;

namespace DefaultNamespace
{
    public class FlapsAnimator : MonoBehaviour
    {
        internal enum Axis
        {
            x,
            y,
            z
        }

        [Serializable]
        private class Flap
        {
            public Axis axis;
            public Transform transform;

            public float Angle
            {
                set =>
                    transform.localRotation = Quaternion.Euler(
                        axis == Axis.x ? value : 0,
                        axis == Axis.y ? value : 0,
                        axis == Axis.z ? value : 0
                    );
            }
        }

        [SerializeField] private float Amplitude = 20;

        [SerializeField] private List<Flap> yaw;
        [SerializeField] private List<Flap> leftRoll;
        [SerializeField] private List<Flap> rightRoll;
        private Wings wings;

        private void Awake()
        {
            wings = GetComponentInParent<Wings>();
        }

        private void Update()
        {
            foreach (var flap in yaw)
            {
                flap.Angle = wings.Yaw * Amplitude;
            }

            foreach (var flap in leftRoll)
            {
                flap.Angle = Amplitude * Mathf.Clamp(wings.Roll - wings.Pitch, -1, 1);
            }

            foreach (var flap in rightRoll)
            {
                flap.Angle = Amplitude * Mathf.Clamp(-wings.Roll - wings.Pitch, -1, 1);
            }
        }
    }
}