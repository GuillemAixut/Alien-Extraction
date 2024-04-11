using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using YmirEngine;

public class OpenMenu_OnCollision : YmirComponent
{
    public string goName;
    public GameObject canvas;

    private Player player = null;

    public void Start()
    {
        GameObject gameObject = InternalCalls.GetGameObjectByName("Player");

        if (gameObject != null)
        {
            player = gameObject.GetComponent<Player>();
        }

        canvas = InternalCalls.GetGameObjectByName(goName);
    }

    public void Update()
    {
        return;
    }

    public void OnCollisionStay(GameObject other)
    {
        if (other.Tag == "Player")
        {
            canvas.SetActive(true);
            player.inputsList.Add(INPUT.I_STOP);
        }
    }
}