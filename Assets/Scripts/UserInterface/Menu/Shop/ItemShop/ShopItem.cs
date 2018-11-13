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

    [HideInInspector]
    public int index;

    private ButtonHelperController buttonHelper;

    void Start() {

        shopController = gameObject.GetComponentInParent<ShopController>();
        // add listener to eventtriggers

        buttonHelper = GameController.instance.buttonHelper;

        buttonHelper.PointerUpTriggerEvent(showDetails, ShowDetails);
        buttonHelper.PointerUpTriggerEvent(buyItem, BuyItems);

    }


    void ShowDetails(PointerEventData data) {

        if (string.IsNullOrEmpty(set.title)) {
            set = Strings.GetStringSet(index);
        }

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
