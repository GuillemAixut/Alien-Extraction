using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using YmirEngine;

public class DroneXenomorphAttack : YmirComponent
{

    public GameObject thisReference = null;

    public GameObject drone;

    private float damageTimer;

    private float clawDamage;
    private float tailDamage;

    private GameObject player;

    private Health healthScript;

    public void Start()
	{
        player = InternalCalls.GetGameObjectByName("Player");
        healthScript = player.GetComponent<Health>();
        damageTimer = 0f;
        clawDamage = 150f;
        tailDamage = 200f;
    }

    public void Update()
	{
        damageTimer -= Time.deltaTime;

        gameObject.SetRotation(drone.transform.globalRotation);

        if (drone.GetComponent<DroneXenomorphBaseScript>().timeCounter > 0.5f)
        {
            gameObject.SetPosition(drone.transform.globalPosition);
        }
        else if ((drone.GetComponent<DroneXenomorphBaseScript>().timeCounter < 0.5f) && (drone.GetComponent<DroneXenomorphBaseScript>().GetState() == DroneState.CLAW || drone.GetComponent<DroneXenomorphBaseScript>().GetState() == DroneState.TAIL))
        {
            //gameObject.SetPosition(new Vector3(gameObject.transform.globalPosition.x, gameObject.transform.globalPosition.y, gameObject.transform.GetForward().z + 5));
            if (drone.GetComponent<DroneXenomorphBaseScript>().CheckDistance(gameObject.transform.globalPosition, drone.transform.globalPosition, 10f))
            {
                gameObject.SetVelocity(gameObject.transform.GetForward() * 100f);
            }
            else
            {
                gameObject.SetVelocity(gameObject.transform.GetForward() * 0f);
            }
        }

    }

    public void OnCollisionStay(GameObject other)
    {
        if (other.Name == "Player" && damageTimer <= 0)
        {
            //Debug.Log("[ERROR] HIT");
            damageTimer = 0.8f;
            if (drone.GetComponent<DroneXenomorphBaseScript>().GetState() == DroneState.CLAW)
            {
                healthScript.TakeDmg(clawDamage);
            }
            else if (drone.GetComponent<DroneXenomorphBaseScript>().GetState() == DroneState.TAIL)
            {
                healthScript.TakeDmg(tailDamage);
            }
        }
    }

}