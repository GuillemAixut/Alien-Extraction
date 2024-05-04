using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;

using YmirEngine;

public class UI_Inventory : YmirComponent
{
    private GameObject _selectedGO, _textHP, _textArmor, _textSpeed, _textReload, _textDamage, _textRate, _textResin;
    public GameObject focusedGO, goDescription, goText, goName;

    private bool _show;

    public Player player = null;
    public Health health = null;

    // Items dictionary
    static public Dictionary<string, Item> itemsDictionary = new Dictionary<string, Item>();

    public void Start()
    {
        CreateItemDictionary();

        focusedGO = UI.GetFocused();
        _selectedGO = UI.GetSelected();

        goDescription = InternalCalls.GetChildrenByName(gameObject, "Item Description Image"); // TODO: ARREGLAR-HO, FER SIGUI PARE TEXT

        goText = InternalCalls.GetChildrenByName(gameObject, "Item Description Text");
        goName = InternalCalls.GetChildrenByName(gameObject, "Item Description Name");
        goDescription.SetActive(false);// TODO: when menu opened

        goText.SetActive(false);
        goName.SetActive(false);

        _show = false;

        GetPlayerScript();
        GetHealthScript();

        _textHP = InternalCalls.GetGameObjectByName("Text HP");
        _textArmor = InternalCalls.GetGameObjectByName("Text Armor");
        _textSpeed = InternalCalls.GetGameObjectByName("Text Speed");
        _textReload = InternalCalls.GetGameObjectByName("Text Reload");
        _textDamage = InternalCalls.GetGameObjectByName("Text Damage");
        _textRate = InternalCalls.GetGameObjectByName("Text Rate");
        _textResin = InternalCalls.GetGameObjectByName("Text Resin");

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
            goName.SetActive(false);

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

            if (cs_UI_Item_Button != null)
            {
                // Si se quita peta xd
                cs_UI_Item_Button.item.itemType.ToString();
                //cs_UI_Item_Button.item.currentSlot.ToString();
                //

                if (((cs_UI_Item_Button.item.itemType != ITEM_SLOT.NONE ||
                                cs_UI_Item_Button.item.itemType != ITEM_SLOT.SAVE) &&
                                cs_UI_Item_Button.item.currentSlot == ITEM_SLOT.NONE) &&
                                Input.GetGamepadButton(GamePadButton.LEFTSHOULDER) == KeyState.KEY_DOWN)
                {
                    cs_UI_Item_Button.item.currentSlot = ITEM_SLOT.NONE;
                    cs_UI_Item_Button.item.itemType = ITEM_SLOT.NONE;

                    // Add real art and other stuff

                    GameObject imageItem = InternalCalls.GetChildrenByName(focusedGO.parent, "Image Item");

                    UI.ChangeImageUI(imageItem, "Assets/UI/Inventory Buttons/New Buttons/Unselected.png", (int)UI_STATE.NORMAL);

                    cs_UI_Item_Button.item.description = "Empty";
                    cs_UI_Item_Button.item.name = "Empty";
                    cs_UI_Item_Button.UpdateInfo();
                }
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
            //UI.SetUIState(focusedGO, (int)UI_STATE.NORMAL);
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

    void UpdateTextPos() // Place the descrition game object on the selected GO
    {
        UI.SetUIPosWithOther(goDescription, focusedGO.parent);// TODO: ARREGLAR - HO, FER SIGUI PARE TEXT
        UI.SetUIPosWithOther(goText, focusedGO.parent);
        UI.SetUIPosWithOther(goName, focusedGO.parent);
    }

    public void ShowText(bool isActive) // Show description of item when pressing R1
    {
        goDescription.SetActive(isActive);
        goText.SetActive(isActive);// TODO: ARREGLAR - HO, FER SIGUI PARE TEXT
        goName.SetActive(isActive);
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

    public void UpdateTextStats() // Print player info on the screen
    {
        if (player != null)
        {
            UI.TextEdit(_textSpeed, player.movementSpeed.ToString());
            UI.TextEdit(_textRate, player.fireRate.ToString());
            UI.TextEdit(_textReload, player.reloadDuration.ToString());
            UI.TextEdit(_textDamage, player.damageMultiplier.ToString());
            UI.TextEdit(_textResin, player.resin.ToString());
        }

        if (health != null)
        {
            UI.TextEdit(_textHP, health.currentHealth.ToString());
            UI.TextEdit(_textArmor, health.armor.ToString());
        }
    }

    private void SetSlots() // Place the items from player to inventory
    {
        bool isInventory; 
        Debug.Log("SetSlots: ");

        for (int i = 0; i < player.itemsList.Count; i++)
        {
            isInventory = true;

            GameObject character = InternalCalls.CS_GetChild(gameObject, 1);
            GameObject inventory = InternalCalls.CS_GetChild(gameObject, 2);

            for (int c = 0; c < InternalCalls.CS_GetChildrenSize(character); c++)
            {
                GameObject button = InternalCalls.CS_GetChild(InternalCalls.CS_GetChild(InternalCalls.CS_GetChild(character, c), 0), 2);  // (Grid (Slot (Button)))
                Debug.Log("button name " + button.Name);

                if (button != null)
                {
                    if (button.GetComponent<UI_Item_Button>().SetItem(player.itemsList[i]))
                    {
                        isInventory = false;
                        break;
                    }
                }
            }

            if (isInventory)
            {
                for (int inv = 0; inv < InternalCalls.CS_GetChildrenSize(inventory); inv++)
                {
                    GameObject button = InternalCalls.CS_GetChild(InternalCalls.CS_GetChild(inventory, inv), 2);  // (Slot (Button)))

                    if (button != null)
                    {
                        if (button.GetComponent<UI_Item_Button>().SetItem(player.itemsList[i]))
                        {
                            break;
                        }
                    }
                }
            }
        }
    }

    #region DEFINE ITEMS

    private void CreateItemDictionary()
    {
        itemsDictionary.Add("skin_common", new
            Item(
            /*always none*/     ITEM_SLOT.NONE,
            /*item type*/       ITEM_SLOT.MATERIAL,
            /*itemRarity*/      ITEM_RARITY.COMMON,
            /*isEquipped*/      false,
            /*name*/            "Common Alien Skin",
            /*description*/     "It's made of a tough material, capable of resisting the creatures own acid",
            /*imagePath*/       "Assets/UI/Items Slots/Iconos/SkinIconColor.png"
                ));

        itemsDictionary.Add("skin_rare", new
           Item(
           /*always none*/     ITEM_SLOT.NONE,
           /*item type*/       ITEM_SLOT.MATERIAL,
           /*itemRarity*/      ITEM_RARITY.RARE,
           /*isEquipped*/      false,
           /*name*/            "Rare Alien Skin",
           /*description*/     "It's made of a tough material, capable of resisting the creatures own acid",
           /*imagePath*/       "Assets/UI/Items Slots/Iconos/SkinIconColor.png"
               ));

        itemsDictionary.Add("skin_epic", new
            Item(
            /*always none*/     ITEM_SLOT.NONE,
            /*item type*/       ITEM_SLOT.MATERIAL,
            /*itemRarity*/      ITEM_RARITY.EPIC,
            /*isEquipped*/      false,
            /*name*/            "Epic Alien Skin",
            /*description*/     "It's made of a tough material, capable of resisting the creatures own acid",
            /*imagePath*/       "Assets/UI/Items Slots/Iconos/SkinIconColor.png"
                ));

        itemsDictionary.Add("claw_common", new
            Item(
            /*always none*/     ITEM_SLOT.NONE,
            /*item type*/       ITEM_SLOT.MATERIAL,
            /*itemRarity*/      ITEM_RARITY.COMMON,
            /*isEquipped*/      false,
            /*name*/            "Common Alien Claw",
            /*description*/     "Shiny, black and extremely sharp claw, capable of slicing through almost anything within its grasp",
            /*imagePath*/       "Assets/UI/Items Slots/Iconos/ClawIconColor.png"
                ));

        itemsDictionary.Add("claw_rare", new
            Item(
            /*always none*/     ITEM_SLOT.NONE,
            /*item type*/       ITEM_SLOT.MATERIAL,
            /*itemRarity*/      ITEM_RARITY.RARE,
            /*isEquipped*/      false,
            /*name*/            "Rare Alien Claw",
            /*description*/     "Shiny, black and extremely sharp claw, capable of slicing through almost anything within its grasp",
            /*imagePath*/       "Assets/UI/Items Slots/Iconos/ClawIconColor.png"
                ));

        itemsDictionary.Add("claw_epic", new
            Item(
            /*always none*/     ITEM_SLOT.NONE,
            /*item type*/       ITEM_SLOT.MATERIAL,
            /*itemRarity*/      ITEM_RARITY.EPIC,
            /*isEquipped*/      false,
            /*name*/            "Epic Alien Claw",
            /*description*/     "Shiny, black and extremely sharp claw, capable of slicing through almost anything within its grasp",
            /*imagePath*/       "Assets/UI/Items Slots/Iconos/ClawIconColor.png"
                ));

        itemsDictionary.Add("tailtip_common", new
            Item(
            /*always none*/     ITEM_SLOT.NONE,
            /*item type*/       ITEM_SLOT.MATERIAL,
            /*itemRarity*/      ITEM_RARITY.COMMON,
            /*isEquipped*/      false,
            /*name*/            "Common Alien Tail Tip",
            /*description*/     "A sharp metallic piece of the tail, they use it as a slicing weapon",
            /*imagePath*/       "Assets/UI/Items Slots/Iconos/TailIconColor.png"
                ));

        itemsDictionary.Add("tailtip_rare", new
            Item(
            /*always none*/     ITEM_SLOT.NONE,
            /*item type*/       ITEM_SLOT.MATERIAL,
            /*itemRarity*/      ITEM_RARITY.RARE,
            /*isEquipped*/      false,
            /*name*/            "Rare Alien Tail Tip",
            /*description*/     "A sharp metallic piece of the tail, they use it as a slicing weapon",
            /*imagePath*/       "Assets/UI/Items Slots/Iconos/TailIconColor.png"
                ));

        itemsDictionary.Add("tailtip_epic", new
            Item(
            /*always none*/     ITEM_SLOT.NONE,
            /*item type*/       ITEM_SLOT.MATERIAL,
            /*itemRarity*/      ITEM_RARITY.EPIC,
            /*isEquipped*/      false,
            /*name*/            "Epic Alien Tail Tip",
            /*description*/     "A sharp metallic piece of the tail, they use it as a slicing weapon",
            /*imagePath*/       "Assets/UI/Items Slots/Iconos/TailIconColor.png"
                ));

        itemsDictionary.Add("acidvesicle_common", new
            Item(
            /*always none*/     ITEM_SLOT.NONE,
            /*item type*/       ITEM_SLOT.MATERIAL,
            /*itemRarity*/      ITEM_RARITY.COMMON,
            /*isEquipped*/      false,
            /*name*/            "Common Alien Acid Vesicle",
            /*description*/     "Acid-filled organic bags, can be useful for some crafts",
            /*imagePath*/       "Assets/UI/Items Slots/Iconos/AcidVesicleIconColor.png"
                ));

        itemsDictionary.Add("acidvesicle_rare", new
            Item(
            /*always none*/     ITEM_SLOT.NONE,
            /*item type*/       ITEM_SLOT.MATERIAL,
            /*itemRarity*/      ITEM_RARITY.RARE,
            /*isEquipped*/      false,
            /*name*/            "Rare Alien Acid Vesicle",
            /*description*/     "Acid-filled organic bags, can be useful for some crafts",
            /*imagePath*/       "Assets/UI/Items Slots/Iconos/AcidVesicleIconColor.png"
                ));

        itemsDictionary.Add("acidvesicle_epic", new
            Item(
            /*always none*/     ITEM_SLOT.NONE,
            /*item type*/       ITEM_SLOT.MATERIAL,
            /*itemRarity*/      ITEM_RARITY.EPIC,
            /*isEquipped*/      false,
            /*name*/            "Epic Alien Acid Vesicle",
            /*description*/     "Acid-filled organic bags, can be useful for some crafts",
            /*imagePath*/       "Assets/UI/Items Slots/Iconos/AcidVesicleIconColor.png"
                ));

        itemsDictionary.Add("exocranium_common", new
            Item(
            /*always none*/     ITEM_SLOT.NONE,
            /*item type*/       ITEM_SLOT.MATERIAL,
            /*itemRarity*/      ITEM_RARITY.COMMON,
            /*isEquipped*/      false,
            /*name*/            "Common Alien Exocranium",
            /*description*/     "An elongated shell. Hard, resistant and light. Sometimes containing some vesicles on the sides",
            /*imagePath*/       "Assets/UI/Items Slots/Iconos/ExocraniumIconColor.png"
                ));

        itemsDictionary.Add("exocranium_rare", new
            Item(
            /*always none*/     ITEM_SLOT.NONE,
            /*item type*/       ITEM_SLOT.MATERIAL,
            /*itemRarity*/      ITEM_RARITY.RARE,
            /*isEquipped*/      false,
            /*name*/            "Rare Alien Exocranium",
            /*description*/     "An elongated shell. Hard, resistant and light. Sometimes containing some vesicles on the sides",
            /*imagePath*/       "Assets/UI/Items Slots/Iconos/ExocraniumIconColor.png"
                ));

        itemsDictionary.Add("exocranium_epic", new
            Item(
            /*always none*/     ITEM_SLOT.NONE,
            /*item type*/       ITEM_SLOT.MATERIAL,
            /*itemRarity*/      ITEM_RARITY.EPIC,
            /*isEquipped*/      false,
            /*name*/            "Epic Alien Exocranium",
            /*description*/     "An elongated shell. Hard, resistant and light. Sometimes containing some vesicles on the sides",
            /*imagePath*/       "Assets/UI/Items Slots/Iconos/ExocraniumIconColor.png"
                ));

        itemsDictionary.Add("bone_common", new
            Item(
            /*always none*/     ITEM_SLOT.NONE,
            /*item type*/       ITEM_SLOT.MATERIAL,
            /*itemRarity*/      ITEM_RARITY.COMMON,
            /*isEquipped*/      false,
            /*name*/            "Common Alien Aluminum Bone",
            /*description*/     "It is light, soft, malleable, and a good conductor of both electricity and heat",
            /*imagePath*/       "Assets/UI/Items Slots/Iconos/BoneIconColor.png"
                ));

        itemsDictionary.Add("bone_rare", new
            Item(
            /*always none*/     ITEM_SLOT.NONE,
            /*item type*/       ITEM_SLOT.MATERIAL,
            /*itemRarity*/      ITEM_RARITY.RARE,
            /*isEquipped*/      false,
            /*name*/            "Rare Alien Aluminum Bone",
            /*description*/     "It is light, soft, malleable, and a good conductor of both electricity and heat",
            /*imagePath*/       "Assets/UI/Items Slots/Iconos/BoneIconColor.png"
                ));

        itemsDictionary.Add("bone_epic", new
            Item(
            /*always none*/     ITEM_SLOT.NONE,
            /*item type*/       ITEM_SLOT.MATERIAL,
            /*itemRarity*/      ITEM_RARITY.EPIC,
            /*isEquipped*/      false,
            /*name*/            "Epic Alien Aluminum Bone",
            /*description*/     "It is light, soft, malleable, and a good conductor of both electricity and heat",
            /*imagePath*/       "Assets/UI/Items Slots/Iconos/BoneIconColor.png"
                ));

        itemsDictionary.Add("core_mythic", new
            Item(
            /*always none*/     ITEM_SLOT.NONE,
            /*item type*/       ITEM_SLOT.MATERIAL,
            /*itemRarity*/      ITEM_RARITY.MYTHIC,
            /*isEquipped*/      false,
            /*name*/            "Mythical Alien Core",
            /*description*/     "An amazing but unknown part of the alien, it can be used to upgrade your weapon",
            /*imagePath*/       "Assets/UI/Items Slots/Iconos/AlienCoreIconColor.png"
                ));

        itemsDictionary.Add("upgradevessel_mythic", new
            Item(
            /*always none*/     ITEM_SLOT.NONE,
            /*item type*/       ITEM_SLOT.MATERIAL,
            /*itemRarity*/      ITEM_RARITY.MYTHIC,
            /*isEquipped*/      false,
            /*name*/            "Resin Vessel",
            /*description*/     "Adds one more Resin Vessel when crafted, allowing you to get more heals on the raid",
            /*imagePath*/       "Assets/UI/Items Slots/Iconos/ResinVesselIconColor.png"
                ));

        // Item Name: 
        itemsDictionary.Add("armor_common", new
            I_Equippable(
            /*always none*/     ITEM_SLOT.NONE,
            /*item type*/       ITEM_SLOT.ARMOR,
            /*itemRarity*/      ITEM_RARITY.COMMON,
            /*isEquipped*/      false,
            /*name*/            "Common Exo-Armor",
            /*description*/     "An exo-armor that will help you withstand the blows of Xenomorphs. It's light and tough, much better than marine tech",
            /*imagePath*/       "Assets/UI/Items Slots/Iconos/ArmorIconColor.png",
            /*HP*/                  0,
            /*armor*/               10,
            /*speed*/               0,
            /*fireRate*/            0,
            /*reloadSpeed*/         0,
            /*damageMultiplier*/    0
            ));

        itemsDictionary.Add("armor_rare", new
            I_Equippable(
            /*always none*/     ITEM_SLOT.NONE,
            /*item type*/       ITEM_SLOT.ARMOR,
            /*itemRarity*/      ITEM_RARITY.RARE,
            /*isEquipped*/      false,
            /*name*/            "Rare Exo-Armor",
            /*description*/     "An exo-armor that will help you withstand the blows of Xenomorphs. It's light and tough, much better than marine tech",
            /*imagePath*/       "Assets/UI/Items Slots/Iconos/ArmorIconColor.png",
            /*HP*/                  0,
            /*armor*/               18,
            /*speed*/               0,
            /*fireRate*/            0,
            /*reloadSpeed*/         0,
            /*damageMultiplier*/    0
            ));

        itemsDictionary.Add("armor_epic", new
            I_Equippable(
            /*always none*/     ITEM_SLOT.NONE,
            /*item type*/       ITEM_SLOT.ARMOR,
            /*itemRarity*/      ITEM_RARITY.EPIC,
            /*isEquipped*/      false,
            /*name*/            "Epic Exo-Armor",
            /*description*/     "An exo-armor that will help you withstand the blows of Xenomorphs. It's light and tough, much better than marine tech",
            /*imagePath*/       "Assets/UI/Items Slots/Iconos/ArmorIconColor.png",
            /*HP*/                  0,
            /*armor*/               25,
            /*speed*/               0,
            /*fireRate*/            0,
            /*reloadSpeed*/         0,
            /*damageMultiplier*/    0
            ));

        itemsDictionary.Add("ofChip_common", new
            I_Equippable(
            /*always none*/     ITEM_SLOT.NONE,
            /*item type*/       ITEM_SLOT.CHIP,
            /*itemRarity*/      ITEM_RARITY.COMMON,
            /*isEquipped*/      false,
            /*name*/            "Common Offensive Chip",
            /*description*/     "A thin layer made of aluminum bone with thin and sharp canals. Boosts the offensive capabilities",
            /*imagePath*/       "Assets/UI/Items Slots/Iconos/OffensiveChipIcon2Color.png",
            /*HP*/                  0,
            /*armor*/               0,
            /*speed*/               1,
            /*fireRate*/            0,
            /*reloadSpeed*/         5,
            /*damageMultiplier*/    12.5f
            ));

        itemsDictionary.Add("ofChip_rare", new
            I_Equippable(
            /*always none*/     ITEM_SLOT.NONE,
            /*item type*/       ITEM_SLOT.CHIP,
            /*itemRarity*/      ITEM_RARITY.RARE,
            /*isEquipped*/      false,
            /*name*/            "Rare Offensive Chip",
            /*description*/     "A thin layer made of aluminum bone with thin and sharp canals. Boosts the offensive capabilities",
            /*imagePath*/       "Assets/UI/Items Slots/Iconos/OffensiveChipIcon2Color.png",
            /*HP*/                  0,
            /*armor*/               0,
            /*speed*/               2,
            /*fireRate*/            0,
            /*reloadSpeed*/         9,
            /*damageMultiplier*/    24f
            ));

        itemsDictionary.Add("ofChip_epic", new
            I_Equippable(
            /*always none*/     ITEM_SLOT.NONE,
            /*item type*/       ITEM_SLOT.CHIP,
            /*itemRarity*/      ITEM_RARITY.EPIC,
            /*isEquipped*/      false,
            /*name*/            "Epic Offensive Chip",
            /*description*/     "A thin layer made of aluminum bone with thin and sharp canals. Boosts the offensive capabilities",
            /*imagePath*/       "Assets/UI/Items Slots/Iconos/OffensiveChipIcon2Color.png",
            /*HP*/                  0,
            /*armor*/               0,
            /*speed*/               3,
            /*fireRate*/            0,
            /*reloadSpeed*/         15,
            /*damageMultiplier*/    45f
            ));

        itemsDictionary.Add("defChip_common", new
            I_Equippable(
            /*always none*/     ITEM_SLOT.NONE,
            /*item type*/       ITEM_SLOT.CHIP,
            /*itemRarity*/      ITEM_RARITY.COMMON,
            /*isEquipped*/      false,
            /*name*/            "Common Defensive Chip",
            /*description*/     "A thick layer made of aluminum bone with many thin canals spread around various layers. Boosts the defensive capabilities",
            /*imagePath*/       "Assets/UI/Items Slots/Iconos/DefensiveChipIcon2Color.png",
            /*HP*/                  0,
            /*armor*/               5,
            /*speed*/               0,
            /*fireRate*/            0,
            /*reloadSpeed*/         0,
            /*damageMultiplier*/    0
            ));

        itemsDictionary.Add("defChip_rare", new
            I_Equippable(
            /*always none*/     ITEM_SLOT.NONE,
            /*item type*/       ITEM_SLOT.CHIP,
            /*itemRarity*/      ITEM_RARITY.RARE,
            /*isEquipped*/      false,
            /*name*/            "Rare Defensive Chip",
            /*description*/     "A thick layer made of aluminum bone with many thin canals spread around various layers. Boosts the defensive capabilities",
            /*imagePath*/       "Assets/UI/Items Slots/Iconos/DefensiveChipIcon2Color.png",
            /*HP*/                  0,
            /*armor*/               9,
            /*speed*/               0,
            /*fireRate*/            0,
            /*reloadSpeed*/         0,
            /*damageMultiplier*/    0
            ));

        itemsDictionary.Add("defChip_epic", new
            I_Equippable(
            /*always none*/     ITEM_SLOT.NONE,
            /*item type*/       ITEM_SLOT.CHIP,
            /*itemRarity*/      ITEM_RARITY.EPIC,
            /*isEquipped*/      false,
            /*name*/            "Epic Defensive Chip",
            /*description*/     "A thick layer made of aluminum bone with many thin canals spread around various layers. Boosts the defensive capabilities",
            /*imagePath*/       "Assets/UI/Items Slots/Iconos/DefensiveChipIcon2Color.png",
            /*HP*/                  0,
            /*armor*/               12.5f,
            /*speed*/               0,
            /*fireRate*/            0,
            /*reloadSpeed*/         0,
            /*damageMultiplier*/    0
            ));

        // Item Name: 
        itemsDictionary.Add("grenade_common", new
            I_Consumables(
            /*always none*/     ITEM_SLOT.NONE,
            /*item type*/       ITEM_SLOT.CONSUMABLE,
            /*itemRarity*/      ITEM_RARITY.COMMON,
            /*isEquipped*/      false,
            /*name*/            "Common Grenade",
            /*description*/     "High damage explosive grenade that contains “Spitter” acid",
            /*imagePath*/       "Assets/UI/Items Slots/Iconos/GrenadeIcon2Color.png",
            /*dmg*/             200,
            /*area*/            6,
            /*time*/            0
            ));

        itemsDictionary.Add("grenade_rare", new
            I_Consumables(
            /*always none*/     ITEM_SLOT.NONE,
            /*item type*/       ITEM_SLOT.CONSUMABLE,
            /*itemRarity*/      ITEM_RARITY.RARE,
            /*isEquipped*/      false,
            /*name*/            "Rare Grenade",
            /*description*/     "High damage explosive grenade that contains “Spitter” acid",
            /*imagePath*/       "Assets/UI/Items Slots/Iconos/GrenadeIcon2Color.png",
            /*dmg*/             360,
            /*area*/            7,
            /*time*/            0
            ));

        itemsDictionary.Add("grenade_epic", new
            I_Consumables(
            /*always none*/     ITEM_SLOT.NONE,
            /*item type*/       ITEM_SLOT.CONSUMABLE,
            /*itemRarity*/      ITEM_RARITY.EPIC,
            /*isEquipped*/      false,
            /*name*/            "Epic Grenade",
            /*description*/     "High damage explosive grenade that contains “Spitter” acid",
            /*imagePath*/       "Assets/UI/Items Slots/Iconos/GrenadeIcon2Color.png",
            /*dmg*/             450,
            /*area*/            8,
            /*time*/            0
            ));

        itemsDictionary.Add("bakerhouse_common", new
            I_Consumables(
            /*always none*/     ITEM_SLOT.NONE,
            /*item type*/       ITEM_SLOT.CONSUMABLE,
            /*itemRarity*/      ITEM_RARITY.COMMON,
            /*isEquipped*/      false,
            /*name*/            "Common Baker House",
            /*description*/     "Miniature of a legendary baker house. It has a mechanism that attracts enemies with a peculiar noise for a few seconds",
            /*imagePath*/       "Assets/UI/Items Slots/Iconos/BakerHouseIconColor.png",
            /*dmg*/             0,
            /*area*/            10,
            /*time*/            3
            ));

        itemsDictionary.Add("bakerhouse_rare", new
            I_Consumables(
            /*always none*/     ITEM_SLOT.NONE,
            /*item type*/       ITEM_SLOT.CONSUMABLE,
            /*itemRarity*/      ITEM_RARITY.RARE,
            /*isEquipped*/      false,
            /*name*/            "Rare Baker House",
            /*description*/     "Miniature of a legendary baker house. It has a mechanism that attracts enemies with a peculiar noise for a few seconds",
            /*imagePath*/       "Assets/UI/Items Slots/Iconos/BakerHouseIconColor.png",
            /*dmg*/             0,
            /*area*/            12,
            /*time*/            4
            ));

        itemsDictionary.Add("bakerhouse_epic", new
            I_Consumables(
            /*always none*/     ITEM_SLOT.NONE,
            /*item type*/       ITEM_SLOT.CONSUMABLE,
            /*itemRarity*/      ITEM_RARITY.EPIC,
            /*isEquipped*/      false,
            /*name*/            "Epic Baker House",
            /*description*/     "Miniature of a legendary baker house. It has a mechanism that attracts enemies with a peculiar noise for a few seconds",
            /*imagePath*/       "Assets/UI/Items Slots/Iconos/BakerHouseIconColor.png",
            /*dmg*/             0,
            /*area*/            15,
            /*time*/            6
            ));
    }

    static public Item SearchItemInDictionary(string name)
    {
        Debug.Log("hola " + itemsDictionary[name].name);
        return itemsDictionary[name];
    }
    #endregion
}

