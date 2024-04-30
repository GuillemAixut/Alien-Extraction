using System;
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

    public bool inverted;

    public void Start()
    {
        inMovement = false;
        closing = false;
    }

    public void Update()
    {
        if (inverted)
        {
            velocity = -velocity;
            inverted = false;
        }

        if (timer > -0.1)
        {
            timer -= Time.deltaTime; //Ejemplo: El timer inicia valiendo 6
        }

        if (timer > (animDuration / 3) * 2) //Ejemplo: Hasta el segundo 4 (6 a 4 segundos)
        {
            gameObject.SetVelocity(gameObject.transform.GetForward() * velocity);
            Debug.Log("1. Opening: Time remaining " + timer);
        }
        else if (timer > (animDuration / 3)) //Ejemplo: Desde el segundo 4 al segundo 2 (4 a 2 segundos)
        {
            gameObject.SetVelocity(Vector3.zero);
            gameObject.ClearForces();
            Debug.Log("2. Waiting: Time remaining " + timer);
        }
        else if (timer > 0f) //Ejemplo: Del segundo 2 al final (2 a 0 segundos)
        {
            closing = true;
            gameObject.SetVelocity(gameObject.transform.GetForward() * -velocity);
            Debug.Log("3. Closing: Time remaining " + timer);
        }
        else if (timer < 0f)
        {
            inMovement = false;
            closing = false;
            gameObject.SetVelocity(Vector3.zero);
            gameObject.ClearForces();
        }
    }

    public void OnCollisionStay(GameObject other)
    {
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
}