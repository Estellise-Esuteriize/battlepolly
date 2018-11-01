using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShopItem : MonoBehaviour {

    public EventTrigger showDetails;
    public EventTrigger buyItem;

    public StringSets set;
    public Sprite image;

    private ShopController shopController;

    private int index;

    void Start() {

        shopController = gameObject.GetComponentInParent<ShopController>();
        // add listener to eventtriggers

        ButtonHelperController.instance.PointerUpTriggerEvent(showDetails, ShowDetails);
        ButtonHelperController.instance.PointerUpTriggerEvent(buyItem, BuyItems);

    }


    void ShowDetails(PointerEventData data) {

        shopController.itemDetails.ShowDetails(set, image);

    }

    void BuyItems(PointerEventData data) {
        shopController.BuyItem(index);
    }


    public void SetIndex(int index) {
        this.index = index;
    }

    public int GetIndex() {
        return index;
    }

    
}
