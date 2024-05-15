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

	private float damage = 350f;

    private GameObject player;

    private Health healthScript;

    private bool destroyed;

    private float destroyTimer;


    public void Start()
	{
		movementSpeed = 7000f;
        player = InternalCalls.GetGameObjectByName("Player");
        healthScript = player.GetComponent<Health>();
        destroyed = false;
        destroyTimer = 0f;
        Vector3 direction = gameObject.transform.globalPosition - player.transform.globalPosition;
        Quaternion rotation = Quaternion.LookRotation(direction);
        Debug.Log("[ERROR] ROTATIONNN " + gameObject.transform.globalRotation);
        gameObject.transform.localRotation = rotation;
        gameObject.SetRotation(rotation);
        gameObject.SetImpulse(gameObject.transform.GetForward() * movementSpeed * Time.deltaTime);
        Debug.Log("[ERROR] ROTATIONNN " + gameObject.transform.globalRotation);
    }

    public void Update()
	{
        destroyTimer += Time.deltaTime;

        if (destroyed || destroyTimer >= 2f) 
        {
            InternalCalls.Destroy(gameObject);
        }

    }

    public void OnCollisionStay(GameObject other)
    {
        if (other.Name == "Player" && destroyed == false && player.GetComponent<Player>().vulnerable)
        {
            healthScript.TakeDmg(damage);
            destroyed = true;

            player.GetComponent<Player>().TakeDMG();
        }
    }

}