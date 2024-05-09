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
    public string enumRarity = "";
    public string menuName = "";

    private GameObject _menuReference;
    public bool updateStats = false;

    // Debug
    public string name = "";
    public ITEM_SLOT itemType;
    public ITEM_RARITY itemRarity;
    public float HP, armor, speed, fireRate, reloadSpeed, damageMultiplier;
    public bool isEquipped = false;

    public void Start()
    {
        _menuReference = InternalCalls.GetGameObjectByName(menuName);

        updateStats = false;

        if (item == null)
        {
            itemType = SetType(enumItem);
            currentSlot = SetType(enumSlot);
            itemRarity = SetRarity(enumRarity);

            isEquipped = false;
            item = CreateItemBase();
        }

        //item = new Item(currentSlot, itemType, itemRarity, isEquipped,
        //    /*name*/"Empty",
        //    /*description*/ "Empty",
        //    /*imagePath*/ "");
        //

        if (Equals(menuName, "Inventory Menu"))
        {
            _menuReference.GetComponent<UI_Inventory>().UpdateTextStats();
        }
    }

    public void Update()
    {
        if (updateStats && _menuReference.GetComponent<UI_Inventory>() != null && Equals(menuName, "Inventory Menu"))
        {
            if (item.currentSlot != ITEM_SLOT.NONE && item.currentSlot != ITEM_SLOT.SAVE)
            {
                if (!item.isEquipped)
                {
                    item.isEquipped = true;
                    item.UpdateStats();
                    _menuReference.GetComponent<UI_Inventory>().UpdateTextStats();
                }
            }
            else
            {
                if (item.isEquipped)
                {
                    item.isEquipped = false;
                    item.UpdateStats();
                    _menuReference.GetComponent<UI_Inventory>().UpdateTextStats();
                }
            }

            updateStats = false;
        }

        return;
    }

    public void UpdateInfo()
    {
        if (Equals(menuName, "Inventory Menu"))
        {
            if (_menuReference.GetComponent<UI_Inventory>().goName != null)
            {
                //Debug.Log("" + item.name);
                UI.TextEdit(_menuReference.GetComponent<UI_Inventory>().goName, item.name);
            }

            if (_menuReference.GetComponent<UI_Inventory>().goDescription != null)
            {
                //Debug.Log("" + item.description);
                UI.TextEdit(_menuReference.GetComponent<UI_Inventory>().goText, item.description);
            }
        }

        else if (Equals(menuName, "Crafting Canvas"))
        {
            if (_menuReference.GetComponent<UI_Crafting>().goName != null)
            {
                //Debug.Log("" + item.name);
                UI.TextEdit(_menuReference.GetComponent<UI_Crafting>().goName, item.name);
            }

            if (_menuReference.GetComponent<UI_Crafting>().goDescription != null)
            {
                //Debug.Log("" + item.description);
                UI.TextEdit(_menuReference.GetComponent<UI_Crafting>().goText, item.description);
            }
        }
        
        else if (Equals(menuName, "Stash Canvas"))
        {
            if (_menuReference.GetComponent<UI_Stash>().goName != null)
            {
                //Debug.Log("" + item.name);
                UI.TextEdit(_menuReference.GetComponent<UI_Stash>().goName, item.name);
            }

            if (_menuReference.GetComponent<UI_Stash>().goDescription != null)
            {
                //Debug.Log("" + item.description);
                UI.TextEdit(_menuReference.GetComponent<UI_Stash>().goText, item.description);
            }
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
            case "MATERIAL":
                elementChanged = ITEM_SLOT.MATERIAL;
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

    public ITEM_RARITY SetRarity(string type)
    {
        ITEM_RARITY elementChanged = ITEM_RARITY.NONE;

        switch (type)
        {
            case "COMMON":
                {
                    elementChanged = ITEM_RARITY.COMMON;
                }
                break;
            case "RARE":
                {
                    elementChanged = ITEM_RARITY.RARE;
                }
                break;
            case "EPIC":
                {
                    elementChanged = ITEM_RARITY.EPIC;
                }
                break;
            case "NONE":
                {
                    elementChanged = ITEM_RARITY.NONE;
                }
                break;
            default:
                break;
        }

        return elementChanged;
    }

    public string SetInspectorType(ITEM_SLOT type) // Set values inspector when item is set
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
            case ITEM_SLOT.MATERIAL:
                elementChanged = "MATERIAL";
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

    public bool SetItem(Item _item)
    {
        currentSlot = SetType(enumSlot);
        itemType = SetType(enumItem);

        if (item == null)
        {
            item = CreateItemBase();
        }

        bool ret = false;
        //Debug.Log("item currentSlot: " + item.currentSlot.ToString());
        //Debug.Log("itemType: " + item.itemType.ToString());

        //Debug.Log("itemType que le pasas: " + _item.itemType.ToString());
        //Debug.Log("isEquipped: " + _item.isEquipped.ToString());
        //Debug.Log("Rarity: " + _item.itemRarity.ToString());

        // is empty // is equipped // can be placed
        if (item.itemType == ITEM_SLOT.NONE &&
            ((_item.isEquipped && _item.itemType == item.currentSlot) ||
            item.currentSlot == ITEM_SLOT.NONE || item.currentSlot == ITEM_SLOT.MATERIAL))
        {
            item = _item;
            enumSlot = SetInspectorType(item.currentSlot);
            enumItem = SetInspectorType(item.itemType);

            ret = true;

            UI.ChangeImageUI(InternalCalls.CS_GetChild(gameObject.parent, 1), item.imagePath, (int)UI_STATE.NORMAL);

            switch (item.itemRarity)
            {
                case ITEM_RARITY.COMMON:
                    UI.ChangeImageUI(InternalCalls.CS_GetChild(gameObject.parent, 0), "Assets/UI/Items Slots/Rarities/CommonRarity.png", (int)UI_STATE.NORMAL); ;
                    break;
                case ITEM_RARITY.RARE:
                    UI.ChangeImageUI(InternalCalls.CS_GetChild(gameObject.parent, 0), "Assets/UI/Items Slots/Rarities/RareRarity.png", (int)UI_STATE.NORMAL);
                    break;
                case ITEM_RARITY.EPIC:
                    UI.ChangeImageUI(InternalCalls.CS_GetChild(gameObject.parent, 0), "Assets/UI/Items Slots/Rarities/EpicRarity.png", (int)UI_STATE.NORMAL);
                    break;
                case ITEM_RARITY.MYTHIC:
                    UI.ChangeImageUI(InternalCalls.CS_GetChild(gameObject.parent, 0), "Assets/UI/Items Slots/Rarities/MythicRarity.png", (int)UI_STATE.NORMAL);
                    break;
                case ITEM_RARITY.NONE:
                    UI.ChangeImageUI(InternalCalls.CS_GetChild(gameObject.parent, 0), "Assets/UI/Items Slots/Unselected.png", (int)UI_STATE.NORMAL);
                    break;
                default:
                    break;
            }

            //item.LogStats();

            Debug.Log("aaa " + currentSlot.ToString() + " item: " + _item.itemType.ToString());
        }

        //Debug.Log("return: " + ret.ToString());
        return ret;
    }

    public Item CreateItemBase()
    {
        Item _item;

        if (itemType == ITEM_SLOT.ARMOR || itemType == ITEM_SLOT.CHIP)
        {
            _item = new I_Equippable(currentSlot, itemType, itemRarity, isEquipped,
            /*name*/"Empty",
            /*description*/ "Empty",
            /*imagePath*/"Assets/UI/Inventory Buttons/New Buttons/Unselected.png",
            /*dictionaryName*/"",
            HP, armor, speed, fireRate, reloadSpeed, damageMultiplier);
        }

        else if (itemType == ITEM_SLOT.CONSUMABLE)
        {
            _item = new I_Consumables(currentSlot, itemType, itemRarity, isEquipped,
            /*name*/"Empty",
            /*description*/ "Assets/UI/Inventory Buttons/New Buttons/Unselected.png",
            /*imagePath*/ "");
        }

        // TODO: DEBUG MATERIAL, delete when crafting is linked to inventory
        else if (itemType == ITEM_SLOT.MATERIAL)
        {
            string path = "";
            switch (name)
            {
                case "Alien Skin":
                    path = "Assets/UI/Items Slots/Iconos/SkinIconColor.png";
                    break;
                case "Alien Exocranium":
                    path = "Assets/UI/Items Slots/Iconos/ExocraniumIconColor.png";
                    break;
                case "Alien Acid Vesicle":
                    path = "Assets/UI/Items Slots/Iconos/AcidVesicleIconColor.png";
                    break;
                case "Alien Tail tip":
                    path = "Assets/UI/Items Slots/Iconos/TailIconColor.png";
                    break;
                case "Alien Aluminium Bone":
                    path = "Assets/UI/Items Slots/Iconos/BoneIconColor.png";
                    break;
                case "Alien Claw":
                    path = "Assets/UI/Items Slots/Iconos/ClawIconColor.png";
                    break;
                default:
                    break;
            }

            _item = new Item(currentSlot, itemType, itemRarity, isEquipped,
            /*name*/name,
            /*description*/ "Empty",
            /*imagePath*/ path);

            switch (_item.itemRarity)
            {
                case ITEM_RARITY.COMMON:
                    UI.ChangeImageUI(InternalCalls.CS_GetChild(gameObject.parent, 0), "Assets/UI/Items Slots/Rarities/CommonRarity.png", (int)UI_STATE.NORMAL); ;
                    break;
                case ITEM_RARITY.RARE:
                    UI.ChangeImageUI(InternalCalls.CS_GetChild(gameObject.parent, 0), "Assets/UI/Items Slots/Rarities/RareRarity.png", (int)UI_STATE.NORMAL);
                    break;
                case ITEM_RARITY.EPIC:
                    UI.ChangeImageUI(InternalCalls.CS_GetChild(gameObject.parent, 0), "Assets/UI/Items Slots/Rarities/EpicRarity.png", (int)UI_STATE.NORMAL);
                    break;
                case ITEM_RARITY.MYTHIC:
                    UI.ChangeImageUI(InternalCalls.CS_GetChild(gameObject.parent, 0), "Assets/UI/Items Slots/Rarities/MythicRarity.png", (int)UI_STATE.NORMAL);
                    break;
                case ITEM_RARITY.NONE:
                    UI.ChangeImageUI(InternalCalls.CS_GetChild(gameObject.parent, 0), "Assets/UI/Items Slots/Unselected.png", (int)UI_STATE.NORMAL);
                    break;
                default:
                    break;
            }

            UI.ChangeImageUI(InternalCalls.CS_GetChild(gameObject.parent, 1), _item.imagePath, (int)UI_STATE.NORMAL);
        }

        else
        {
            _item = new Item(currentSlot, itemType, itemRarity, isEquipped,
            /*name*/"Empty",
            /*description*/ "Empty",
            /*imagePath*/ "Assets/UI/Inventory Buttons/New Buttons/Unselected.png");

            UI.ChangeImageUI(InternalCalls.CS_GetChild(gameObject.parent, 1), _item.imagePath, (int)UI_STATE.NORMAL); // Empty image
            UI.ChangeImageUI(InternalCalls.CS_GetChild(gameObject.parent, 0), "Assets/UI/Items Slots/Unselected.png", (int)UI_STATE.NORMAL); // No rarity
        }

        //item.LogStats();    

        return _item;
    }
}