using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using YmirEngine;

public class UI_Crafting_Recipe : YmirComponent
{
    public int itemNumber;
    private Player player = null;

    public void Start()
    {
        //_itemButtonList = new List<UI_Item_Button>(); 

        //for (int i = 0; i < InternalCalls.CS_GetChildrenSize(gameObject) - 1; i++) // Don't check last element (item to create)
        //{
        //    GameObject button = InternalCalls.CS_GetChild(InternalCalls.CS_GetChild(gameObject, i), 2); // (Slot(Button))
        //    _itemButtonList.Add(button.GetComponent<UI_Item_Button>());
        //}

        GetPlayerScript();
    }

    public void Update()
    {
        return;
    }

    public void Check() // Check if all slots are filled to craft item
    {
        int count = 0; 

        for (int i = 0; i < InternalCalls.CS_GetChildrenSize(gameObject) - 1; i++) // Don't check last element (item to create)
        {
            GameObject button = InternalCalls.CS_GetChild(InternalCalls.CS_GetChild(gameObject, i), 2); // (Slot(Button))
            Debug.Log(button.Name);
            Debug.Log(button.GetComponent<UI_Item_Button>().itemType.ToString());
            Debug.Log(button.GetComponent<UI_Item_Button>().currentSlot.ToString());

            if (button.GetComponent<UI_Item_Button>().itemType != ITEM_SLOT.NONE) // Check if it's empty
            {
                count++;
                Debug.Log(count.ToString());
            }
        }

        if (count == InternalCalls.CS_GetChildrenSize(gameObject) - 1) // If all slots are filled, craft item
        {
            Craft();
            Debug.Log("CRAFT");
        }

        else
        {
            Debug.Log("NO CRAFT");
        }
    }

    private void Craft()
    {
        // Delete item recipe
        for (int i = 0; i < InternalCalls.CS_GetChildrenSize(gameObject) - 1; i++) // Don't check last element (item to create)
        {
            // Empty item
            InternalCalls.CS_GetChild(InternalCalls.CS_GetChild(gameObject, i), 2).GetComponent<UI_Item_Button>().item = InternalCalls.CS_GetChild(InternalCalls.CS_GetChild(gameObject, i), 2).GetComponent<UI_Item_Button>().CreateItemBase();
            
            // Delete item from player list
            for (int j = 0; j < player.itemsList.Count; j++)
            {
                if (player.itemsList[i].Equals(InternalCalls.CS_GetChild(InternalCalls.CS_GetChild(gameObject, i), 2)))
                {
                    player.itemsList.Remove(player.itemsList[i]);
                    break;
                }
            }
        }

        // Don't trust reference
        //GameObject craftGO = InternalCalls.CS_GetChild(InternalCalls.CS_GetChild(gameObject, InternalCalls.CS_GetChildrenSize(gameObject) - 1), 2); // Last GO of grid is the crafted item
        //craftGO.GetComponent<UI_Item_Button>().item = UI_Inventory.SearchItemInDictionary("armor_common"); // TODO: ajustar nom busca + formula raresa
        //player.itemsList.Add(craftGO.GetComponent<UI_Item_Button>().item);

        // Add new item
        InternalCalls.CS_GetChild(InternalCalls.CS_GetChild(gameObject, InternalCalls.CS_GetChildrenSize(gameObject) - 1), 2).GetComponent<UI_Item_Button>().item =
            UI_Inventory.SearchItemInDictionary("armor_common"); // TODO: ajustar nom busca + formula raresa
        player.itemsList.Add(InternalCalls.CS_GetChild(InternalCalls.CS_GetChild(gameObject, InternalCalls.CS_GetChildrenSize(gameObject) - 1), 2).GetComponent<UI_Item_Button>().item);
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