using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopController : MonoBehaviour {


    public ItemDetails itemDetails;
    public ItemInventoryController[] inventory;

    private DataController data = null;

    public void BuyItem(int index) {

        if (data == null)
            data = DataController.instance;

        float currentTrash = data.dataFile.trash_cash;

        Inventory item = data.dataFile.items[index];

        if (currentTrash >= item.item_price) {
            item.item_count++;
            currentTrash -= item.item_price;

            data.dataFile.items[index] = item;
            data.dataFile.trash_cash = currentTrash;

            inventory[index].AddItem(item.item_count);
            itemDetails.SetTrash(currentTrash);
        }


    }

}
