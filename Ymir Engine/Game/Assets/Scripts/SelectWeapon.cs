using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using YmirEngine;

public class SelectWeapon : YmirComponent
{
    public int weapon = 0;  // TODO: Sara --> change to enum
    private BaseTeleporter levelSelector;

    public void Start()
    {
        GameObject gameObject = InternalCalls.GetGameObjectByName("Background");
        if (gameObject != null)
        {
            levelSelector = gameObject.GetComponent<BaseTeleporter>();
        }
    }

    public void Update()
    {
        return;
    }

    public void OnClickButton()
    {
        GameObject gameObject = InternalCalls.GetGameObjectByName("Lvl (" + ((int)levelSelector.selectedLvl + 1) + ")");
        GameObject gameObject1 = InternalCalls.GetGameObjectByName("Weapon (" + ((int)levelSelector.selectedWeapon + 1) + ")");

        //Debug.Log("Weapon (" + ((int)levelSelector.selectedWeapon + 1) + ")");

        LEVELS selectedLvlPrev = levelSelector.selectedLvl;
        WEAPON_TYPE selectedWeaponPrev = levelSelector.selectedWeapon;

        if (levelSelector.selectedWeapon != (WEAPON_TYPE)weapon)
        {
            levelSelector.selectedWeapon = (WEAPON_TYPE)weapon;
        }
        else
        {
            levelSelector.selectedWeapon = WEAPON_TYPE.NONE;
        }

        if (gameObject != null/* && levelSelector.selectedLvl != selectedLvlPrev*/)
        {
            switch (selectedLvlPrev)
            {
                case LEVELS.NONE:
                    {
                        break;
                    }
                case LEVELS.WAREHOUSE:
                    {
                        UI.ChangeImageUI(gameObject, "Assets/UI/Teleport Buttons/BotonHover.png", (int)UI_STATE.NORMAL);
                        break;
                    }
                case LEVELS.LAB:
                    {
                        UI.ChangeImageUI(gameObject, "Assets/UI/Teleport Buttons/BotonHover.png", (int)UI_STATE.NORMAL);
                        break;
                    }
                case LEVELS.HATCHERY:
                    {
                        UI.ChangeImageUI(gameObject, "Assets/UI/Teleport Buttons/BotonHover.png", (int)UI_STATE.NORMAL);
                        break;
                    }
            }
        }

        if (gameObject1 != null)
        {
            switch (selectedWeaponPrev)
            {
                case WEAPON_TYPE.NONE:
                    {
                        break;
                    }
                case WEAPON_TYPE.SMG:
                    {
                        UI.ChangeImageUI(gameObject1, "Assets/UI/Teleport Buttons/SmgUnselected.png", (int)UI_STATE.NORMAL);
                        break;
                    }
                case WEAPON_TYPE.SHOTGUN:
                    {
                        UI.ChangeImageUI(gameObject1, "Assets/UI/Teleport Buttons/ShotgunUnselected.png", (int)UI_STATE.NORMAL);
                        break;
                    }
                case WEAPON_TYPE.PLASMA:
                    {
                        UI.ChangeImageUI(gameObject1, "Assets/UI/Teleport Buttons/LaserUnselected.png", (int)UI_STATE.NORMAL);
                        break;
                    }
            }
        }

        Debug.Log("Weapon: " + levelSelector.selectedWeapon.ToString());

        //UI.TextEdit(levelSelector.weaponText, levelSelector.weaponDescriptions[(int)levelSelector.selectedWeapon]);
    }

    public void OnHoverButton()
    {
        if (levelSelector != null)
        {
            switch ((WEAPON_TYPE)weapon)
            {
                case WEAPON_TYPE.NONE:
                    {
                        UI.TextEdit(levelSelector.weaponText, "");
                    }
                    break;
                case WEAPON_TYPE.SMG:
                    {
                        UI.TextEdit(levelSelector.weaponText, "The “S.T.A.P.L.E.R.” is a fast submachine \ngun capable of blasting multiple \nlow damage bullets in less than \na second, perfect for a fast cleaning.");
                    }
                    break;
                case WEAPON_TYPE.SHOTGUN:
                    {
                        UI.TextEdit(levelSelector.weaponText, "The “Spar-X” is a shotgun that allows \nyou to eliminate multiple enemies in \none shot, perfect for close-distance combat.");
                    }
                    break;
                case WEAPON_TYPE.PLASMA:
                    {
                        UI.TextEdit(levelSelector.weaponText, "The “EM-MK2” is a plasma weapon capable\nof firing a constant beam of lethal \nenergy, ideal for suppressing enemies\n in medium and long-range combat. ");
                    }
                    break;

            }
        }
    }
}