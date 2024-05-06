using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using YmirEngine;

public class Plasma : Weapon
{

    public int upgrade = 0;

    public float damageEscalation;

    public Plasma() : base(WEAPON_TYPE.PLASMA) { }

    public void Start()
    {
        _upgrade = (UPGRADE)upgrade;

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
    }
    public override void Shoot()
    {

        GameObject target = null;

        switch (_upgrade)
        {
            case UPGRADE.LVL_0:
                target = gameObject.RaycastHit(gameObject.transform.globalPosition, gameObject.transform.GetForward(), range);
                break;
            case UPGRADE.LVL_1:
                target = gameObject.RaycastHit(gameObject.transform.globalPosition, gameObject.transform.GetForward(), range);
                break;
            case UPGRADE.LVL_2:
                target = gameObject.RaycastHit(gameObject.transform.globalPosition, gameObject.transform.GetForward(), range);
                break;
            case UPGRADE.LVL_3_ALPHA:
                target = gameObject.RaycastHit(gameObject.transform.globalPosition, gameObject.transform.GetForward(), range);
                break;
            case UPGRADE.LVL_3_BETA:
                // NEED TO ADD Piercing
                target = gameObject.RaycastHit(gameObject.transform.globalPosition, gameObject.transform.GetForward(), range);
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
                Debug.Log("[ERROR] HIT ENEMy");
                //-----------------------------------------------------------------------------------
            }
        }
    }

}