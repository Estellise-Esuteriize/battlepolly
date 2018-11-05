using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuController : MonoBehaviour {

    public GameObject storyBoard;

    public MenuEventButton play;
    public MenuEventButton shop;
    public MenuEventButton shopBack;


    DataController dataInstance;
    ButtonHelperController buttonHelper;

    private Transform levelMenu = null;

    private bool firstRun = false;

    void Start() {
        buttonHelper = ButtonHelperController.instance;
        dataInstance = DataController.instance;

        buttonHelper.PointerUpTriggerEvent(shop.button, Shop);
        buttonHelper.PointerUpTriggerEvent(shopBack.button, BackFromShop);
        buttonHelper.PointerUpTriggerEvent(play.button, Play);

        firstRun = dataInstance.dataFile.first_run;

        if (firstRun) {
            GameObject obs = Instantiate(storyBoard) as GameObject;
            obs.transform.SetParent(transform);
            obs.transform.SetAsLastSibling();

            obs.SetActive(false);

            RectTransform rect = obs.GetComponent<RectTransform>();

            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;

            rect.localScale = Vector3.one;

            levelMenu = play.newWindow.transform;
            play.newWindow = obs;
        }
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

        StoryController story = play.newWindow.GetComponent<StoryController>();

        if (story != null) {
            //play.currentWindow.SetActive(false);
            play.newWindow.SetActive(true);
            story.SetTransition(play.currentWindow.transform, levelMenu);
        }
        else {
            play.currentWindow.SetActive(false);
            play.newWindow.SetActive(true);
        }

    }
    
    
}

[System.Serializable]
public struct MenuEventButton {

    public GameObject currentWindow;
    public GameObject newWindow;
    public EventTrigger button;
}
