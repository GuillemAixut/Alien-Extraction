using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using YmirEngine;

public class UI_Inventory : YmirComponent
{
    private GameObject _selectedGO;
    public GameObject _focusedGO;
    public bool _droppable = true;
    private bool _show;

    public void Start()
    {
        _focusedGO = UI.GetFocused();
        _selectedGO = UI.GetSelected();
        _show = false;
    }

    public void Update()
    {
        _focusedGO = UI.GetFocused();// call this when menu starts or when changed, not efficient rn

        if (_focusedGO != null)
        {
            if (Input.GetGamepadButton(GamePadButton.RIGHTSHOULDER) == KeyState.KEY_REPEAT)
            {
                if (!_show)
                {
                    _show = true;
                    _focusedGO.GetComponent<UI_Item_Button>().ShowInfo(_show);
                }
            }

            else if (Input.GetGamepadButton(GamePadButton.RIGHTSHOULDER) == KeyState.KEY_UP)
            {
                if (_show)
                {
                    _show = false;
                    _focusedGO.GetComponent<UI_Item_Button>().ShowInfo(_show);
                }
            }

            if (Input.GetGamepadButton(GamePadButton.A) == KeyState.KEY_DOWN)
            {
                SwitchItems();
            }

            if (Input.GetGamepadButton(GamePadButton.X) == KeyState.KEY_DOWN)
            {
                SwitchMenu();
            }

            if (_focusedGO.GetComponent<UI_Item_Button>() != null && _droppable)
            {
                if (((_focusedGO.GetComponent<UI_Item_Button>().item.itemType != ITEM_SLOT.NONE ||
                                _focusedGO.GetComponent<UI_Item_Button>().item.itemType != ITEM_SLOT.SAVE) &&
                                _focusedGO.GetComponent<UI_Item_Button>().item.currentSlot == ITEM_SLOT.NONE) &&
                                Input.GetGamepadButton(GamePadButton.LEFTSHOULDER) == KeyState.KEY_DOWN)
                {
                    _focusedGO.GetComponent<UI_Item_Button>().item.currentSlot = ITEM_SLOT.NONE;
                    _focusedGO.GetComponent<UI_Item_Button>().item.itemType = ITEM_SLOT.NONE;

                    // Add real art and other stuff
                    UI.ChangeImageUI(_focusedGO, "Assets/UI/Inventory Buttons/InventorySlotUnselected.png", (int)UI_STATE.NORMAL);

                    //GameObject text = InternalCalls.GetChildrenByName(_focusedGO, "Text");
                    UI.TextEdit(InternalCalls.GetChildrenByName(_focusedGO, "Text"), " ");
                }
            }
           
            Debug.Log(_focusedGO.GetComponent<UI_Item_Button>().item.itemType.ToString());
            Debug.Log(_focusedGO.GetComponent<UI_Item_Button>().item.currentSlot.ToString());
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

            if ((_selectedGO.GetComponent<UI_Item_Button>().item.itemType == _focusedGO.GetComponent<UI_Item_Button>().item.currentSlot &&
                _selectedGO.GetComponent<UI_Item_Button>().item.itemType != ITEM_SLOT.NONE) ||
                (_focusedGO.GetComponent<UI_Item_Button>().item.currentSlot == ITEM_SLOT.NONE && _focusedGO.GetComponent<UI_Item_Button>().item.itemType == ITEM_SLOT.NONE) ||
                _focusedGO.GetComponent<UI_Item_Button>().item.currentSlot == ITEM_SLOT.SAVE)
            {
                UI.SwitchPosition(_selectedGO, _focusedGO);

                _show = false;
                _focusedGO.GetComponent<UI_Item_Button>().ShowInfo(_show);
                _selectedGO.GetComponent<UI_Item_Button>().ShowInfo(_show);

                ITEM_SLOT aux = _selectedGO.GetComponent<UI_Item_Button>().item.currentSlot;
                _selectedGO.GetComponent<UI_Item_Button>().item.currentSlot = _focusedGO.GetComponent<UI_Item_Button>().item.currentSlot;
                _focusedGO.GetComponent<UI_Item_Button>().item.currentSlot = aux;
            }

            else
            {
                // maybe error sound?
            }

            UI.SetUIState(_selectedGO, (int)UI_STATE.NORMAL);
        }
    }

    private void SwitchMenu()
    {
        // Can't do it with names
        GameObject inventoryGO = InternalCalls.GetGameObjectByName("Inventory");

        if (InternalCalls.CompareGameObjectsByUID(InternalCalls.CS_GetParent(_focusedGO), inventoryGO))
        {
            GameObject gridGO = InternalCalls.GetGameObjectByName("Grid Armor");
            UI.SetUIState(InternalCalls.CS_GetChild(gridGO, 0), (int)UI_STATE.FOCUSED);
        }

        else
        {
            UI.SetUIState(InternalCalls.CS_GetChild(inventoryGO, 0), (int)UI_STATE.FOCUSED);
        }
    }

    public void Deactivate()
    {
        // TODO: CANVIAR ESTA BASURA

        Debug.Log("aaaaaaaaaaaaa");

        GameObject inventoryGO = InternalCalls.GetGameObjectByName("Inventory");
        UI.SetActiveAllUI(inventoryGO, false);

        GameObject armor = InternalCalls.GetGameObjectByName("Armor");
        armor.GetComponent<UI_Item_Button>().ShowInfo(false);

        GameObject chip1 = InternalCalls.GetGameObjectByName("Chip1");
        chip1.GetComponent<UI_Item_Button>().ShowInfo(false);

        GameObject chip2 = InternalCalls.GetGameObjectByName("Chip2");
        chip2.GetComponent<UI_Item_Button>().ShowInfo(false);

        GameObject consumable1 = InternalCalls.GetGameObjectByName("Consumable1");
        consumable1.GetComponent<UI_Item_Button>().ShowInfo(false);

        GameObject consumable2 = InternalCalls.GetGameObjectByName("Consumable2");
        consumable2.GetComponent<UI_Item_Button>().ShowInfo(false);

        GameObject save = InternalCalls.GetGameObjectByName("Save Item");
        save.GetComponent<UI_Item_Button>().ShowInfo(false);
    }
}