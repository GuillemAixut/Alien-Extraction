using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using YmirEngine;

public class BH_Shotgun : YmirComponent
{
    Player player;
    private float timer;
    private bool destroyed;

    private Vector3 size;

    GameObject playerObject;

    public void Start()
    {
        playerObject = InternalCalls.GetGameObjectByName("Player");
    }

    public void Update()
    {
    
        //InternalCalls.Destroy(gameObject);
        
    }

    public void OnCollisionStay(GameObject other)
    {
        if (other.Tag == "Enemy")
        {
            Debug.Log("EnemyHIT!");
        }
    }
}