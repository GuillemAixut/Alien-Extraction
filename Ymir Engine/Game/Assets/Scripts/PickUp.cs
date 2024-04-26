using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using YmirEngine;

public class PickUp : YmirComponent
{
	private bool picked = false;

	public void Start()
	{
		picked = false;
	}

	public void Update()
	{

	}

	public void OnCollisionStay(GameObject other)
	{
		if (other.Tag == "Player" && !picked)
		{
			Audio.PlayEmbedAudio(gameObject);

			//TODO: Hacer que el item se destruya/elimine
			gameObject.SetActive(false);
			InternalCalls.Destroy(gameObject);
            picked = true;

            //TODO: Hacer que se sumen al inventario o algo para mantener la cuenta
        }
	}
}