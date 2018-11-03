using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuController : MonoBehaviour {


    public MenuEventButton play;
    public MenuEventButton shop;
    public MenuEventButton shopBack;


    ButtonHelperController buttonHelper;

    void Start() {
        buttonHelper = ButtonHelperController.instance;


        buttonHelper.PointerUpTriggerEvent(shop.button, Shop);
        buttonHelper.PointerUpTriggerEvent(shopBack.button, BackFromShop);
        buttonHelper.PointerUpTriggerEvent(play.button, Play);
    }

    void Shop(PointerEventData data) {
        shop.currentWindow.SetActive(false);
        shop.newWindow.SetActive(true);
    }

    void BackFromShop(PointerEventData data) {
        shopBack.currentWindow.SetActive(false);
        shopBack.newWindow.SetActive(true);
    }

    void Play(PointerEventData data) {
        //play.currentWindow.SetActive(false);
        play.newWindow.SetActive(true);

        play.newWindow.GetComponent<StoryController>().SetTransition();

    }
    
    
}

[System.Serializable]
public struct MenuEventButton {

    public GameObject currentWindow;
    public GameObject newWindow;
    public EventTrigger button;
}
