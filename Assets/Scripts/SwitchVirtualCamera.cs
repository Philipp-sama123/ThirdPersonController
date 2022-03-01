using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchVirtualCamera : MonoBehaviour
{
    private CinemachineVirtualCamera virtualCamera;
    private int priorityBoostAmount = 10; 
    private void Awake()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>(); 
    }

    public void HandleAiming(bool isAiming)
    {
        if(isAiming)
        {
            StartAim(); 
        }else
        {
            CancelAiming(); 
        }
    }

    private void CancelAiming()
    {
        virtualCamera.Priority += priorityBoostAmount; 
    }

    private void StartAim()
    {
        virtualCamera.Priority -= priorityBoostAmount;
    }
}
