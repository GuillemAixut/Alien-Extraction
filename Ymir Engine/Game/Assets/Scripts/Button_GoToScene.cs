using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using YmirEngine;

public class Button_GoToScene : YmirComponent
{
    public string sceneName = "BASE_FINAL/LVL_BASE_COLLIDERS";
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

    }

    public void OnClickButton()
    {
        Debug.Log("Go to scene " + sceneName + ".yscene");
        Audio.PauseAllAudios();

        Globals.GetPlayerScript().SavePlayer();
        
        if (loadSceneImg != null)
        {
            loadSceneImg.SetActive(true);
        }

        InternalCalls.LoadScene("Assets/" + sceneName + ".yscene");
    }
}