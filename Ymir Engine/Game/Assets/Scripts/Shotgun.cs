using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using YmirEngine;

public class Shotgun : Weapon
{

    public int upgrade = 0;

    public int ammoInChamber;
    public int dispersion;
    public Shotgun() : base(WEAPON_TYPE.SHOTGUN) { }

    public void Start()
    {
        _upgrade = (UPGRADE)upgrade;

        switch (_upgrade)
        {
            case UPGRADE.LVL_0:

                ammo = 16;
                ammoInChamber = 2;
                fireRate = 1.3f;
                damage = 55;
                reloadTime = 2.7f;
                range = 10.5f;
                dispersion = 100;

                break;
            case UPGRADE.LVL_1:

                ammo = 26;
                ammoInChamber = 2;
                fireRate = 1.2f;
                damage = 70;
                reloadTime = 2.6f;
                range = 10.5f;
                dispersion = 100;

                break;
            case UPGRADE.LVL_2:

                ammo = 26;
                ammoInChamber = 2;
                fireRate = 1.2f;
                damage = 75;
                reloadTime = 2.5f;
                range = 21f;
                dispersion = 80;

                break;
            case UPGRADE.LVL_3_ALPHA:

                ammo = 28;
                ammoInChamber = 2;
                fireRate = 0.7f;
                damage = 80;
                reloadTime = 2.1f;
                range = 21f;
                dispersion = 80;

                break;
            case UPGRADE.LVL_3_BETA:

                ammo = 28;
                ammoInChamber = 4;
                fireRate = 1.3f;
                damage = 80;
                reloadTime = 2.5f;
                range = 21;
                dispersion = 80;

                break;
            default:
                break;
        }
    }
    public override void Shoot()
    {
        currentAmmo--;

        Quaternion rot = gameObject.transform.globalRotation * new Quaternion(0.7071f, 0.0f, 0.0f, -0.7071f); // <- -90º Degree Quat

        InternalCalls.CreateShotgunSensor(gameObject.transform.globalPosition, rot, gameObject.transform.GetRight());

        switch (_upgrade)
        {
            case UPGRADE.LVL_0:

                break;
            case UPGRADE.LVL_1:

                break;
            case UPGRADE.LVL_2:

                break;
            case UPGRADE.LVL_3_ALPHA:

                break;
            case UPGRADE.LVL_3_BETA:

                break;
            default:
                break;
        }
    }
    public override void Reload()
    {
        currentAmmo = ammo;

        Audio.PlayAudio(gameObject, "W_FSADReload");
    }
}