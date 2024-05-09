using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using YmirEngine;

public class Button_GoToScene : YmirComponent
{
    public string sceneName = "BASE_FINAL/LVL_BASE_COLLIDERS";

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
            InternalCalls.LoadScene("Assets/" + sceneName + ".yscene");
            loadScene = false;

            return;
        }
    }

    public void OnClickButton()
    {
        Debug.Log("Go to scene " + sceneName + ".yscene");
        Audio.PauseAllAudios();

        if (loadSceneImg != null)
        {
            loadSceneImg.SetActive(true);
        }

        loadScene = true;

        Globals.GetPlayerScript().SavePlayer();
    }
}