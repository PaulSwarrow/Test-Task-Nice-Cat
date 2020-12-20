using System;
using UnityEngine;

namespace DefaultNamespace.Ui
{
    public class AimUi : MonoBehaviour
    {

        [SerializeField] private RectTransform pointer;

        private Camera camera;
        private RectTransform rectTransform;

        private void Awake()
        {
            camera = Camera.main;
            rectTransform = (RectTransform) transform;

        }

        private void Update()
        {
            var position = camera.WorldToScreenPoint(GameManager.instance.playerController.AimPoint);
            pointer.anchoredPosition = position;
            
        }

    }
}