using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using YmirEngine;

public class Tp_To_Lvl2_Part1 : YmirComponent
{
    // Loading scene
    private GameObject loadSceneImg;
    private bool loadScene = false;

    public void Start()
	{
        loadSceneImg = InternalCalls.GetGameObjectByName("Load Scene Img");

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
            InternalCalls.LoadScene("Assets/LVL2_LAB_PART1_FINAL/LVL2_LAB_PART1_COLLIDERS.yscene");
            loadScene = false;

            return;
        }

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

            loadScene = true;
        }
    }
}