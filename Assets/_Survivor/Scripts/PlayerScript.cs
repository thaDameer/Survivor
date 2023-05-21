using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private float moveSpeed = 5;
    Vector3 inputDir
    {
        get
        {
            Vector3 input = new Vector3(Input.GetAxis("Horizontal"),0,Input.GetAxis("Vertical"));
            return input;
        }
    }

    private void FixedUpdate()
    {
        if (inputDir != Vector3.zero)
        {
            _rigidbody.velocity = inputDir * moveSpeed;
            var rot = Quaternion.LookRotation(inputDir,Vector3.up);
            rot.y = 0;
            transform.rotation = rot;
        }
    }
}
