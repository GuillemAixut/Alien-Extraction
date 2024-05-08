using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using YmirEngine;

public enum WEAPON_TYPE
{
    NONE = -1,

    SMG,
    SHOTGUN,
    PLASMA
}

public enum UPGRADE
{
    NONE = -1,

    LVL_0,
    LVL_1,
    LVL_2,
    LVL_3_ALPHA,
    LVL_3_BETA,
}

public abstract class Weapon : YmirComponent
{
    private string _name;

    protected WEAPON_TYPE _type;
    protected UPGRADE _upgrade;

    public int ammo;
    public float fireRate;
    public float damage;
    public float reloadTime;
    public float range;

    public int currentAmmo;
    protected float fireRateTimer;
    protected float reloadTimer;


    public Player player;
    public Weapon(WEAPON_TYPE type = WEAPON_TYPE.NONE, UPGRADE upgrade = UPGRADE.NONE)
    {
        _type = type;
        _upgrade = upgrade;

        _name = "";

        ammo = 0;
        fireRate = 0;
        damage = 0;
        reloadTime = 0;
        range = 0;

        currentAmmo = 0;
        fireRateTimer = 0;
        reloadTimer = 0;
    }

    public string Name { get { return _name; } }
    public WEAPON_TYPE Type { get { return _type; } }
    public UPGRADE Upgrade { get { return _upgrade; } }
    public abstract void Shoot();
    public abstract void Reload();

    public abstract void Start();
    public void Update()
    {
        if (fireRateTimer > 0) fireRateTimer -= Time.deltaTime;
        if (reloadTimer > 0) reloadTimer -= Time.deltaTime;

        Debug.Log("Shoot Cooldown: " + fireRateTimer);
        Debug.Log("Realod Cooldown: " + reloadTimer);
    }
    public bool ShootAvailable()
    {
        return (fireRateTimer <= 0 && currentAmmo > 0) ? true : false;
    }

    public bool ReloadAvailable()
    {
        return (reloadTimer <= 0 && currentAmmo < ammo) ? true : false;
    }
}