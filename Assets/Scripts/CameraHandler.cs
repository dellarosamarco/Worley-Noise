using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    public static CameraHandler instance;
    public GameObject camera3D;
    public GameObject camera2D;

    private void Awake()
    {
        instance = this;
    }

    public void set3D()
    {
        camera3D.SetActive(true);
        camera2D.SetActive(false);
    }

    public void set2D()
    {
        camera3D.SetActive(false);
        camera2D.SetActive(true);
    }
}
