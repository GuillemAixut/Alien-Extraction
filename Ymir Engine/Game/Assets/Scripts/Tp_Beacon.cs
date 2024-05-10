using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using YmirEngine;

public class Tp_Beacon : YmirComponent
{
    // Loading scene
    private GameObject loadSceneImg;
    private bool loadScene = false;

    public void Start()
    {
        loadSceneImg = InternalCalls.GetGameObjectByName("Loading Scene Canvas");

        if (loadSceneImg != null)
        {
            loadSceneImg.SetActive(false);
        }

        loadScene = false;
    }

    public void Update()
    {
        if (loadScene)
        {
            InternalCalls.LoadScene("Assets/BASE_FINAL/LVL_BASE_COLLIDERS.yscene");
            loadScene = false;

            return;
        }

        return;
    }

    public void OnCollisionStay(GameObject other)
    {
        if (other.Tag == "Player")
        {
            Audio.StopAllAudios();
            if (loadSceneImg != null)
            {
                loadSceneImg.SetActive(true);
            }

            loadScene = true;
        }
    }
}