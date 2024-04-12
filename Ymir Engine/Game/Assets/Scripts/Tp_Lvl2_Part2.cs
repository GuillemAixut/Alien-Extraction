using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using YmirEngine;

public class Tp_Lvl2_Part2 : YmirComponent
{
    public void OnCollisionStay(GameObject other)
    {
        if(other.Tag == "Player")
        {
            InternalCalls.LoadScene("Assets/LVL2_LAB_PART2_FINAL/LVL2_LAB_PART2.yscene");
        }
    }
    
}