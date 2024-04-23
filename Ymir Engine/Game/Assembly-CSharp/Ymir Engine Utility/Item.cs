using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YmirEngine
{
    enum UI_STATE
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
        CHIP1,
        CHIP2,
        CONSUMABLE1,
        CONSUMABLE2,
        SAVE,
        NONE
    }

    // WIP
    public class Item
    {
        public ITEM_SLOT currentSlot;
        public ITEM_SLOT itemType;
        public float HP, armor, speed, fireRate, reloadSpeed, damageMultiplier;

        public Item(ITEM_SLOT currentSlot, ITEM_SLOT itemType, float HP = 0, float armor = 0, float speed = 0, float fireRate = 0, float reloadSpeed = 0, float damageMultiplier = 0)
        {
            this.currentSlot = currentSlot;
            this.itemType = itemType;
            this.HP = HP;
            this.armor = armor;
            this.speed = speed;
            this.fireRate = fireRate;
            this.reloadSpeed = reloadSpeed;
            this.damageMultiplier = damageMultiplier;
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
