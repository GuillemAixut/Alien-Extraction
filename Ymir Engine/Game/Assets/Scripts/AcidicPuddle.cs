using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Lifetime;
using YmirEngine;

public class AcidPuddle : YmirComponent
{
    private Player player;
    private GameObject playerObject;

    //private float damage = 25;
    //private int currentTicks = 0; //Guardamos los ticks actuales para esperar
    private float tickDamage = 1.064f;
    //private bool addTick; //No se si los triggers suceden todos a la vez pero por si acaso me esperare 1 update
    private float lifeTimer;
   // private float duration = 1.8f;


    public void Start()
    {
        Debug.Log("ACID PUDDLE");

        lifeTimer = 0;
        tickDamage = 1.064f;
        //Game object del player para interactuar sobre el
        playerObject = GetPlayer();

        if(playerObject != null )
        {
            player = playerObject.GetComponent<Player>();
        }
        
    }

    public void Update()
    {
        lifeTimer = player.acidicTimer;

        if (lifeTimer <= 0)
        {
            InternalCalls.Destroy(gameObject);
        }
    }

    public void OnCollisionEnter(GameObject other)
    {
        //if (other.Tag == "Enemy")
        //{
        //    //Deal damage
        //    other.GetComponent<Enemy>().life -= damage;
        //}
    }
    public void OnCollisionStay(GameObject other)
    {
        if (other.Tag == "Enemy")
        {
            //Deal damage
            //other.GetComponent<Enemy>().life -= tickDamage;
            other.GetComponent<Enemy_Test>().life -= tickDamage;
        }
    }

    private GameObject GetPlayer()
    {
        GameObject gameObject = InternalCalls.GetGameObjectByName("Player");
        
        if (gameObject != null)
        {
            return gameObject;
        }
        else return null;
    }

}