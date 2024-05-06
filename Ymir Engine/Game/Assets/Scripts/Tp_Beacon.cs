using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using YmirEngine;

public class Tp_Beacon : YmirComponent
{
    private GameObject loadSceneImg;

    public void Start()
    {
        loadSceneImg = InternalCalls.GetGameObjectByName("Load Scene Img");

        if (loadSceneImg != null)
        {
            loadSceneImg.SetActive(false);
        }
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

            InternalCalls.LoadScene("Assets/BASE_FINAL/LVL_BASE_COLLIDERS.yscene");
        }
    }
}