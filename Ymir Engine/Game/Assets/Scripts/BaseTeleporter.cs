using System.Collections.Generic;
using System.Linq;
using YmirEngine;

public enum LEVELS
{
    WAREHOUSE,
    LAB,
    HATCHERY,

    NONE
}

public class BaseTeleporter : YmirComponent
{
    public LEVELS selectedLvl = LEVELS.NONE;
    public WEAPON_TYPE selectedWeapon = WEAPON_TYPE.NONE;

    public GameObject canvas, button, lvlText, weaponText;
    private Player csPlayer = null;

    private bool _setNormal = false;

    private GameObject _grid;

    public void Start()
    {
        button = InternalCalls.GetGameObjectByName("Go to raid");
        lvlText = InternalCalls.GetGameObjectByName("Lvl description");
        weaponText = InternalCalls.GetGameObjectByName("Weapon description");

        GetPlayerScript();

        canvas = InternalCalls.GetGameObjectByName("Level Selection Canvas");

        selectedLvl = LEVELS.NONE;
        selectedWeapon = WEAPON_TYPE.NONE;

        _grid = InternalCalls.GetGameObjectByName("Grid");
    }

    public void Update()
    {
        // TODO: delete this
        if (Input.GetGamepadButton(GamePadButton.X) == KeyState.KEY_DOWN)
        {
            UI.SetFirstFocused(gameObject);
        }

        if (Input.GetGamepadButton(GamePadButton.B) == KeyState.KEY_DOWN)
        {
            csPlayer.PlayerStopState(false);
            canvas.SetActive(false);
        }

        if (!_setNormal && selectedLvl != LEVELS.NONE && selectedWeapon != WEAPON_TYPE.NONE)
        {
            Debug.Log("Lvl: " + selectedLvl.ToString() + " Weapon: " + selectedWeapon.ToString());

            UI.SetUIState(button, (int)UI_STATE.NORMAL);
            _grid.GetComponent<UI_Inventory_Grid>().naviagteY = true;

            _setNormal = true;

            switch (selectedLvl)
            {
                case LEVELS.WAREHOUSE:
                    {
                        button.GetComponent<Button_Navigation>().sceneName = "LVL1_FINAL/LVL1_FINAL_COLLIDERS";
                    }
                    break;

                case LEVELS.LAB:
                    {
                         button.GetComponent<Button_Navigation>().sceneName = "LVL2_LAB_PART1_FINAL/LVL2_LAB_PART1_COLLIDERS";
                    }
                    break;

                case LEVELS.HATCHERY:
                    {
                         button.GetComponent<Button_Navigation>().sceneName = "LVL3_BlockOut/LVL3_PART1_COLLIDERS";
                    }
                    break;
            }

            Debug.Log("scene: " + button.GetComponent<Button_Navigation>().sceneName);
        }
        else if ((UI_STATE)UI.GetUIState(button) != UI_STATE.DISABLED &&
            (selectedLvl == LEVELS.NONE || selectedWeapon == WEAPON_TYPE.NONE))
        {
            Debug.Log("Lvl: " + selectedLvl.ToString() + " Weapon: " + selectedWeapon.ToString());

            UI.SetUIState(button, (int)UI_STATE.DISABLED);
            _grid.GetComponent<UI_Inventory_Grid>().naviagteY = false;

            _setNormal = false;
        }

        return;
    }

    private void GetPlayerScript()
    {
        GameObject gameObject = InternalCalls.GetGameObjectByName("Player");

        if (gameObject != null)
        {
            csPlayer = gameObject.GetComponent<Player>();
        }
    }
}