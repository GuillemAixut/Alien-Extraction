using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using YmirEngine;

public class Key : YmirComponent
{
	public GameObject door;
	public void Start()
	{
		
	}

	public void Update()
	{
		
	}

    public void OnCollisionStay(GameObject other)
    {
        if (other.Tag == "Player")
        {
            Audio.PlayEmbedAudio(gameObject);

            InternalCalls.DisableComponent(door, "PHYSICS", true);
            gameObject.SetActive(false);
            InternalCalls.Destroy(gameObject);

        }
    }
}