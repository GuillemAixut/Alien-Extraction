using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using YmirEngine;

public class OpenMenu_OnCollision : YmirComponent
{
    public string goName = "";
    public GameObject canvas = null;

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

    public void OnCollisionEnter(GameObject other)
    {
        //Debug.Log("other.name");
        //Debug.Log("" + other.name);
        //Debug.Log("" + other.Name);

        if (other.Tag == "Player" || other.Name == "Player")
        {
            canvas.SetActive(true);
            UI.SetFirstFocused(canvas);
            player.PlayerStopState(true);
        }
    }
    public void OnCollisionExit(GameObject other)
    {
        if (other.Tag == "Player" || other.Name == "Player")
        {
            canvas.SetActive(false);
            player.PlayerStopState(false);
        }
    }
    //public void OnCollisionStay(GameObject other)
    //{
    //    //Debug.Log("other.name");
    //    //Debug.Log("" + other.name);
    //    //Debug.Log("" + other.Name);

    //    if (other.Tag == "Player" || other.Name == "Player")
    //    {
    //        canvas.SetActive(true);
    //        //player.inputsList.Add(INPUT.I_STOP);
    //    }
    //}
}