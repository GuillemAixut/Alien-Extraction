using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using YmirEngine;

public class SpitterAcidExplosion : YmirComponent
{
    public GameObject thisReference = null;

    private float movementSpeed;

    //private float damage = 275f;

    private GameObject player;

    private Health healthScript;

    private bool destroyed;

    private float destroyTimer;

    Quaternion rotation;

    public void Start()
    {
        movementSpeed = 4000f;
        player = InternalCalls.GetGameObjectByName("Player");
        healthScript = player.GetComponent<Health>();
        gameObject.SetImpulse(gameObject.transform.GetForward() * movementSpeed * Time.deltaTime);
        destroyed = false;
        destroyTimer = 0f;
    }

    public void Update()
    {
        destroyTimer += Time.deltaTime;

        if (destroyed)
        {
            InternalCalls.Destroy(gameObject);
        }
        else if (destroyTimer >= 1.3f)
        {
            //DO EXPLOSION
            rotation = gameObject.transform.globalRotation;
            InternalCalls.CreateSpitterAcidShrapnel(gameObject.transform.globalPosition, rotation);
            float angle = 90.0f;
            rotation = Quaternion.RotateQuaternionY(rotation, angle);
            InternalCalls.CreateSpitterAcidShrapnel(gameObject.transform.globalPosition, rotation);
            angle = 180.0f;
            rotation = Quaternion.RotateQuaternionY(rotation, angle);
            InternalCalls.CreateSpitterAcidShrapnel(gameObject.transform.globalPosition, rotation);
            angle = 270.0f;
            rotation = Quaternion.RotateQuaternionY(rotation, angle);
            InternalCalls.CreateSpitterAcidShrapnel(gameObject.transform.globalPosition, rotation);

            InternalCalls.Destroy(gameObject);
        }

    }

    public void OnCollisionStay(GameObject other)
    {
        if (other.Name == "Player" && destroyed == false)
        {
            //healthScript.TakeDmg(damage);
            //destroyed = true;
        }
    }

}