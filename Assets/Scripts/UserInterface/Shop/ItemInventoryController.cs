using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemInventoryController : MonoBehaviour {

    public Sprite defaultImg;
    public Sprite haveItemImg;

    public Text items;

    private Sprite currentImg;

    private Image img;


    public void AddItem(int item) {

        if (img == null) {
            img = GetComponent<Image>();
        }

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
