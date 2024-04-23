using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using YmirEngine;

public class Enemy_Test : YmirComponent
{
	public void Start()
	{
 
	}

	public void Update()
	{

	}

    public void OnCollisionStay(GameObject other)
    {
        if (other.Name == "Tail")
        {
            gameObject.SetImpulse(gameObject.transform.GetForward() * -4);
            //InternalCalls.Destroy(gameObject);
            //Le hace daño
        }
    }

    public void OnCollisionExit(GameObject other)
    {
        if (other.Name == "Tail")
        {
            Debug.Log("Manel");
            gameObject.SetVelocity(new Vector3(0, 0, 0));
            gameObject.ClearForces();
        }
    }
}