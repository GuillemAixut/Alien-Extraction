using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using YmirEngine;

public class Tp_To_Lvl3_Part1 : YmirComponent
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

            InternalCalls.LoadScene("Assets/LVL3_BlockOut/LVL3_PART1_COLLIDERS.yscene");
        }
    }
}