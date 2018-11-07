using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuController : MonoBehaviour {

    public GameObject storyBoard;

    public MenuEventButton play;
    public MenuEventButton shop;
    public MenuEventButton shopBack;
    public MenuEventButton levelBack;


    DataController dataInstance;
    ButtonHelperController buttonHelper;
    Loader lInstance;

    private Transform levelMenu = null;

    private bool firstRun = false;

    void Start() {
        buttonHelper = ButtonHelperController.instance;
        dataInstance = DataController.instance;
        lInstance = Loader.instance;

        buttonHelper.PointerUpTriggerEvent(shop.button, Shop);
        buttonHelper.PointerUpTriggerEvent(shopBack.button, BackFromShop);
        buttonHelper.PointerUpTriggerEvent(play.button, Play);
        buttonHelper.PointerUpTriggerEvent(levelBack.button, LevelBack);

        firstRun = dataInstance.dataFile.first_run;

        levelMenu = play.newWindow.transform;

        if (firstRun) {
            GameObject obs = Instantiate(storyBoard) as GameObject;
            obs.transform.SetParent(transform);
            obs.transform.SetAsLastSibling();

            obs.SetActive(false);

            RectTransform rect = obs.GetComponent<RectTransform>();

            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;

            rect.localScale = Vector3.one;

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
        firstRun = dataInstance.dataFile.first_run;

        if (story != null && firstRun) {
            //play.currentWindow.SetActive(false);
            play.newWindow.SetActive(true);
            story.SetTransition(play.currentWindow.transform, levelMenu);
        }
        else {

            GameObject[] parms = new GameObject[] { play.currentWindow, levelMenu.gameObject };

            lInstance.ShowBlackScreen(parms);
        }

    }


    void LevelBack(PointerEventData data) {

        GameObject[] parms = new GameObject[] { levelBack.currentWindow, levelBack.newWindow };

        lInstance.ShowBlackScreen(parms);
    }
    
}

[System.Serializable]
public struct MenuEventButton {

    public GameObject currentWindow;
    public GameObject newWindow;
    public EventTrigger button;
}
