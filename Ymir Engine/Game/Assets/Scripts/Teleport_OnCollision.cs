using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using YmirEngine;

public class Teleport_OnCollision : YmirComponent
{
    public string scene = "";

    public void Start()
    {
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
            InternalCalls.LoadScene(scene);
        }
    }
}