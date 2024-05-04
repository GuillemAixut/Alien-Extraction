using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using YmirEngine;

public class Door_Horizontal : YmirComponent
{
    float timer = 0;
    float animDuration = 3f;
    float velocity = 20f;

    bool inMovement;
    bool closing;

    private GameObject lDoor;
    private GameObject rDoor;

    private bool onCollision;

    public void Start()
    {
        lDoor = InternalCalls.CS_GetChild(gameObject, 0);
        rDoor = InternalCalls.CS_GetChild(gameObject, 1);

        onCollision = false;
        inMovement = false;
        closing = false;
    }

    public void Update()
    {

        if (timer > (animDuration / 3) * 2) //Ejemplo: Hasta el segundo 4 (6 a 4 segundos)
        {
            lDoor.SetVelocity(lDoor.transform.GetForward() * velocity);
            rDoor.SetVelocity(rDoor.transform.GetForward() * -velocity);
            //Debug.Log("1. Opening: Time remaining " + timer);
        }
        else if (timer > (animDuration / 3)) //Ejemplo: Desde el segundo 4 al segundo 2 (4 a 2 segundos)
        {
            lDoor.SetVelocity(Vector3.zero);
            rDoor.SetVelocity(Vector3.zero);
            lDoor.ClearForces();
            rDoor.ClearForces();
            //Debug.Log("2. Waiting: Time remaining " + timer);
        }
        else if(onCollision)
        {
            
        }
        else if (timer > 0f) //Ejemplo: Del segundo 2 al final (2 a 0 segundos)
        {
            closing = true;
            lDoor.SetVelocity(lDoor.transform.GetForward() * -velocity);
            rDoor.SetVelocity(rDoor.transform.GetForward() * velocity);
            //Debug.Log("3. Closing: Time remaining " + timer);
        }
        else if (timer < 0f)
        {
            inMovement = false;
            closing = false;
            lDoor.SetVelocity(Vector3.zero);
            rDoor.SetVelocity(Vector3.zero);
            lDoor.ClearForces();
            rDoor.ClearForces();
        }
        
        timer -= Time.deltaTime; //Ejemplo: El timer inicia valiendo 6 
        
        Debug.Log("onCollision: " + onCollision);
    }

    public void OnCollisionStay(GameObject other)
    {
        if (other.Tag == "Player")
        {
            onCollision = true;
        }

        if (other.Tag == "Player" && !inMovement)
        {
            timer = animDuration;
            inMovement = true;
        }
        else if (other.Tag == "Player" && inMovement && closing)
        {
            timer = animDuration - timer;
            closing = false;
        }
    }

    public void OnCollisionExit(GameObject other) 
    {
        if(other.Tag == "Player")
        {
            onCollision = false;
        }
    }
}