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
        Vector3 shotgunOffset;
        Vector3 offsetDirection = gameObject.transform.GetForward().normalized;
        float distance = 40.0f;

        shotgunOffset = gameObject.transform.globalPosition + new Vector3(0,15,0) + (offsetDirection * distance);
        gameObject.SetPosition(shotgunOffset);
        gameObject.SetRotation(playerObject.transform.globalRotation * new Quaternion(0.7071f, 0.0f, 0.0f, -0.7071f)); // <- -90 Degree Quat

        InternalCalls.Destroy(gameObject);
    }

    public void OnCollisionStay(GameObject other)
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
            Debug.Log("[ERROR] HIT ENEMy");
           
        }
    }
}