using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class DataFile{

    public float trash_cash;

    public bool music;

    public bool first_run;

    public int currentLevel;

    public Levels[] level;

    public List<Inventory> items;

}

[System.Serializable]
public struct Inventory {

    public string item_name;

    public int item_count;

    public int item_price;

}

[System.Serializable]
public struct Levels{
    public string levelName;
    public int levelIndex;
    public Sprite image;
}