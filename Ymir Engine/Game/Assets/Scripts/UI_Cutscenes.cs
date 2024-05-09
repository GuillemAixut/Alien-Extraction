using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using YmirEngine;

public class UI_Cutscenes : YmirComponent
{
    public GameObject img = null;
    public GameObject button = null;
    public string imgPath = "Assets\\Cutscenes\\";
    public string imgName = "";

    public int currentFrame = 0;
    public int maxFrames = 0;

    public bool hasFinished = false;
    public bool introScene = false;
    public bool winScene = false;
    public bool loseScene = false;

    private float timer = 2f;
    private float finishTimer = 2f;
    public void Start()
    {
        button = InternalCalls.GetGameObjectByName("Button_A");
        img = InternalCalls.GetGameObjectByName("CutsceneImg");
        currentFrame = 0; 
        
        hasFinished = false;
    }

    public void Update()
    {
        if (img != null && !hasFinished && button != null)
        {
           
            if (finishTimer >= 0)
            {
                finishTimer -= Time.deltaTime;
                button.SetActive(false);
               
            }
            if (finishTimer <= 0)
            {
                button.SetActive(true);
                //Poner Aqui la imagen
                if (Input.GetGamepadButton(GamePadButton.A) == KeyState.KEY_DOWN)
                {
                    currentFrame++;
                    finishTimer = timer;
                    UI.ChangeImageUI(img, imgPath + imgName + "(" + currentFrame.ToString() + ")" + ".png", (int)UI_STATE.NORMAL);

                    if (currentFrame == maxFrames)
                    {
                        hasFinished = true;
                        if (introScene)
                        {
                            InternalCalls.LoadScene("Assets/BASE_FINAL/LVL_BASE_COLLIDERS");
                        }
                    }
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