using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using YmirEngine;

public class SpitterAcidSpit : YmirComponent
{
    public GameObject thisReference = null;

    private float movementSpeed;

	private float damage = 300f;

    private GameObject player;

    private Health healthScript;

    private bool destroyed;

    public void Start()
	{
		movementSpeed = 7000f;
        player = InternalCalls.GetGameObjectByName("Player");
        healthScript = player.GetComponent<Health>();
        gameObject.SetImpulse(gameObject.transform.GetForward() * -movementSpeed * Time.deltaTime);
        destroyed = false;
	}

	public void Update()
	{
        if (destroyed) 
        {
            //InternalCalls.Destroy(gameObject);
        }

    }

    public void OnCollisionStay(GameObject other)
    {
        if (other.Name == "Player" && destroyed == false)
        {
            healthScript.TakeDmg(damage);
            destroyed = true;
        }
    }

}