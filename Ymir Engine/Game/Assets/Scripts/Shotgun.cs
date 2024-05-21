using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using YmirEngine;

public class Shotgun : Weapon
{
    public int ammoInChamber;
    public int dispersion;
    public Shotgun() : base(WEAPON_TYPE.SHOTGUN) { }

    public override void Start()
    {
        playerObject = InternalCalls.GetGameObjectByName("Player");
        player = playerObject.GetComponent<Player>();

        reloadTime = 2.5f;

        switch (_upgrade)
        {
            case UPGRADE.LVL_0:

                particlesGO = InternalCalls.GetChildrenByName(gameObject, "ParticlesShotgunDefault");
                ammo = 16;
                ammoInChamber = 2;
                fireRate = 1.3f;
                damage = 110; //55
                range = 10.5f;
                dispersion = 100;

                break;
            case UPGRADE.LVL_1:

                particlesGO = InternalCalls.GetChildrenByName(gameObject, "ParticlesShotgunLVL1");
                ammo = 26;
                ammoInChamber = 2;
                fireRate = 1.2f;
                damage = 140; //70
                range = 10.5f;
                dispersion = 100;

                break;
            case UPGRADE.LVL_2:

                particlesGO = InternalCalls.GetChildrenByName(gameObject, "ParticlesShotgunLVL2");
                ammo = 26;
                ammoInChamber = 2;
                fireRate = 1.2f;
                damage = 150; //75
                range = 21f;
                dispersion = 80;

                break;
            case UPGRADE.LVL_3_ALPHA:

                particlesGO = InternalCalls.GetChildrenByName(gameObject, "ParticlesShotgunLVL3A");
                ammo = 28;
                ammoInChamber = 2;
                fireRate = 0.7f;
                damage = 160; //80
                range = 21f;
                dispersion = 80;

                break;
            case UPGRADE.LVL_3_BETA:

                particlesGO = InternalCalls.GetChildrenByName(gameObject, "ParticlesShotgunLVL3B");
                ammo = 28;
                ammoInChamber = 4;
                fireRate = 1.3f;
                damage = 320; //80
                range = 21f;
                dispersion = 80;

                break;
            default:
                break;
        }

        currentAmmo = ammo;
    }

    public override void Shoot()
    {
        currentAmmo -= ammoInChamber;
        fireRateTimer = fireRate;

        Audio.PlayAudio(gameObject, "W_FSADShot");
        Particles.ParticleShoot(particlesGO, gameObject.transform.GetForward());
        Particles.PlayParticlesTrigger(particlesGO);

        Quaternion rot = gameObject.transform.globalRotation * new Quaternion(0.7071f, 0.0f, 0.0f, -0.7071f); // <- -90º Degree Quat


        InternalCalls.CreateShotgunSensor(gameObject.transform.globalPosition + offset + (gameObject.transform.GetForward() * range), rot, 70, 15, gameObject.transform.GetRight());

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

        //Audio.PlayAudio(gameObject, "W_FSADReload");
    }

    public override void StartReload()
    {
        reloading = true;
        reloadTimer = reloadTime;

        Audio.PlayAudio(gameObject, "W_FSADReload");
    }
}