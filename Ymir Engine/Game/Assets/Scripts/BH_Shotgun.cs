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
        gameObject.SetPosition(player.currentWeapon.gameObject.transform.globalPosition + player.currentWeapon.offset + (player.currentWeapon.gameObject.transform.GetForward() * player.currentWeapon.range * 4));
        gameObject.SetRotation(playerObject.transform.globalRotation * new Quaternion(0.7071f, 0.0f, 0.0f, -0.7071f)); // <- -90 Degree Quat

        InternalCalls.Destroy(gameObject);
    }

    public void OnCollisionEnter(GameObject other)
    {

        if (other.Tag != "Enemy")
        {
            Audio.PlayAudio(gameObject, "W_FSADSurf");
        }
        else
        {
            Audio.PlayAudio(gameObject, "W_FSADEnemy");
            FaceHuggerBaseScript aux = other.GetComponent<FaceHuggerBaseScript>();

            if (aux != null)
            {
                aux.life -= 110;
            }

            DroneXenomorphBaseScript aux2 = other.GetComponent<DroneXenomorphBaseScript>();
            if (aux2 != null)
            {
                aux2.life -= 110;
            }

            QueenXenomorphBaseScript aux3 = other.GetComponent<QueenXenomorphBaseScript>();
            if (aux3 != null)
            {
                aux3.life -= 110;
            }

            SpitterBaseScript aux4 = other.GetComponent<SpitterBaseScript>();
            if (aux4 != null)
            {
                aux4.life -= 110;
            }

            Debug.Log("[ERROR] HIT ENEMy");
           
        }
    }
}