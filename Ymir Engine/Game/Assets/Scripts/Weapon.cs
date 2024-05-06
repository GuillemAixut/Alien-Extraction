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
    }

    public string Name { get { return _name; } }
    public WEAPON_TYPE Type { get { return _type; } }
    public UPGRADE Upgrade { get { return _upgrade; } }
    public abstract void Shoot();

}