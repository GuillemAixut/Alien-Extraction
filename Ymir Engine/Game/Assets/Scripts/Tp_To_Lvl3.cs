using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using YmirEngine;

public class Tp_To_Lvl3 : YmirComponent
{

	public void Start()
	{
		Debug.Log("HelloWorld"); 
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
            InternalCalls.LoadScene("Assets/LVL3_BlockOut/LVL3_BOSS_COLLDIERS.yscene");
        }
    }
}