using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using YmirEngine;

public class UI_Cutscenes : YmirComponent
{
    public GameObject img = null;
    public string imgPath = "Assets\\Cutscenes\\";
    public string imgName = "";

    public int currentFrame = 0;
    public int maxFrames = 0;

    public bool hasFinished = false;

    public void Start()
    {
        img = InternalCalls.GetGameObjectByName("CutsceneImg");
        currentFrame = 0; 
        
        hasFinished = false;
    }

    public void Update()
    {
        if (img != null && !hasFinished)
        {
            if (Input.GetGamepadButton(GamePadButton.A) == KeyState.KEY_DOWN)
            {
                currentFrame++;

                UI.ChangeImageUI(img, imgPath + imgName + "(" + currentFrame.ToString() +")" + ".png", (int)UI_STATE.NORMAL);

                if (currentFrame == maxFrames)
                {
                    hasFinished = true;


                }
            }
        }

        return;
    }

    public void Reset()
    {
        hasFinished = false;
        currentFrame = 0;
    }
}