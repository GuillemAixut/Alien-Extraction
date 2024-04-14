using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using YmirEngine;

public class Tp_To_Lvl1 : YmirComponent
{

	public void Start()
	{
		Debug.Log("HelloWorld"); 
	}

	public void Update()
	{
		return;
	}

    public void OnCollisionStay(GameObject other)
    {
        //TODO: Mostrat UI de que puede interactuar si pulsa el botón asignado
        if (other.Tag == "Player")
        {
            other.SetActive(false);
            InternalCalls.LoadScene("Assets/LVL1_FINAL/LVL1_FINAL_COLLIDERS.yscene");
        }
    }
}