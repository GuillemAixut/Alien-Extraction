using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using YmirEngine;

public class Plasma : Weapon
{
    public float damageEscalation;

    public float currentDamage;

    public Plasma() : base(WEAPON_TYPE.PLASMA) { }

    public override void Start()
    {
        range = 200;

        switch (_upgrade)
        {
            case UPGRADE.LVL_0:

                ammo = 200;
                fireRate = 0.03f;
                damage = 2.4f;
                damageEscalation = 0.006f;
                reloadTime = 3f;

                break;
            case UPGRADE.LVL_1:

                ammo = 200;
                fireRate = 0.03f;
                damage = 3;
                damageEscalation = 0.01f;
                reloadTime = 2f;

                break;
            case UPGRADE.LVL_2:

                ammo = 300;
                fireRate = 0.02f;
                damage = 3.6f;
                damageEscalation = 0.01f;
                reloadTime = 2f;

                break;
            case UPGRADE.LVL_3_ALPHA:

                ammo = 300;
                fireRate = 0.015f;
                damage = 5f;
                damageEscalation = 0.015f;
                reloadTime = 2f;

                break;
            case UPGRADE.LVL_3_BETA:

                ammo = 200;
                fireRate = 0.02f;
                damage = 4f;
                damageEscalation = 0.01f;
                reloadTime = 2f;

                break;
            default:
                break;
        }

        currentDamage = damage;
        currentAmmo = ammo;
    } 
    public override void Shoot()
    {
        currentAmmo--;
        fireRateTimer = fireRate;

        Audio.PlayAudio(gameObject, "W_PlasmaShot");

        GameObject target = null;

        switch (_upgrade)
        {
            case UPGRADE.LVL_0:
                target = gameObject.RaycastHit(gameObject.transform.globalPosition + offset, gameObject.transform.GetForward(), range);
                break;
            case UPGRADE.LVL_1:
                target = gameObject.RaycastHit(gameObject.transform.globalPosition + offset, gameObject.transform.GetForward(), range);
                break;
            case UPGRADE.LVL_2:
                target = gameObject.RaycastHit(gameObject.transform.globalPosition + offset, gameObject.transform.GetForward(), range);
                break;
            case UPGRADE.LVL_3_ALPHA:
                target = gameObject.RaycastHit(gameObject.transform.globalPosition + offset, gameObject.transform.GetForward(), range);
                break;
            case UPGRADE.LVL_3_BETA:
                // NEED TO ADD Piercing
                target = gameObject.RaycastHit(gameObject.transform.globalPosition + offset, gameObject.transform.GetForward(), range);
                break;
            default:
                break;
        }

        if (target != null)
        {
            if (target.Tag != "Enemy")
            {
                Audio.PlayAudio(gameObject, "W_PlasmaSurf");
            }
            else
            {
                Audio.PlayAudio(gameObject, "W_PlasmaEnemy");

                //---------------Xiao: Gurrada Pendiente de Cambiar----------------------------
                FaceHuggerBaseScript aux = target.GetComponent<FaceHuggerBaseScript>();

                if (aux != null)
                {
                    aux.life -= currentDamage;
                }

                DroneXenomorphBaseScript aux2 = target.GetComponent<DroneXenomorphBaseScript>();
                if (aux2 != null)
                {
                    aux2.life -= currentDamage;
                }

                QueenXenomorphBaseScript aux3 = target.GetComponent<QueenXenomorphBaseScript>();
                if (aux3 != null)
                {
                    aux3.life -= currentDamage;
                }
                Debug.Log("[ERROR] HIT ENEMy");
                //-----------------------------------------------------------------------------------
            }
        }

        currentDamage += currentDamage * damageEscalation;
    }
    public override void Reload()
    {
        currentAmmo = ammo;

        Audio.PlayAudio(gameObject, "W_PlasmaReload");
    }

    public void ResetDamage()
    {
        currentDamage = damage;
    }

}