using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewStats", menuName = "Character/Stats", order = 1)]
public class CharacterStats_SO : ScriptableObject
{
    [System.Serializable]
    public class CharLevelUps
    {
        public int maxHealth;
        public int maxMana;
        public int maxWealth;
        public int baseDamage;
        public float baseResistance;
        public float maxEncumbrance;
    }

    #region Fields
    public bool setManually = false;
    public bool saveDataOnClose = false;

    public ItemPickUp weapon { get; private set; }
    public ItemPickUp headArmor { get; private set; }
    public ItemPickUp chestArmor { get; private set; }
    public ItemPickUp handArmor { get; private set; }
    public ItemPickUp legArmor { get; private set; }
    public ItemPickUp footArmor { get; private set; }
    public ItemPickUp misc1 { get; private set; }
    public ItemPickUp misc2 { get; private set; }

    public int maxHealth = 0;
    public int currentHealth = 0;

    public int maxWealth = 0;
    public int currentWealth = 0;

    public int maxMana = 0;
    public int currentMana = 0;

    public int baseDamage = 0;
    public int currentDamage = 0;

    public float baseResistance = 0;
    public float currentResistance = 0f;

    public float maxEncumbrance = 0f;
    public float currentEncumbrance = 0f;

    public int charExperience = 0;
    public int charLevel = 0;

    public CharLevelUps[] charLevelUps;
    #endregion

    #region Stat Increasers
    public void ApplyHealth(int healthAmount)
    {
        if ((currentHealth + healthAmount) > maxHealth)
        {
            currentHealth = maxHealth;
        }
        else
        {
            currentHealth += healthAmount;
        }
    }

    public void ApplyMana(int manaAmount)
    {
        if ((currentMana + manaAmount) > maxMana)
        {
            currentMana = maxMana;
        }
        else
        {
            currentMana += manaAmount;
        }
    }

    public void GiveWealth(int wealthAmount)
    {
        if ((currentWealth + wealthAmount) > maxWealth)
        {
            currentWealth = maxWealth;
        }
        else
        {
            currentWealth += wealthAmount;
        }
    }

    public void EquipWeapon(ItemPickUp weaponPickUp, CharacterInventory charInventory, GameObject weaponSlot)
    {
        Rigidbody newWeapon;

        weapon = weaponPickUp;
        charInventory.inventoryDisplaySlots[2].sprite = weaponPickUp.itemDefinition.itemIcon;
        newWeapon = Instantiate(weaponPickUp.itemDefinition.weaponSlotObject.weaponPreb, weaponSlot.transform);
        currentDamage = baseDamage + weapon.itemDefinition.itemAmount;
    }

    public void EquipArmor(ItemPickUp armorPickUp, CharacterInventory charInventory)
    {
        switch (armorPickUp.itemDefinition.itemArmorSubType)
        {
            case ItemArmorSubType.Head:
                charInventory.inventoryDisplaySlots[3].sprite = armorPickUp.itemDefinition.itemIcon;
                headArmor = armorPickUp;
                currentResistance += armorPickUp.itemDefinition.itemAmount;
                break;
            case ItemArmorSubType.Chest:
                charInventory.inventoryDisplaySlots[4].sprite = armorPickUp.itemDefinition.itemIcon;
                chestArmor = armorPickUp;
                currentResistance += armorPickUp.itemDefinition.itemAmount;
                break;
            case ItemArmorSubType.Hands:
                charInventory.inventoryDisplaySlots[5].sprite = armorPickUp.itemDefinition.itemIcon;
                handArmor = armorPickUp;
                currentResistance += armorPickUp.itemDefinition.itemAmount;
                break;
            case ItemArmorSubType.Legs:
                charInventory.inventoryDisplaySlots[6].sprite = armorPickUp.itemDefinition.itemIcon;
                legArmor = armorPickUp;
                currentResistance += armorPickUp.itemDefinition.itemAmount;
                break;
            case ItemArmorSubType.Boots:
                charInventory.inventoryDisplaySlots[7].sprite = armorPickUp.itemDefinition.itemIcon;
                footArmor = armorPickUp;
                currentResistance += armorPickUp.itemDefinition.itemAmount;
                break;
        }
    }
    #endregion

    #region Stat Reducers
    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            Death();
        }
    }

    public void TakeMana(int amount)
    {
        currentMana -= amount;

        if (currentMana < 0)
        {
            currentMana = 0;
        }
    }

    public bool UnEquipWeapon(ItemPickUp weaponPickUp, CharacterInventory charInventory, GameObject weaponSlot)
    {
        bool previousWeaponSame = false;

        if (weapon != null)
        {
            if (weapon == weaponPickUp)
            {
                previousWeaponSame = true;
            }
            charInventory.inventoryDisplaySlots[2].sprite = null;
            DestroyObject(weaponSlot.transform.GetChild(0).gameObject);
            weapon = null;
            currentDamage = baseDamage;
        }

        return previousWeaponSame;
    }

    public bool UnEquipArmor(ItemPickUp armorPickUp, CharacterInventory charInventory)
    {
        bool previousArmorSame = false;

        switch (armorPickUp.itemDefinition.itemArmorSubType)
        {
            case ItemArmorSubType.Head:
                if (headArmor != null)
                {
                    if (headArmor == armorPickUp)
                    {
                        previousArmorSame = true;
                    }
                    charInventory.inventoryDisplaySlots[3].sprite = null;
                    currentResistance -= armorPickUp.itemDefinition.itemAmount;
                    headArmor = null;
                }
                break;
            case ItemArmorSubType.Chest:
                if (chestArmor != null)
                { 
                    if (chestArmor == armorPickUp)
                    {
                        previousArmorSame = true;
                    }
                    charInventory.inventoryDisplaySlots[4].sprite = null;
                    currentResistance -= armorPickUp.itemDefinition.itemAmount;
                    chestArmor = null;
                }
                break;
            case ItemArmorSubType.Hands:
                if (handArmor != null)
                {
                    if (handArmor == armorPickUp)
                    {
                        previousArmorSame = true;
                    }
                    charInventory.inventoryDisplaySlots[5].sprite = null;
                    currentResistance -= armorPickUp.itemDefinition.itemAmount;
                    handArmor = null;
                }
                break;
            case ItemArmorSubType.Legs:
                if (legArmor != null)
                {
                    if (legArmor == armorPickUp)
                    {
                        previousArmorSame = true;
                    }
                    charInventory.inventoryDisplaySlots[6].sprite = null;
                    currentResistance -= armorPickUp.itemDefinition.itemAmount;
                    legArmor = null;
                }
                break;
            case ItemArmorSubType.Boots:
                if (footArmor != null)
                {
                    if (footArmor == armorPickUp)
                    {
                        previousArmorSame = true;
                    }
                    charInventory.inventoryDisplaySlots[7].sprite = null;
                    currentResistance -= armorPickUp.itemDefinition.itemAmount;
                    footArmor = null;
                }
                break;
        }

        return previousArmorSame;
    }
    #endregion

    #region Character Level Up and Death
    private void Death()
    {
        Debug.Log("You kicked it! Sorry Moosa-Magoose.");
        //Call to Game Manager for Death State to trigger respawn
        //Dispaly the Death visualization
    }

    private void LevelUp()
    {
        charLevel += 1;
        //Display Level Up Visualization

        maxHealth = charLevelUps[charLevel -1].maxHealth;
        maxMana = charLevelUps[charLevel - 1].maxMana;
        maxWealth = charLevelUps[charLevel - 1].maxWealth;
        baseDamage = charLevelUps[charLevel - 1].baseDamage;
        baseResistance = charLevelUps[charLevel - 1].baseResistance;
        maxEncumbrance = charLevelUps[charLevel - 1].maxEncumbrance;
    }
    #endregion

}