using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class DataFile{

    public float trash_cash;

    public bool music;

    public List<Inventory> items;

}

[System.Serializable]
public struct Inventory {

    public string item_name;

    public int item_count;

    public int item_price;

}