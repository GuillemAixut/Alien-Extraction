﻿using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using YmirEngine;

public class Tp_Lvl2_Part2 : YmirComponent
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

    public void Update()
    {
        return;
    }

    public void OnCollisionEnter(GameObject other)
    {
        //TODO: Mostrat UI de que puede interactuar si pulsa el boton asignado
        if (other.Tag == "Player")
        {
            Audio.StopAllAudios();
            if (loadSceneImg != null)
            {
                loadSceneImg.SetActive(true);
            }

            InternalCalls.LoadScene("Assets/LVL2_LAB_PART2_FINAL/LVL2_LAB_PART2_COLLIDERS.yscene");
        }
    }
    
}