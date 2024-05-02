using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YmirEngine
{
    public enum UI_STATE
    {
        DISABLED,
        NORMAL,
        FOCUSED,
        PRESSED,
        RELEASE,
        SELECTED,

        NONE
    };

    public enum ITEM_SLOT
    {
        ARMOR,
        CHIP,
        CONSUMABLE,
        SAVE,
        NONE, 

        SIZE
    }

    public enum ITEM_RARITY
    {
        COMMON,
        RARE,
        EPIC,

        NONE
    }

    // WIP
    public class Item
    {
        public ITEM_SLOT currentSlot;
        public ITEM_SLOT itemType;
        public ITEM_RARITY itemRarity;

        public float HP, armor, speed, fireRate, reloadSpeed, damageMultiplier;

        public string imagePath = "";
        public string name = "";
        public string description = "";
        public bool isEquipped = false;

        public Item(ITEM_SLOT currentSlot = ITEM_SLOT.NONE, ITEM_SLOT itemType = ITEM_SLOT.NONE,
            float HP = 0, float armor = 0, float speed = 0, float fireRate = 0, float reloadSpeed = 0, float damageMultiplier = 0,
            string imagePath = "", ITEM_RARITY itemRarity = ITEM_RARITY.COMMON, bool isEquipped = false, string name = "Empty", string description = "Empty")
        {
            this.name = name;
            this.description = description;
            this.currentSlot = currentSlot;
            this.itemType = itemType;
            this.HP = HP;
            this.armor = armor;
            this.speed = speed;
            this.fireRate = fireRate;
            this.reloadSpeed = reloadSpeed;
            this.damageMultiplier = damageMultiplier;
            this.imagePath = imagePath;
            this.itemRarity = itemRarity;
            this.isEquipped = isEquipped;
        }
    }

    public class Upgrade
    {
        public string name, description;
        public int cost;
        public bool isUnlocked;

        public Upgrade(string name, string description, int cost, bool isUnlocked)
        {
            this.name = name;
            this.description = description;
            this.cost = cost;
            this.isUnlocked = isUnlocked;
        }
    }
}
