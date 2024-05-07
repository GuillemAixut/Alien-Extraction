using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YmirEngine
{
    public static class Globals
    {
        public static string saveGameDir = "Assets/GameFiles/";
        public static string saveGameExt = ".ygame";

        #region DEFINE ITEMS

        // Items dictionary
        public static Dictionary<string, Item> itemsDictionary = new Dictionary<string, Item>();

        public static void CreateItemDictionary()
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

        public static Item SearchItemInDictionary(string name)
        {
            Debug.Log("hola " + itemsDictionary[name].name);
            return itemsDictionary[name];
        }
        #endregion
    }
}