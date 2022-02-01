using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    public CinemachineVirtualCamera target;
    public CinemachineVirtualCamera selfCamera;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            selfCamera.Priority = 0;
            target.Priority = 10;
        }
    }
}
