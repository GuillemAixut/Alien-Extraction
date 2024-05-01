using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using YmirEngine;

public class UI_Item_Button : YmirComponent
{
    public Item item = null;
    public ITEM_SLOT currentSlot;   // Slot type --> type of item that can be placed

    public string enumItem = "";
    public string enumSlot = "";
    public string descriptionText = "";
    public string menuName = "";

    private GameObject _menuReference;
    public bool updateStats;

    // Debug
    public ITEM_SLOT itemType;
    public float HP, armor, speed, fireRate, reloadSpeed, damageMultiplier;

    public void Start()
    {
        GameObject goDescription = InternalCalls.GetChildrenByName(gameObject, "Description");
        GameObject goText = InternalCalls.GetChildrenByName(goDescription, "Text");
        descriptionText = UI.GetUIText(goText);

        _menuReference = InternalCalls.GetGameObjectByName(menuName);

        itemType = SetType(enumItem);
        currentSlot = SetType(enumSlot);

        item = new Item(currentSlot, itemType, HP, armor, speed, fireRate, reloadSpeed, damageMultiplier);
    }

    public void Update()
    {
        if (updateStats && _menuReference.GetComponent<UI_Inventory>() != null)
        {
            if (item.currentSlot != ITEM_SLOT.NONE)
            {
                UpdateStats();
                _menuReference.GetComponent<UI_Inventory>().UpdateTextStats();
            }

            updateStats = false;
        }

        return;
    }

    public void UpdateInfo()
    {
        if (_menuReference.GetComponent<UI_Inventory>().goDescription != null)
        {
            UI.TextEdit(_menuReference.GetComponent<UI_Inventory>().goText, descriptionText);
        }
    }

    private ITEM_SLOT SetType(string type)
    {
        ITEM_SLOT elementChanged = ITEM_SLOT.NONE;

        switch (type)
        {
            case "ARMOR":
                elementChanged = ITEM_SLOT.ARMOR;
                break;
            case "CHIP":
                elementChanged = ITEM_SLOT.CHIP;
                break;
            case "CONSUMABLE":
                elementChanged = ITEM_SLOT.CONSUMABLE;
                break;
            case "SAVE":
                elementChanged = ITEM_SLOT.SAVE;
                break;
            case "NONE":
                elementChanged = ITEM_SLOT.NONE;
                break;
            default:
                break;
        }

        return elementChanged;
    }
    
    private string SetInspectorType(ITEM_SLOT type) // Set values inspector when item is set
    {
        string elementChanged = " ";

        switch (type)
        {
            case ITEM_SLOT.ARMOR:
                elementChanged = "ARMOR";
                break;
            case ITEM_SLOT.CHIP:
                elementChanged = "CHIP";
                break;
            case ITEM_SLOT.CONSUMABLE:
                elementChanged = "CONSUMABLE";
                break;
            case ITEM_SLOT.SAVE:
                elementChanged = "SAVE";
                break;
            case ITEM_SLOT.NONE:
                elementChanged = "NONE";
                break;
            default:
                break;
        }

        return elementChanged;
    }

    private void UpdateStats() // TODO: cambiar cuando items funcionen en player
    {
        if (_menuReference != null)
        {
            _menuReference.GetComponent<UI_Inventory>().health.currentHealth += item.HP;
            _menuReference.GetComponent<UI_Inventory>().health.maxHealth += item.HP;
            _menuReference.GetComponent<UI_Inventory>().health.armor += item.armor;
            _menuReference.GetComponent<UI_Inventory>().player.movementSpeed += item.speed;
            _menuReference.GetComponent<UI_Inventory>().player.reloadDuration += item.reloadSpeed;
            _menuReference.GetComponent<UI_Inventory>().player.fireRate += item.fireRate;
            _menuReference.GetComponent<UI_Inventory>().player.damageMultiplier += item.damageMultiplier;
        }
    }

    public bool SetItem(Item _item)
    {
        currentSlot = SetType(enumSlot);
        itemType = SetType(enumItem);
        item = new Item(currentSlot, itemType, HP, armor, speed, fireRate, reloadSpeed, damageMultiplier);

        bool ret = false;
        Debug.Log("item currentSlot: " + item.currentSlot.ToString());
        Debug.Log("itemType: " + item.itemType.ToString());

        Debug.Log("itemType que le pasas: " + _item.itemType.ToString());
        Debug.Log("isEquipped: " + _item.isEquipped.ToString());
        Debug.Log("Rarity: " + _item.itemRarity.ToString());

        // is empty // is equipped // can be placed
        if (item.itemType == ITEM_SLOT.NONE &&
            ((_item.isEquipped && _item.itemType == item.currentSlot) ||
            item.currentSlot == ITEM_SLOT.NONE))
        {
            item = _item;
            enumSlot = SetInspectorType(item.currentSlot);
            enumItem = SetInspectorType(item.itemType);

            ret = true;

            UI.ChangeImageUI(InternalCalls.CS_GetChild(gameObject.parent, 1), item.imagePath, (int)UI_STATE.NORMAL);

            switch (item.itemRarity) // TODO: Rarity image crashes, error with meta file
            {
                case ITEM_RARITY.COMMON:
                    UI.ChangeImageUI(InternalCalls.CS_GetChild(gameObject.parent, 0), "Assets/UI/Inventory Buttons/New Buttons/Icons/AcidVesicleIconColor.png", (int)UI_STATE.NORMAL); ;
                    break;
                case ITEM_RARITY.RARE:
                    UI.ChangeImageUI(InternalCalls.CS_GetChild(gameObject.parent, 0), "Assets/UI/Inventory Buttons/New Buttons/Icons/ExocraniumIconColor.png", (int)UI_STATE.NORMAL); 
                    break;
                case ITEM_RARITY.EPIC:
                    UI.ChangeImageUI(InternalCalls.CS_GetChild(gameObject.parent, 0), "Assets/UI/Inventory Buttons/New Buttons/Icons/BoneIconColor.png", (int)UI_STATE.NORMAL); 
                    break;                
                case ITEM_RARITY.NONE:
                    UI.ChangeImageUI(InternalCalls.CS_GetChild(gameObject.parent, 0), "Assets/UI/Item Slots/Unselected.png", (int)UI_STATE.NORMAL); 
                    break;
                default:
                    break;
            }

            Debug.Log("aaa " + currentSlot.ToString() + " item: " + _item.itemType.ToString());
        }

        Debug.Log("return: " + ret.ToString());
        return ret;
    }
}