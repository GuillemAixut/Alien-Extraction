using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using YmirEngine;

public class Button_Navigation : YmirComponent
{
    public string sceneName = "BASE_FINAL/LVL_BASE_COLLIDERS";

    public void OnClickButton()
    {
        Debug.Log("Go to scene" + sceneName + ".yscene");
        Audio.PauseAllAudios();
        InternalCalls.LoadScene("Assets/" + sceneName + ".yscene");
    }
}