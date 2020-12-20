using UnityEngine;

namespace DefaultNamespace
{
    public class PlaneNpc
    {
        public Vector3 followTarget;
        public PlaneEntity plane;
        public float attackCooldown;
        public float attackTime;
        public NpcTask task;
    }

    public enum NpcTask
    {
        idle, attack
    }
}