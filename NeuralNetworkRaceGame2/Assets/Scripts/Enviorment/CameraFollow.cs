﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform objectToFollow;
    public Vector3 offset = new Vector3(0f,.83f,-2.54f);
    public float followSpeed = 10;
    public float lookSpeed = 10;
    public bool follow = false;

    private void LookAtTarget()
    {
        Vector3 _lookDirection = objectToFollow.position - transform.position;
        Quaternion _rot = Quaternion.LookRotation(_lookDirection, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, _rot, lookSpeed * Time.deltaTime);
    }

    private void MoveToTarget()
    {
        Vector3 _targetPos = objectToFollow.position +
                             objectToFollow.forward * offset.z +
                             objectToFollow.right * offset.x +
                             objectToFollow.up * offset.y;
        transform.position = Vector3.Lerp(transform.position, _targetPos, followSpeed * Time.deltaTime);
    }

    private void FixedUpdate()
    {
        if (follow)
        {
            LookAtTarget();
            MoveToTarget();
        }
    }

   
}
