using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using YmirEngine;


public class Health_Bar_Test : YmirComponent
{
    public float HP = 500;
    public float initialHP = 500;
    public Vector3 initialScale;

    public void Start()
    {
        HP = 500.0f;
        initialHP = HP;
        initialScale = gameObject.transform.localScale;
        Debug.Log("HelloWorld");
    }

    public void Update()
    {
        gameObject.SetAsBillboard();

        float scaleX = Mathf.Max(0, initialScale.x * (HP / initialHP));
        Vector3 newScale = new Vector3(scaleX, initialScale.y, initialScale.z);
        gameObject.SetScale(newScale);

        if (Input.GetKey(YmirKeyCode.H) == KeyState.KEY_DOWN)
        {
            GetHeal(25);
        }

        if (Input.GetKey(YmirKeyCode.J) == KeyState.KEY_DOWN)
        {
            GetDamage(25);
        }
    }

    public void GetDamage(int damage)
    {
        if (HP >= damage)
        {
            HP -= damage;
            Debug.Log("HP (damage): " + HP);
        }
    }

    public void GetHeal(int heal)
    {
        if (HP + heal < initialHP)
        {
            HP += heal;
            Debug.Log("HP (heal): " + heal);
        }
        else
        {
            HP = initialHP;
            Debug.Log("HP (heal): " + HP);
        }
    }
}
