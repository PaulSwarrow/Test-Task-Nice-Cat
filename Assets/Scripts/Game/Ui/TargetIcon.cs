using System;
using System.Drawing;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

namespace DefaultNamespace.Ui
{
    public class TargetIcon : MonoBehaviour
    {
        public PlaneNpc target;

        private Camera camera;
        private RectTransform rectTransform;
        [SerializeField] private Image frame;
        [SerializeField] private Text distanceField;
        [SerializeField] private Image arrow;

        [SerializeField] private float padding = 20;
        [SerializeField] private float textOffsetX = 60;
        [SerializeField] private float textOffsetY = 38;

        private void Awake()
        {
            camera = Camera.main;
            rectTransform = (RectTransform) transform;
        }

        private void Update()
        {
            var position = camera.WorldToScreenPoint(target.plane.transform.position);
            var visible = Screen.safeArea.Contains(position) && position.z > 0;
            position.z = 0;
            frame.enabled = visible;
            arrow.enabled = !visible;
            distanceField.text =
                Vector3.Distance(camera.transform.position, target.plane.transform.position).ToString();
            if (!visible)
            {
                var w = Screen.safeArea.size.x;
                var h = Screen.safeArea.size.y;
                var delta = target.plane.transform.position - camera.transform.position;
                var center = new Vector3(w / 2, h / 2);
                var vector = camera.transform.InverseTransformDirection(delta);
                vector.z = 0;
                vector.Normalize();

                var offset = vector * Screen.safeArea.size.magnitude;
                offset.x = Mathf.Clamp(offset.x, padding - w / 2, w / 2 - padding);
                offset.y = Mathf.Clamp(offset.y, padding - h / 2, h / 2 - padding);
                rectTransform.position = center + offset;
                arrow.transform.rotation = Quaternion.LookRotation(Vector3.forward, offset);

                distanceField.rectTransform.anchoredPosition =
                    new Vector2(-vector.x * textOffsetX, -vector.y * textOffsetY);
            }
            else
            {
                distanceField.rectTransform.anchoredPosition = new Vector2(0, -textOffsetY);
                rectTransform.position = position;
            }
        }
    }
}