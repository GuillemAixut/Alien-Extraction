using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using YmirEngine;

public class Teleport_OnCollision : YmirComponent
{
    public string scene = "";
    //private GameObject loadSceneImg = null;

    public void Start()
    {
        //loadSceneImg = InternalCalls.GetGameObjectByName("Load Scene Img");
        //loadSceneImg.SetActive(false);
    }

    public void Update()
    {
        return;
    }

    public void OnCollisionEnter(GameObject other)
    {
        if (other.Tag == "Player")
        {
            Audio.StopAllAudios();
            //loadSceneImg.SetActive(true);

            InternalCalls.LoadScene(scene);
        }
    }
}