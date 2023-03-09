using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class InventoryManager : MonoBehaviour
{

    public GameObject itemSlotPrefab;
    private Transform itemSlotContainer;
    private Inventory inventory;
    private List<GameObject> itemSlotGameObjects = new List<GameObject>();


    private void Awake()
    {
        itemSlotContainer = transform.Find("itemSlotContainer");
    }

    public void SetInventory(Inventory inventory)
    {
        this.inventory = inventory;
        Debug.Log("inventory set");
        inventory.OnItemListChanged += Inventory_OnItemListChanged;
        RefreshInventoryItems();
    }

    private void Inventory_OnItemListChanged(object sender, System.EventArgs e)
    {
        RefreshInventoryItems();
    }

    public void OnButton(Item item)
    {
        Debug.Log("button clicked");
        inventory.UseItem(item);
        RefreshInventoryItems();
    }

    private void RefreshInventoryItems()
    {
        if (inventory == null)
        {
            return;
        }
        // Destroy all previously instantiated item slot game objects
        foreach (GameObject itemSlotGameObject in itemSlotGameObjects)
        {
            Destroy(itemSlotGameObject);
        }
        itemSlotGameObjects.Clear();

        int x = -6;
        int y = 3;
        float itemSlotCellSize = 30f;
        foreach (Item item in inventory.GetItemList())
        {

            GameObject itemSlotGameObject = Instantiate(itemSlotPrefab, itemSlotContainer);
            itemSlotGameObjects.Add(itemSlotGameObject);
            RectTransform itemSlotRectTransform = itemSlotGameObject.GetComponent<RectTransform>();
            itemSlotRectTransform.gameObject.SetActive(true);

            Button button = itemSlotGameObject.AddComponent<Button>();
            button.onClick.AddListener(() => {
            Debug.Log("Button clicked with " + item);
            OnButton(item);});

            itemSlotRectTransform.anchoredPosition = new Vector2(x * itemSlotCellSize, y * itemSlotCellSize);
            Image image= itemSlotRectTransform.Find("Image").GetComponent<Image>();
            image.sprite=item.GetSprite();


            TextMeshProUGUI invText = itemSlotRectTransform.Find("text").GetComponent<TextMeshProUGUI>();
            if(item.amount>1)
            {
            invText.SetText(item.amount.ToString());
            }
            else{
                invText.SetText("");
            }
            x+=2;
            if (x > 6)
            {
                x = -6;
                y-=2;
            }
        }
    }



}

