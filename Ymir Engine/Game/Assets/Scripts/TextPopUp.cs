using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using YmirEngine;

public class TextPopUp : YmirComponent
{
    public GameObject textUI;
    bool show;

    public void Start()
	{
        textUI.SetActive(false);
	}

    public void OnCollisionStay(GameObject other)
    {
        if (other.Tag == "Player")
        {
            show = true;
            textUI.SetActive(true);
        }
    }

    public void Update()
    {
        if (!show)
        {
            textUI.SetActive(false);
        }

        show = false;
    }
}