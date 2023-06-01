using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPDisplayController : MonoBehaviour
{
    private Camera mainCamera;
    private bool isSetup;
    public Transform rotateTarget;

    private void Start()
    {
        mainCamera = Camera.main;
        isSetup = !(mainCamera != null) && !(rotateTarget != null);
    }

    private void Update()
    {

            TurnTurnTowardsCamera();
        
    }

    private void TurnTurnTowardsCamera()
    {
        rotateTarget.rotation.SetLookRotation(mainCamera.transform.position, rotateTarget.up);
    }
}