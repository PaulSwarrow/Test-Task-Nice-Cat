using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] public Transform target;

    [SerializeField] private Vector3 offset;
    [SerializeField] private Vector3 lookOffset;
    [SerializeField] private float moveSpeed = 2;
    [SerializeField] private float lookSpeed =2 ;


    void FixedUpdate()
    {
        var targetPosition = target.position + target.TransformDirection(offset);
        var lookPosition = target.position + target.TransformDirection(lookOffset);

        transform.position = Vector3.Slerp(transform.position, targetPosition, moveSpeed * Time.fixedDeltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookPosition - transform.position, target.up), lookSpeed*Time.fixedDeltaTime );
    }
}
