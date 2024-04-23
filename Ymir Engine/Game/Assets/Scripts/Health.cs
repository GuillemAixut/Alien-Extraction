using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using YmirEngine;

public class Health : YmirComponent
{
    
    //public GameObject player = null;
    public GameObject healthBar = null;
    public GameObject deathCanvas = null;
    public GameObject winCanvas = null;

    public float currentHealth = 0;
    public float maxHealth = 1200;
    public float armor = 0;
    public bool isAlive;

    public float debugDmg = 100;

    private Player player = null;

    public void Start()
    {
        debugDmg = 100;
        maxHealth = 1200;

        GetPlayerScript();

        healthBar = InternalCalls.GetGameObjectByName("Health Bar");
        deathCanvas = InternalCalls.GetGameObjectByName("Death Canvas");
        winCanvas = InternalCalls.GetGameObjectByName("Win Canvas");

        if (healthBar != null) 
        { 
            UI.SliderSetMax(healthBar, maxHealth); 
            UI.SliderEdit(healthBar, maxHealth); 
        }
        
        currentHealth = maxHealth;
        
        isAlive = true;
    }

    public void Update()
    {
        //Debug.Log("QUE cono pasa 2 " + GetCurrentHealth());
        if (player != null && player.godMode)
        {
            if (Input.GetKey(YmirKeyCode.F3) == KeyState.KEY_DOWN)
            {
                Debug.Log("Instant win");

                WinScreen();
            }

            if (Input.GetKey(YmirKeyCode.F4) == KeyState.KEY_DOWN)
            {
                Debug.Log("Instant lose");

                DeathScreen();
            }
        }
        else
        {
            if (Input.GetKey(YmirKeyCode.F5) == KeyState.KEY_DOWN)
            {
                Debug.Log("Take debug dmg");

                TakeDmg(debugDmg);
            }

            if (Input.GetKey(YmirKeyCode.F6) == KeyState.KEY_DOWN)
            {
                Debug.Log("Get debug health");

                TakeDmg(-debugDmg);

            }
        }

        return;
    }

    public void TakeDmg(float dmg)
    {
        if (player != null && !player.godMode)
        {
            currentHealth -= (dmg + armor); // reduce damage with amount of armor

            if (currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }

            else if (currentHealth <= 0)
            {
                currentHealth = 0;
                DeathScreen();
            }
            if (healthBar != null)
            {
                UI.SliderEdit(healthBar, currentHealth);
            }

        }
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    public bool DeathScreen()
    {
        if (deathCanvas != null) { deathCanvas.SetActive(true); }
        if (player != null) { player.gameObject.SetActive(false); }
        isAlive = false;
        Audio.StopAllAudios();

        return true;
    }

    public bool WinScreen()
    {
        if (winCanvas != null) { winCanvas.SetActive(true); }
        if (player != null) { player.gameObject.SetActive(false); }

        return true;
    }

    private void GetPlayerScript()
    {
        GameObject gameObject = InternalCalls.GetGameObjectByName("Player");

        if (gameObject != null)
        {
            player = gameObject.GetComponent<Player>();
        }
    }

    //private void GetAnotherScript()
    //{
    //    GameObject gameObject = InternalCalls.GetGameObjectByName("Player");

    //    if (gameObject != null)
    //    {
    //        PlayerMovement p = gameObject.GetComponent<PlayerMovement>();

    //        //Debug.Log("MovmentSpeed= " + movementSpeed);

    //    }
    //}
}