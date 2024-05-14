using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using YmirEngine;

public class SMG : Weapon
{
    public SMG() : base(WEAPON_TYPE.SMG) {}

    public override void Start()
    {

        range = 100;

        switch (_upgrade)
        {
            case UPGRADE.LVL_0:

                particlesGO = InternalCalls.GetChildrenByName(gameObject, "ParticlesSmgDefault");
                ammo = 35;
                fireRate = 0.06f;
                damage = 5;
                reloadTime = 1.8f;

                break;
            case UPGRADE.LVL_1:

                ammo = 35;
                fireRate = 0.05f;
                damage = 9;
                reloadTime = 1.6f;

                break;
            case UPGRADE.LVL_2:

                ammo = 40;
                fireRate = 0.05f;
                damage = 11;
                reloadTime = 1.5f;

                break;
            case UPGRADE.LVL_3_ALPHA:

                ammo = 110;
                fireRate = 0.04f;
                damage = 13;
                reloadTime = 1.3f;

                break;
            case UPGRADE.LVL_3_BETA:

                ammo = 40;
                fireRate = 0.02f;
                damage = 10;
                reloadTime = 1.5f;

                break;
            default:
                break;
        }

        currentAmmo = ammo;
    }

    public override void Shoot()
    {
        currentAmmo--;
        fireRateTimer = fireRate;

        Audio.PlayAudio(gameObject, "W_FirearmShot");
        Particles.ParticleShoot(particlesGO, gameObject.transform.GetForward());
        Particles.PlayParticlesTrigger(particlesGO);

        GameObject target;

        target = gameObject.RaycastHit(gameObject.transform.globalPosition + offset, gameObject.transform.GetForward(), range);

        if (target != null)
        {
            if (target.Tag != "Enemy")
            {
                Audio.PlayAudio(gameObject, "W_FirearmSurf");
            }
            else
            {
                Audio.PlayAudio(gameObject, "W_FirearmEnemy");

                //---------------Xiao: Gurrada Pendiente de Cambiar----------------------------
                FaceHuggerBaseScript aux = target.GetComponent<FaceHuggerBaseScript>();

                if (aux != null)
                {
                    aux.life -= damage;
                }

                DroneXenomorphBaseScript aux2 = target.GetComponent<DroneXenomorphBaseScript>();
                if (aux2 != null)
                {
                    aux2.life -= damage;
                }

                QueenXenomorphBaseScript aux3 = target.GetComponent<QueenXenomorphBaseScript>();
                if (aux3 != null)
                {
                    aux3.life -= damage;
                }
                Debug.Log("[ERROR] HIT ENEMY");
                //-----------------------------------------------------------------------------------
            }
        }
    }

    public override void Reload()
    {
        currentAmmo = ammo;

        Audio.PlayAudio(gameObject, "W_FirearmReload");
    }
}