using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class VirtualCollider : MonoBehaviour
{

    [SerializeField] private MeshRenderer mesh;


    private void OnEnable()
    {
        GameManager.instance.colliders.RegisterCollider(this);
    }

    private void OnDisable()
    {
        GameManager.instance.colliders.UnregisterCollider(this);
    }

    public bool HitTest(Ray ray, out float distance) => mesh.bounds.IntersectRay(ray, out distance);


}