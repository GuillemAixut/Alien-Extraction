using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using YmirEngine;

public class Alien_Trap : YmirComponent
{
    public float velocity = 10f;
    public bool axisZ = false;
    public float damage = 10f;
    private float time = 0f;
    private bool active = false;
    private bool hitPlayer = false;
    public void Start()
    {
        Debug.Log("HelloWorld");
    }

    public void Update()
    {
        if (Input.GetKey(YmirKeyCode.SPACE) == KeyState.KEY_DOWN)
        {
            active = true;
            time = 0f;
        }
        if (active & !axisZ)
        {
            if (time < 0.3f)
            {

                gameObject.transform.localPosition = gameObject.transform.localPosition + new Vector3(velocity * time, 0, 0);
                time += Time.deltaTime;
                if (hitPlayer)
                {
                    active = false;
                }
            }
            if (time > 0.3f && time < 0.6f)
            {
                gameObject.transform.localPosition = gameObject.transform.localPosition - new Vector3(velocity * (time - 0.3f), 0, 0);
                time += Time.deltaTime;
            }
        }

        if (active & axisZ)
        {
            if (time < 0.3f)
            {

                gameObject.transform.localPosition = gameObject.transform.localPosition + new Vector3(0, 0, velocity * time);
                time += Time.deltaTime;

            }
            if (time > 0.3f && time < 0.6f)
            {
                gameObject.transform.localPosition = gameObject.transform.localPosition - new Vector3(0, 0, velocity * (time - 0.3f));
                time += Time.deltaTime;
            }
        }


        return;
    }


    public void OnCollisionStay(GameObject other)
    {
        if (other.Tag == "Player")
        {
            if (active)
            {
                other.GetComponent<Health>().TakeDmg(damage);
                hitPlayer = true;
                // Debug.Log("" + other.GetComponent<Health>().currentHealth);
            }



        }
    }


}