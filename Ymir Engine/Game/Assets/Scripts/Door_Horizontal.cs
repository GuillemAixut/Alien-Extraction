using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using YmirEngine;

public class Door_Horizontal : YmirComponent
{
    bool opened = false;
    float openTime = 0;
    float crono_openTime = 0;
    private Vector3 originalPosition;

    private float autoCloseDelay = 5f;

    public GameObject lDoor = null;
    public GameObject rDoor = null;

    public void Start()
    {
        originalPosition = lDoor.transform.localPosition;
        opened = false;

        //lDoor = InternalCalls.CS_GetChild(gameObject, 1);
        //rDoor = InternalCalls.CS_GetChild(gameObject, 2);

        //lDoor = InternalCalls.GetGameObjectByName("Door1");
        //rDoor = InternalCalls.GetGameObjectByName("Door2");


        Debug.Log("GetRight: " + gameObject.transform.GetRight());
    }

    public void Update()
    {
        //Debug.Log("openTime: " + openTime);

        if (opened)
        {
            openTime = Time.time - openTime;

            if (openTime >= autoCloseDelay + crono_openTime)
            {
                if (lDoor.transform.localPosition.x < originalPosition.x)
                {
                    lDoor.SetPosition(lDoor.transform.globalPosition + new Vector3(5, 0, 0));
                    rDoor.SetPosition(rDoor.transform.globalPosition + new Vector3(-5, 0, 0));
                }
                else
                {
                    opened = false;
                    openTime = 0f;
                }
            }
        }
    }

    public void OnCollisionStay(GameObject other)
    {
        if (other.Tag == "Player")
        {
            lDoor.transform.localPosition = lDoor.transform.localPosition + new Vector3(-5, 0, 0);
            rDoor.transform.localPosition = rDoor.transform.localPosition + new Vector3(5, 0, 0);
            InternalCalls.DisableComponent(gameObject, "PHYSICS");
            opened = true;
            openTime = Time.time;
            crono_openTime = Time.time;
        }
    }
}