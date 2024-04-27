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

    public GameObject enemy = null;
    public GameObject plane = null;

    private FaceHuggerBaseScript aux = null;
    private DroneXenomorphBaseScript aux2 = null;

    public void Start()
    {
        HP = 500.0f;
        initialHP = HP;
        plane = InternalCalls.CS_GetChild(gameObject, 0);
        initialScale = plane.transform.localScale;

        aux = enemy.GetComponent<FaceHuggerBaseScript>();
        aux2 = enemy.GetComponent<DroneXenomorphBaseScript>();
    }

    public void Update()
    {
        gameObject.transform.localPosition = new Vector3(enemy.transform.localPosition.x, gameObject.transform.localPosition.y, enemy.transform.localPosition.z);


        gameObject.SetAsBillboard();

        float scaleX = Mathf.Max(0, initialScale.x * (HP / initialHP));
        Vector3 newScale = new Vector3(scaleX, initialScale.y, initialScale.z);
        plane.SetScale(newScale);

        //---------------Xiao: Gurrada Pendiente de Cambiar----------------------------
        if (aux != null)
        {
            HP = aux.life;
        }

        if (aux2 != null)
        {
            HP = aux2.life;
        }
    }
    public void Destroy()
    {
        InternalCalls.Destroy(gameObject);
    }

}
