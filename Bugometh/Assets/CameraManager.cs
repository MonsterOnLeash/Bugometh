using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    [SerializeField]
    List<CinemachineVirtualCamera> cameras = new List<CinemachineVirtualCamera>();
    CinemachineVirtualCamera spawnCam;

    public void ChangeSpawnCamera(CinemachineVirtualCamera cam)
    {
        spawnCam = cam;
        Debug.Log(cameras.FindIndex(c => c.transform == spawnCam.transform));
        PlayerPrefs.SetInt("Camera", cameras.FindIndex(c => c.transform == spawnCam.transform));
    }

    public void ChangeActiveCamera(int idx)
    {
        for (int i = 0; i < cameras.Capacity; i++)
        {
            cameras[i].Priority = 0;
        }
        cameras[idx].Priority = 10;
    }
    
    
    public void LoadActiveCamera()
    {
        int idx = PlayerPrefs.GetInt("Camera", 0);
        Debug.Log(idx);
        for (int i = 0; i < cameras.Capacity; i++)
        {
            cameras[i].Priority = 0;
        }
        cameras[idx].Priority = 10;
    }
}
