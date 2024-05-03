using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using YmirEngine;

public class Item_World : YmirComponent
{
    struct SearchItem
    {
        string name;
        ITEM_RARITY rarity;

        public SearchItem(string _name, ITEM_RARITY _rarity)
        {
            name = _name;
            rarity = _rarity;
        }
    }

    Dictionary<SearchItem, Item> itemsDictionary = new Dictionary<SearchItem, Item>();

    private Item item = null;
    private Player player = null;

    public ITEM_RARITY itemRarity;

    public bool isEquipped = false;
    public string name = "";
    public string rarity_str = "";

    public void Start()
    {
        GetPlayerScript();
        CreateItemDictionary();

        switch (rarity_str)
        {
            case "COMMON":
                {
                    itemRarity = ITEM_RARITY.COMMON;
                }
                break;
            case "RARE":
                {
                    itemRarity = ITEM_RARITY.RARE;
                }
                break;
            case "EPIC":
                {
                    itemRarity = ITEM_RARITY.EPIC;
                }
                break;
            case "NONE":
                {
                    itemRarity = ITEM_RARITY.NONE;
                }
                break;
            default:
                break;
        }

        SearchItem searchItem = new SearchItem(name, itemRarity);
        item = itemsDictionary[searchItem];

        player.itemsList.Add(item);
    }

    public void Update()
    {
        return;
    }

    private void CreateItemDictionary()
    {
        // Item Name: 
        itemsDictionary.Add(new SearchItem("hola", ITEM_RARITY.EPIC), new
            Item(
            /*always none*/     ITEM_SLOT.NONE,
            /*item type*/       ITEM_SLOT.NONE,
            /*itemRarity*/      ITEM_RARITY.COMMON,
            /*isEquipped*/      false,
            /*name*/            "Empty",
            /*description*/     "Empty",
            /*imagePath*/       ""
                ));

        // Item Name: 
        itemsDictionary.Add(new SearchItem("hola", ITEM_RARITY.EPIC), new
            I_Equippable(
            /*always none*/     ITEM_SLOT.NONE,
            /*item type*/       ITEM_SLOT.NONE,
            /*itemRarity*/      ITEM_RARITY.COMMON,
            /*isEquipped*/      false,
            /*name*/            "Empty",
            /*description*/     "Empty",
            /*imagePath*/       "",
            /*HP*/                  0,
            /*armor*/               0,
            /*speed*/               0,
            /*fireRate*/            0,
            /*reloadSpeed*/         0,
            /*damageMultiplier*/    0
            ));

        // Item Name: 
        itemsDictionary.Add(new SearchItem("hola", ITEM_RARITY.EPIC), new
            I_Consumables(
            /*always none*/     ITEM_SLOT.NONE,
            /*item type*/       ITEM_SLOT.NONE,
            /*itemRarity*/      ITEM_RARITY.COMMON,
            /*isEquipped*/      false,
            /*name*/            "Empty",
            /*description*/     "Empty",
            /*imagePath*/       "",
            /*dmg*/                 0,
            /*range*/               0
            ));
    }

    private void GetPlayerScript()
    {
        GameObject gameObject = InternalCalls.GetGameObjectByName("Player");

        if (gameObject != null)
        {
            player = gameObject.GetComponent<Player>();
        }
    }
}