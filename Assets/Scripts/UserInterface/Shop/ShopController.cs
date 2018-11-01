using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopController : MonoBehaviour {


    public ItemDetails itemDetails;
    public ItemInventoryController[] inventory;


    private int totalItem = 0;


    public void BuyItem(int index) {

        // save some data through data controller;
        // get current value of data controller;

        // test
        totalItem += 1;
        inventory[index].AddItem(totalItem);

    }

}
