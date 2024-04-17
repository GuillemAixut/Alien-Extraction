using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using YmirEngine;

public class Tp_Beacon : YmirComponent
{
    public void OnCollisionStay(GameObject other)
    {
        if (other.Tag == "Player")
        {
            Audio.StopAllAudios();
            InternalCalls.LoadScene("Assets/BASE_FINAL/LVL_BASE_COLLIDERS.yscene");
        }
    }
}