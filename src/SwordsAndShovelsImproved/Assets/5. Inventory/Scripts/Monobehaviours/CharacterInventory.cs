using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class CharacterInventory : MonoBehaviour
{
    #region Variable Declarations
    public static CharacterInventory instance;

    public CharacterStats charStats;
    GameObject foundStats;

    public Image[] hotBarDisplayHolders = new Image[4];
    public GameObject InventoryDisplayHolder;
    public Image[] inventoryDisplaySlots = new Image[30];

    int inventoryItemCap = 20;
    int idCount = 1;
    bool addedItem = true;

    public Dictionary<int, InventoryEntry> itemsInInventory = new Dictionary<int, InventoryEntry>();
    public InventoryEntry itemEntry;
    #endregion

    #region Initializations
    [Inject]
    private void Construct(InventoryDisplay inventoryDisplay)
    {
        InventoryDisplayHolder = inventoryDisplay.InventoryDisplayHolder;
        inventoryDisplaySlots = inventoryDisplay.InventoryDisplayHolder.GetComponentsInChildren<Image>();
        hotBarDisplayHolders = inventoryDisplay.HotbarDisplaySlots;
    }
    
    void Start()
    {
        instance = this;
        itemEntry = new InventoryEntry(0, null, null);
        itemsInInventory.Clear();
    }
    #endregion

    void Update()
    {
        #region Watch for Hotbar Keypresses - Called by Character Controller Later
        //Checking for a hotbar key to be pressed
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            TriggerItemUse(101);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            TriggerItemUse(102);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            TriggerItemUse(103);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            TriggerItemUse(104);
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            DisplayInventory();
        }
        #endregion

        //Check to see if the item has already been added - Prevent duplicate adds for 1 item
        if (!addedItem)
        {
            TryPickUp();
        }
    }

    public void StoreItem(ItemPickUp itemToStore)
    {
        addedItem = false;

        if ((charStats.characterDefinition.currentEncumbrance + itemToStore.itemDefinition.itemWeight) <= charStats.characterDefinition.maxEncumbrance)
        {
            itemEntry.invEntry = itemToStore;
            itemEntry.stackSize = 1;
            itemEntry.hbSprite = itemToStore.itemDefinition.itemIcon;

            //addedItem = false;
            itemToStore.gameObject.SetActive(false);
        }
    }

    void TryPickUp()
    {
        bool itsInInv = true;

        //Check to see if the item to be stored was properly submitted to the inventory - Continue if Yes otherwise do nothing
        if (itemEntry.invEntry)
        {
            //Check to see if any items exist in the inventory already - if not, add this item
            if (itemsInInventory.Count == 0)
            {
                addedItem = AddItemToInv(addedItem);
            }
            //If items exist in inventory
            else
            {
                //Check to see if the item is stackable - Continue if stackable
                if (itemEntry.invEntry.itemDefinition.isStackable)
                {
                    foreach (KeyValuePair<int, InventoryEntry> ie in itemsInInventory)
                    {
                        //Does this item already exist in inventory? - Continue if Yes
                        if (itemEntry.invEntry.itemDefinition == ie.Value.invEntry.itemDefinition)
                        {
                            //Add 1 to stack and destroy the new instance
                            ie.Value.stackSize += 1;
                            AddItemToHotBar(ie.Value);
                            itsInInv = true;
                            DestroyObject(itemEntry.invEntry.gameObject);
                            break;
                        }
                        //If item does not exist already in inventory then continue here
                        else
                        {
                            itsInInv = false;
                        }
                    }
                }
                //If Item is not stackable then continue here
                else
                {
                    itsInInv = false;

                    //If no space and item is not stackable - say inventory full
                    if (itemsInInventory.Count == inventoryItemCap)
                    {
                        itemEntry.invEntry.gameObject.SetActive(true);
                        Debug.Log("Inventory is Full");
                    }
                }

                //Check if there is space in inventory - if yes, continue here
                if (!itsInInv)
                {
                    addedItem = AddItemToInv(addedItem);
                    itsInInv = true;
                }
            }
        }
    }

    bool AddItemToInv(bool finishedAdding)
    {
        itemsInInventory.Add(idCount, new InventoryEntry(itemEntry.stackSize, Instantiate(itemEntry.invEntry), itemEntry.hbSprite));

        DestroyObject(itemEntry.invEntry.gameObject);

        FillInventoryDisplay();
        AddItemToHotBar(itemsInInventory[idCount]);

        idCount = IncreaseID(idCount);

        #region Reset itemEntry
        itemEntry.invEntry = null;
        itemEntry.stackSize = 0;
        itemEntry.hbSprite = null;
        #endregion

        finishedAdding = true;

        return finishedAdding;
    }

    int IncreaseID(int currentID)
    {
        int newID = 1;

        for (int itemCount = 1; itemCount <= itemsInInventory.Count; itemCount++)
        {
            if (itemsInInventory.ContainsKey(newID))
            {
                newID += 1;
            }
            else return newID;
        }

        return newID;
    }

    private void AddItemToHotBar(InventoryEntry itemForHotBar)
    {
        int hotBarCounter = 0;
        bool increaseCount = false;

        //Check for open hotbar slot
        foreach (Image images in hotBarDisplayHolders)
        {
            hotBarCounter += 1;

            if (itemForHotBar.hotBarSlot == 0)
            {
                if (images.sprite == null)
                {
                    //Add item to open hotbar slot
                    itemForHotBar.hotBarSlot = hotBarCounter;
                    //Change hotbar sprite to show item
                    images.sprite = itemForHotBar.hbSprite;
                    increaseCount = true;
                    break;
                }
            }
            else if (itemForHotBar.invEntry.itemDefinition.isStackable)
            {
                increaseCount = true;
            }
        }

        if (increaseCount)
        {
            hotBarDisplayHolders[itemForHotBar.hotBarSlot - 1].GetComponentInChildren<Text>().text = itemForHotBar.stackSize.ToString();
        }

        increaseCount = false;
    }

    void DisplayInventory()
    {
        if (InventoryDisplayHolder.activeSelf == true)
        {
            InventoryDisplayHolder.SetActive(false);
        }
        else
        {
            InventoryDisplayHolder.SetActive(true);
        }
    }

    void FillInventoryDisplay()
    {
        int slotCounter = 9;

        foreach (KeyValuePair<int, InventoryEntry> ie in itemsInInventory)
        {
            slotCounter += 1;
            inventoryDisplaySlots[slotCounter].sprite = ie.Value.hbSprite;
            ie.Value.inventorySlot = slotCounter - 9;
        }

        while (slotCounter < 29)
        {
            slotCounter++;
            inventoryDisplaySlots[slotCounter].sprite = null;
        }
    }

    public void TriggerItemUse(int itemToUseID)
    {
        bool triggerItem = false;

        foreach (KeyValuePair<int, InventoryEntry> ie in itemsInInventory)
        {
            if (itemToUseID > 100)
            {
                itemToUseID -= 100;

                if (ie.Value.hotBarSlot == itemToUseID)
                {
                    triggerItem = true;
                }
            }
            else
            {
                if (ie.Value.inventorySlot == itemToUseID)
                {
                    triggerItem = true;
                }
            }

            if (triggerItem)
            {
                if (ie.Value.stackSize == 1)
                {
                    if (ie.Value.invEntry.itemDefinition.isStackable)
                    {
                        if (ie.Value.hotBarSlot != 0)
                        {
                            hotBarDisplayHolders[ie.Value.hotBarSlot - 1].sprite = null;
                            hotBarDisplayHolders[ie.Value.hotBarSlot - 1].GetComponentInChildren<Text>().text = "0";
                        }

                        ie.Value.invEntry.UseItem();
                        itemsInInventory.Remove(ie.Key);
                        break;
                    }
                    else
                    {
                        ie.Value.invEntry.UseItem();
                        if (!ie.Value.invEntry.itemDefinition.isIndestructable)
                        {
                            itemsInInventory.Remove(ie.Key);
                            break;
                        }
                    }
                }
                else
                {
                    ie.Value.invEntry.UseItem();
                    ie.Value.stackSize -= 1;
                    hotBarDisplayHolders[ie.Value.hotBarSlot - 1].GetComponentInChildren<Text>().text = ie.Value.stackSize.ToString();
                    break;
                }
            }
        }

        FillInventoryDisplay();
    }

                
        


}