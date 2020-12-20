using System;
using UnityEngine;

namespace DefaultNamespace.Ui
{
    public class TargetsTrackerUi : MonoBehaviour
    {
        [SerializeField] private TargetIcon prototype;
        private void Start()
        {
            prototype.gameObject.SetActive(false);

            foreach (var npc in GameManager.instance.aiSystem.list)
            {
                var tracker = Instantiate(prototype, transform);
                tracker.target = npc;
                tracker.gameObject.SetActive(true);
            }
        }
    }
}