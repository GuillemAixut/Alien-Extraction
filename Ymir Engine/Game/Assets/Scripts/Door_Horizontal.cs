using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using YmirEngine;

public class Door_Horizontal : YmirComponent
{
    float timer = 0;
    float animDuration = 5f;  

    public void Start()
    {
    }

    public void Update()
    {
        if (timer > -0.1)
        {
            timer -= Time.deltaTime; //El timer inicia valiendo 6
        }

        if (timer > 4.0f) // Hasta el segundo 4 (6 a 4 segundos)
        {
            //TODO: Set the collider inactive
            gameObject.SetVelocity(gameObject.transform.GetForward() * 5);
            Debug.Log("1. Primera fase: Tiempo restante " + timer);
        }
        else if (timer > 2.0f) // Desde el segundo 4 al segundo 2 (4 a 2 segundos)
        {
            gameObject.SetVelocity(Vector3.zero);
            gameObject.ClearForces();
            Debug.Log("2. Segunda fase: Tiempo restante " + timer);
        }
        else if (timer > 0f) // Del segundo 2 al final (2 a 0 segundos)
        {
            gameObject.SetVelocity(gameObject.transform.GetForward() * -5);
            Debug.Log("3. Tercera fase: Tiempo restante " + timer);
        }
        else if (timer < 0f)
        {
            //TODO: Set the collider active
            gameObject.SetVelocity(Vector3.zero);
            gameObject.ClearForces();
        }

    }

    public void OnCollisionStay(GameObject other)
    {
        if (other.Tag == "Player")
        {
            timer = animDuration;
        }
    }
}