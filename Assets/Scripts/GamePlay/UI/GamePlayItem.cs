using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GamePlayItem : MonoBehaviour {

    public Sprite defualtImg;
    public Sprite haveItmImg;

    public EventTrigger clickItem;
    public Text textHolder;


    public Item item;


    private Image image;


    ButtonHelperController buttonInstance;
    DataController dataInstance;

    void Start() {
        buttonInstance = ButtonHelperController.instance;
        dataInstance = DataController.instance;

        image = clickItem.GetComponent<Image>();


        DataFile data = dataInstance.dataFile;

        Inventory itm = data.items[(int)item];

        if (itm.item_count > 0) {
            textHolder.text = itm.item_count.ToString();
            image.sprite = haveItmImg;
        }
        else {
            textHolder.text = "0";
            image.sprite = defualtImg;
        }


        buttonInstance.PointerUpTriggerEvent(clickItem, ClickItem);

        //
    }


    void ClickItem(PointerEventData dta) {
        DataFile data = dataInstance.dataFile;

        Inventory itm = data.items[(int)item];

        int itemCount = itm.item_count;

        if (itemCount > 0) {
            itemCount -= 1;

            if (itemCount == 0) {
                image.sprite = defualtImg;
            }

            textHolder.text = itemCount.ToString();

            itm.item_count = itemCount;
         
            data.items[(int)item] = itm;

            dataInstance.dataFile = data;

            // use item
        }

   
    }

}
