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

        DataFile dta = data.dataFile;

        float currentTrash = dta.trash_cash;
        Inventory item = dta.items[index];

        if (currentTrash >= item.item_price) {
            item.item_count++;
            currentTrash -= item.item_price;

            dta.items[index] = item;
            dta.trash_cash = currentTrash;

            data.dataFile = dta;

            inventory[index].AddItem(item.item_count);
            itemDetails.SetTrash(currentTrash);
        }


    }

}
