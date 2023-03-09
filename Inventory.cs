using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Inventory
{
    public event EventHandler OnItemListChanged;
    private List<Item> itemList;
    public PlayerController player;

    

    public Inventory(PlayerController player)
    {
        this.player=player;
        itemList = new List<Item>();
        StarterItem(new Item{itemType = Item.ItemType.HintScroll, amount=1});
        StarterItem(new Item{itemType = Item.ItemType.HealthPotion, amount=1});
        StarterItem(new Item{itemType = Item.ItemType.ManipulativesPotion, amount=1});
        
        
        Debug.Log(itemList.Count);
    }


    public void AddItem(GameObject itemObject)
    {
        Item newItem = new Item();

        // Set the item type based on the tag of the GameObject
        switch (itemObject.tag)
        {
            case "HintScroll":
                newItem.itemType = Item.ItemType.HintScroll;
                break;
            case "ManipulativesPotion":
                newItem.itemType = Item.ItemType.ManipulativesPotion;
                break;
            case "HealthPotion":
                newItem.itemType = Item.ItemType.HealthPotion;
                break;
            default:
                // If the tag is not recognized, do not add the item to the inventory
                Debug.LogWarning("Unrecognized item tag: " + itemObject.tag);
                return;
        }

        // Check if the item is stackable
        if (newItem.IsStackable())
        {
            // Look for an existing item of the same type in the inventory
            foreach (Item item in itemList)
            {
                if (item.itemType == newItem.itemType)
                {
                    // If an existing item is found, increase its amount and return
                    item.amount++;
                    OnItemListChanged?.Invoke(this, EventArgs.Empty);

                    // Deactivate the GameObject 
                    itemObject.SetActive(false);
                    Debug.Log(itemList.Count);

                    return;
                }
            }
        }

        // If the item is not stackable or there is no existing item of the same type, add a new item to the inventory
        newItem.amount = 1;
        itemList.Add(newItem);
        OnItemListChanged?.Invoke(this, EventArgs.Empty);

        // Deactivate the GameObject 
        itemObject.SetActive(false);
        Debug.Log(itemList.Count);
    }


    public void RemoveItem(Item item)
    {
        if (item.IsStackable())
        {
            Item itemInInventory = null;
            foreach (Item inventoryItem in itemList)
            {
                if (inventoryItem.itemType == item.itemType)
                {
                    inventoryItem.amount -= item.amount;
                    itemInInventory = inventoryItem;
                }

            }
            if (itemInInventory != null && itemInInventory.amount<=0)
            {
                itemList.Remove(itemInInventory);
            }

        }
        else 
        {
            itemList.Remove(item);
        }
        OnItemListChanged?.Invoke(this, EventArgs.Empty);
    }

    public void StarterItem(Item item)
    {
        itemList.Add(item);
    }

    public void UseItem(Item item)
    {
        Debug.Log("item being used");
        switch (item.itemType)
        {
            case Item.ItemType.HealthPotion:
                player.AddHealth(10);
                RemoveItem(new Item {itemType= Item.ItemType.HealthPotion, amount =1});
            break;
            case Item.ItemType.ManipulativesPotion:
                RemoveItem(new Item {itemType= Item.ItemType.ManipulativesPotion, amount =1});
            break;
            case Item.ItemType.HintScroll:
                RemoveItem(new Item {itemType= Item.ItemType.HintScroll, amount =1});
            break;
        }
        Debug.Log(itemList.Count);
    }

    public List<Item> GetItemList()
    {
        return itemList;
    }
}
