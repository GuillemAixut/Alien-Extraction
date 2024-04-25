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
        gameObject.SetRotation(playerObject.transform.globalRotation * new Quaternion(0.7071f, 0.0f, 0.0f, -0.7071f)); // <- -90 Degree Quat

        InternalCalls.Destroy(gameObject);
    }

    public void OnCollisionStay(GameObject other)
    {

        if (playerObject.Tag != "Enemy")
        {
            Audio.PlayAudio(gameObject, "W_FSADSurf");
        }
        else
        {
            Audio.PlayAudio(gameObject, "W_FSADEnemy");
        }
    }
}