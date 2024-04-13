using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using YmirEngine;

public class SceneManager : YmirComponent
{
	public void Update()
	{
        if (Input.GetKey(YmirKeyCode.KP_1) == KeyState.KEY_DOWN)
        {
            InternalCalls.LoadScene("Assets/BASE_FINAL/LVL_BASE_COLLIDERS.yscene");
            return;
        }

        if (Input.GetKey(YmirKeyCode.KP_2) == KeyState.KEY_DOWN)
        {
            InternalCalls.LoadScene("Assets/LVL1_FINAL/LVL1_FINAL_COLLIDERS.yscene");
            return;
        }

        if (Input.GetKey(YmirKeyCode.KP_3) == KeyState.KEY_DOWN)
        {
            InternalCalls.LoadScene("Assets/LVL2_LAB_PART1_FINAL/LVL2_LAB_PART1_COLLIDERS.yscene");
            return;
        }

        if (Input.GetKey(YmirKeyCode.KP_4) == KeyState.KEY_DOWN)
        {
            InternalCalls.LoadScene("Assets/LVL2_LAB_PART2_FINAL/LVL2_LAB_PART2_COLLIDERS.yscene");
            return;
        }

        if (Input.GetKey(YmirKeyCode.KP_5) == KeyState.KEY_DOWN)
        {
            InternalCalls.LoadScene("Assets/LVL3_BlockOut/LVL3_BOSS_COLLDIERS.yscene");
            return;
        }

        return;
	}
}