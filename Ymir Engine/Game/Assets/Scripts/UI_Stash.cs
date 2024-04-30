using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using YmirEngine;

public class UI_Stash : YmirComponent
{
    private GameObject _selectedGO;
    public GameObject focusedGO, goDescription, goText;

    private bool _show;

    public Player player = null;
    public Health health = null;

    public void Start()
    {
        focusedGO = UI.GetFocused();
        _selectedGO = UI.GetSelected();

        goDescription = InternalCalls.GetChildrenByName(gameObject, "Item Description Image"); // TODO: ARREGLAR-HO, FER SIGUI PARE TEXT
        goText = InternalCalls.GetChildrenByName(gameObject, "Item Description Text");
        goDescription.SetActive(false);// TODO: when menu opened
        goText.SetActive(false);

        _show = false;

        GetPlayerScript();
        GetHealthScript();

        SetSlots();
    }

    public void Update()
    {
        if (player == null)
        {
            GetPlayerScript();
        }

        if (player != null && player.setHover)
        {
            Debug.Log("set first");
            goDescription.SetActive(false);// TODO: when menu opened
            goText.SetActive(false);

            player.setHover = false;
        }

        focusedGO = UI.GetFocused();// call this when menu starts or when changed, not efficient rn

        UI_Item_Button cs_UI_Item_Button = focusedGO.GetComponent<UI_Item_Button>();

        if (focusedGO != null)
        {
            if (Input.GetGamepadButton(GamePadButton.RIGHTSHOULDER) == KeyState.KEY_REPEAT)
            {
                if (!_show)
                {
                    _show = true;
                    ShowText(_show);
                }

                UpdateTextPos();
                cs_UI_Item_Button.UpdateInfo();
            }

            else if (Input.GetGamepadButton(GamePadButton.RIGHTSHOULDER) == KeyState.KEY_UP)
            {
                if (_show)
                {
                    _show = false;
                    ShowText(_show);
                }
            }

            if (Input.GetGamepadButton(GamePadButton.A) == KeyState.KEY_DOWN)
            {
                SwitchItems();
            }

            if (Input.GetGamepadButton(GamePadButton.X) == KeyState.KEY_DOWN)
            {
                UI.SetFirstFocused(gameObject);
            }

            //Debug.Log(_cs_UI_Item_Button.item.itemType.ToString());
            //Debug.Log(_cs_UI_Item_Button.item.currentSlot.ToString());
        }

        //if (Input.GetGamepadButton(GamePadButton.Y) == KeyState.KEY_DOWN)
        //{
        //    Deactivate();
        //}

        return;
    }

    private void SwitchItems()
    {
        _selectedGO = UI.GetSelected();

        if (_selectedGO != null)
        {
            //Debug.Log(_selectedGO.GetComponent<UI_Item_Button>().item.itemType.ToString());
            //Debug.Log(_selectedGO.GetComponent<UI_Item_Button>().item.currentSlot.ToString());

            if ((_selectedGO.GetComponent<UI_Item_Button>().item.itemType == focusedGO.GetComponent<UI_Item_Button>().item.currentSlot &&
                _selectedGO.GetComponent<UI_Item_Button>().item.itemType != ITEM_SLOT.NONE) ||
                (focusedGO.GetComponent<UI_Item_Button>().item.currentSlot == ITEM_SLOT.NONE && focusedGO.GetComponent<UI_Item_Button>().item.itemType == ITEM_SLOT.NONE) ||
                (focusedGO.GetComponent<UI_Item_Button>().item.currentSlot == ITEM_SLOT.SAVE && _selectedGO.GetComponent<UI_Item_Button>().item.currentSlot == ITEM_SLOT.NONE) ||
                (focusedGO.GetComponent<UI_Item_Button>().item.currentSlot == ITEM_SLOT.NONE && _selectedGO.GetComponent<UI_Item_Button>().item.currentSlot == ITEM_SLOT.NONE))
            {
                UI.SwitchPosition(_selectedGO.parent, focusedGO.parent);

                //_show = false;
                //_focusedGO.GetComponent<UI_Item_Button>().ShowInfo(_show);
                //_selectedGO.GetComponent<UI_Item_Button>().ShowInfo(_show);

                ITEM_SLOT aux = _selectedGO.GetComponent<UI_Item_Button>().item.currentSlot;
                _selectedGO.GetComponent<UI_Item_Button>().item.currentSlot = focusedGO.GetComponent<UI_Item_Button>().item.currentSlot;
                focusedGO.GetComponent<UI_Item_Button>().item.currentSlot = aux;

                focusedGO.GetComponent<UI_Item_Button>().updateStats = true;
                _selectedGO.GetComponent<UI_Item_Button>().updateStats = true;
            }

            else
            {
                // maybe error sound?
            }

            UI.SetUIState(_selectedGO, (int)UI_STATE.NORMAL);
            UI.SetUIState(focusedGO, (int)UI_STATE.NORMAL);
        }
    }

    private void SwitchMenu()
    {
        // Can't do it with names
        GameObject inventoryGO = InternalCalls.GetGameObjectByName("Inventory");

        if (InternalCalls.CompareGameObjectsByUID(InternalCalls.CS_GetParent(focusedGO), inventoryGO))
        {
            GameObject gridGO = InternalCalls.GetGameObjectByName("Grid Armor");
            UI.SetUIState(InternalCalls.CS_GetChild(gridGO, 0), (int)UI_STATE.FOCUSED);
        }

        else
        {
            UI.SetUIState(InternalCalls.CS_GetChild(inventoryGO, 0), (int)UI_STATE.FOCUSED);
        }
    }

    void UpdateTextPos()
    {
        UI.SetUIPosWithOther(goDescription, focusedGO.parent);// TODO: ARREGLAR - HO, FER SIGUI PARE TEXT
        UI.SetUIPosWithOther(goText, focusedGO.parent);
    }

    public void ShowText(bool isActive)
    {
        goDescription.SetActive(isActive);
        goText.SetActive(isActive);// TODO: ARREGLAR - HO, FER SIGUI PARE TEXT
    }

    private void GetPlayerScript()
    {
        GameObject gameObject = InternalCalls.GetGameObjectByName("Player");

        if (gameObject != null)
        {
            player = gameObject.GetComponent<Player>();
        }
    }

    private void GetHealthScript()
    {
        GameObject gameObject = InternalCalls.GetGameObjectByName("Player");

        if (gameObject != null)
        {
            health = gameObject.GetComponent<Health>();
        }
    }

    private void SetSlots()
    {
        Debug.Log("ffffffff " + player.itemsList.Count.ToString());
        for (int i = 0; i < player.itemsList.Count; i++)
        {
            Debug.Log("ccccccc " + player.itemsList[i].itemType.ToString());

            GameObject character = InternalCalls.CS_GetChild(gameObject, 1);
            GameObject inventory = InternalCalls.CS_GetChild(gameObject, 2);
            GameObject save = InternalCalls.CS_GetChild(gameObject, 3);

            for (int c = 0; c < InternalCalls.CS_GetChildrenSize(character); c++)
            {
                Debug.Log("aaaaaaaaaaaaaaaaaaaaaaaaaa");
                GameObject button = InternalCalls.CS_GetChild(InternalCalls.CS_GetChild(character, c), 2);  // (Grid (Slot (Button)))

                if (gameObject != null)
                {
                    Debug.Log("bbbb " + button.Name);
                    Debug.Log("hhhhhhh " + player.itemsList[i].itemType.ToString());
                    if (button.GetComponent<UI_Item_Button>().SetItem(player.itemsList[i]))
                    {
                        break;
                    }
                }
            }

            for (int inv = 0; inv < InternalCalls.CS_GetChildrenSize(inventory); inv++)
            {
                Debug.Log("iiiiiiiiiiii");
                GameObject button = InternalCalls.CS_GetChild(InternalCalls.CS_GetChild(inventory, inv), 2);  // (Slot (Button)))

                if (gameObject != null)
                {
                    Debug.Log("jiji " + button.Name);
                    Debug.Log("hhhhhhh " + player.itemsList[i].itemType.ToString());
                    if (button.GetComponent<UI_Item_Button>().SetItem(player.itemsList[i]))
                    {
                        break;
                    }
                }
            }

            //GameObject button = InternalCalls.CS_GetChild(InternalCalls.CS_GetChild(inventory, inv), 2);  // (Slot (Button)))

            //if (gameObject != null)
            //{
            //    Debug.Log("jiji " + button.Name);
            //    Debug.Log("hhhhhhh " + player.itemsList[i].itemType.ToString());
            //    if (button.GetComponent<UI_Item_Button>().SetItem(player.itemsList[i]))
            //    {
            //        break;
            //    }
            //}
        }
    }
}