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

    private void Awake()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
    }
    public void StartAiming()
    {
        virtualCamera.Priority += priorityBoostAmount;
    }
    public void CancelAiming()
    {
        virtualCamera.Priority -= priorityBoostAmount;
    }

}
