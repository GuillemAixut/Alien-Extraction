using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using YmirEngine;

public class TextPopUp : YmirComponent
{
    public GameObject PopUpCanvas;
    bool show;

    public void Start()
	{
		PopUpCanvas.SetActive(false);
	}

    public void OnCollisionStay(GameObject other)
    {
        if (other.Tag == "Player")
        {
            show = true;
        }
    }

    public void Update()
    {
        PopUpCanvas?.SetActive(show);
        show = false;
    }
}