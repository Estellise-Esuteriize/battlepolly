using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Item {
    Heart, Phone, Sheild, Magnet, Sweeper, Bomb, GearsOfWar 
}

public class ItemInventoryController : MonoBehaviour {

    public Item item;

    public Sprite defaultImg;
    public Sprite haveItemImg;

    public Text items;

    private Sprite currentImg;

    private Image img;

    private DataController data;

    void Start() {

        data = DataController.instance;
        img = GetComponent<Image>();

        Inventory inventory = data.dataFile.items[(int)item];

        if (inventory.item_count > 0) {
            items.text = inventory.item_count.ToString();
            img.sprite = haveItemImg;
        }

    }

    public void AddItem(int item) {

        currentImg = img.sprite;

        if (item > 0 && currentImg != haveItemImg) {
            img.sprite = haveItemImg;
        }
        else if (item <= 0 && currentImg != defaultImg) {
            img.sprite = defaultImg;
        }

        items.text = item.ToString();

    }


}
