using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Experimental.Rendering.Universal;

public static class SpawnManager
{

    private static List<Vector2> saves = new List<Vector2>();
    private static Vector2 respawnPoint;

    public static void NewPoint(Vector2 newPoint)
    {
        if (!saves.Contains(newPoint))
            saves.Add(newPoint);
        respawnPoint = newPoint;
        PlayerPrefs.SetFloat("x", newPoint.x);
        PlayerPrefs.SetFloat("y", newPoint.y);
        PlayerPrefs.SetFloat("globalIntensity", GameObject.Find("Global Light 2D").GetComponent<Light2D>().intensity);
    }


}
