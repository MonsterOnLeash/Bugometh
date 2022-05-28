using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Experimental.Rendering.Universal;

public class CameraController : MonoBehaviour
{
    public CinemachineVirtualCamera target;
    public CinemachineVirtualCamera selfCamera;
    private Light2D globalLight;
    public bool changeIntensity = false;
    public float intensity = 1;


    private void Awake()
    {
        globalLight = GameObject.Find("Global Light 2D").GetComponent<Light2D>();
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            selfCamera.Priority = 0;
            target.Priority = 10;
            if (changeIntensity)
            {
                globalLight.intensity = intensity;
            }
        }
    }
    
}
