using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GamePlayItem : MonoBehaviour {

    public Sprite defualtImg;
    public Sprite haveItmImg;

    public Text textHolder;


    public Item item;
    
    private Button clickBtn;
    private Image image;


    DataController dataInstance;

    GameController instance;

    void Start() {

        instance = GameController.instance;

        dataInstance = instance.dataController;

        clickBtn = transform.GetChild(0).GetComponent<Button>();
        image = clickBtn.GetComponent<Image>();

        DataFile data = dataInstance.dataFile;

        Inventory itm = data.items[(int)item];

        if (itm.item_count > 0) {
            textHolder.text = itm.item_count.ToString();
            image.sprite = haveItmImg;
        }
        else {
            textHolder.text = "0";
            image.sprite = defualtImg;
            clickBtn.interactable = false;
        }


    }

    public void UsedItem(int count) {

        if (count <= 0) {
            textHolder.text = "0";
            image.sprite = defualtImg;
            clickBtn.interactable = false;
        }
        else if (count > 0) {
            textHolder.text = count.ToString();
        }

    }


}
