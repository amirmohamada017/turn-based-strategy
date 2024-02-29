using System;
using UnityEngine;
using Cinemachine;

public class ScreenShake : MonoBehaviour
{
    public static ScreenShake Instance { private set; get; }
    
    private CinemachineImpulseSource _cinemachineImpulseSource;

    private void Awake()
    {
        Instance = this;
        _cinemachineImpulseSource = GetComponent<CinemachineImpulseSource>();
    }

    public void Shake(float intensity = 1f)
    {
        _cinemachineImpulseSource.GenerateImpulse(intensity);
    }
}
