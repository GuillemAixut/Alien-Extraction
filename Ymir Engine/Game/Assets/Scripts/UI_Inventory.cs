using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;

using YmirEngine;

public class UI_Inventory : YmirComponent
{
    private GameObject _selectedGO;
    public GameObject focusedGO, goDescription, goText;
    public bool _droppable = true, _disable = false;
    private bool _show;
    private Player _player = null;

    public void Start()
    {
        focusedGO = UI.GetFocused();
        _selectedGO = UI.GetSelected();
        goDescription = InternalCalls.GetGameObjectByName("Item Description Image"); // TODO: ARREGLAR-HO, FER SIGUI PARE TEXT
        goText = InternalCalls.GetGameObjectByName("Item Description Text");
        goDescription.SetActive(false);// TODO: when menu opened
        goText.SetActive(false);
        _disable = false;
        _show = false;
        GetPlayerScript();
    }

    public void Update()
    {
        if (_player == null)
        {
            GetPlayerScript();
        }

        if (_player != null && _player.setHover)
        {
            Debug.Log("set first");
           //UI.SetFirstFocused(gameObject);
            _player.setHover = false;
        }

        if (Input.GetGamepadButton(GamePadButton.X) == KeyState.KEY_DOWN)
        {
            Debug.Log("set first");
            UI.SetFirstFocused(gameObject); // TODO: MissingMethodException WHY?
        }

        focusedGO = UI.GetFocused();// call this when menu starts or when changed, not efficient rn

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
                focusedGO.GetComponent<UI_Item_Button>().UpdateInfo();
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
                SwitchMenu();
            }

            if (focusedGO.GetComponent<UI_Item_Button>() != null && _droppable)
            {
                if (((focusedGO.GetComponent<UI_Item_Button>().item.itemType != ITEM_SLOT.NONE ||
                                focusedGO.GetComponent<UI_Item_Button>().item.itemType != ITEM_SLOT.SAVE) &&
                                focusedGO.GetComponent<UI_Item_Button>().item.currentSlot == ITEM_SLOT.NONE) &&
                                Input.GetGamepadButton(GamePadButton.LEFTSHOULDER) == KeyState.KEY_DOWN)
                {
                    focusedGO.GetComponent<UI_Item_Button>().item.currentSlot = ITEM_SLOT.NONE;
                    focusedGO.GetComponent<UI_Item_Button>().item.itemType = ITEM_SLOT.NONE;

                    // Add real art and other stuff
                    UI.ChangeImageUI(focusedGO, "Assets/UI/Inventory Buttons/InventorySlotUnselected.png", (int)UI_STATE.NORMAL);

                    //GameObject text = InternalCalls.GetChildrenByName(_focusedGO, "Text");
                    UI.TextEdit(InternalCalls.GetChildrenByName(focusedGO, "Text"), " ");
                }
            }

            //Debug.Log(_focusedGO.GetComponent<UI_Item_Button>().item.itemType.ToString());
            //Debug.Log(_focusedGO.GetComponent<UI_Item_Button>().item.currentSlot.ToString());
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
                focusedGO.GetComponent<UI_Item_Button>().item.currentSlot == ITEM_SLOT.SAVE)
            {
                UI.SwitchPosition(_selectedGO, focusedGO);

                //_show = false;
                //_focusedGO.GetComponent<UI_Item_Button>().ShowInfo(_show);
                //_selectedGO.GetComponent<UI_Item_Button>().ShowInfo(_show);

                ITEM_SLOT aux = _selectedGO.GetComponent<UI_Item_Button>().item.currentSlot;
                _selectedGO.GetComponent<UI_Item_Button>().item.currentSlot = focusedGO.GetComponent<UI_Item_Button>().item.currentSlot;
                focusedGO.GetComponent<UI_Item_Button>().item.currentSlot = aux;
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
        UI.SetUIPosWithOther(goDescription, focusedGO);// TODO: ARREGLAR - HO, FER SIGUI PARE TEXT
        UI.SetUIPosWithOther(goText, focusedGO);
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
            _player = gameObject.GetComponent<Player>();
        }
    }

}

