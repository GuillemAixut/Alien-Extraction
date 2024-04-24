using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using YmirEngine;

public class BH_Shotgun : YmirComponent
{
    GameObject playerObject;
    Player player;

    public void Start()
    {
        playerObject = InternalCalls.GetGameObjectByName("Player");
        player = playerObject.GetComponent<Player>();
    }

    public void Update()
    {
        gameObject.SetPosition(player.shotgunOffset);
        InternalCalls.Destroy(gameObject);
    }

    public void OnCollisionStay(GameObject other)
    {

        Debug.Log("ShotgunHit");

        if (other.Tag == "Enemy")
        {
            Debug.Log("EnemyHIT!");
        }
    }
}