using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SwitchVirtualCamera : MonoBehaviour
{
    private CinemachineVirtualCamera virtualCamera;
    private int priorityBoostAmount = 10;

    [SerializeField] private Canvas thirdPersonCanvas; 
    [SerializeField] private Canvas aimCanvas;

    private void Awake()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        aimCanvas.enabled = false;
        thirdPersonCanvas.enabled = true;
    }
    public void StartAiming()
    {
        virtualCamera.Priority += priorityBoostAmount;
        aimCanvas.enabled = true;
        thirdPersonCanvas.enabled = false; 
    }
    public void CancelAiming()
    {
        virtualCamera.Priority -= priorityBoostAmount;
        aimCanvas.enabled = false;
        thirdPersonCanvas.enabled = true;
    }
}
