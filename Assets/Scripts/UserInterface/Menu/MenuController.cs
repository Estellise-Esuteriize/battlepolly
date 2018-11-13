using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuController : MonoBehaviour {

    public GameObject storyBoard;

    public EventTrigger resetData;

    public MenuEventButton play;
    public MenuEventButton shop;
    public MenuEventButton shopBack;
    public MenuEventButton levelBack;


    private Transform levelMenu = null;

    private bool firstRun = false;

    DataController dataInstance;
    ButtonHelperController buttonHelper;
    Loader lInstance;
    GameController instance;

    IEnumerator Start() {
        instance = GameController.instance;
        buttonHelper = instance.buttonHelper;
        dataInstance = instance.dataController;
        lInstance = instance.loader;

        while (lInstance == null) {

            lInstance = instance.loader;

            yield return new WaitForSeconds(.1f);
        }

        buttonHelper.PointerUpTriggerEvent(shop.button, Shop);
        buttonHelper.PointerUpTriggerEvent(shopBack.button, BackFromShop);
        buttonHelper.PointerUpTriggerEvent(play.button, Play);
        buttonHelper.PointerUpTriggerEvent(levelBack.button, LevelBack);

        buttonHelper.PointerUpTriggerEvent(resetData, ResetData);

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



        /* 
         * Loading Test
         * -> Code in this section are for testing the loading screen
         * -> Start
         * 
         * lInstance.StartCoroutine("Loading", 0);
         * 
         * -> End
         /**/

    }


    void LevelBack(PointerEventData data) {

        GameObject[] parms = new GameObject[] { levelBack.currentWindow, levelBack.newWindow };

        lInstance.ShowBlackScreen(parms);
    }



    /*
     * Method for Resetting Json Data
     */
    void ResetData(PointerEventData data) {

        string filename = "/DataFile.json";
        string path = Application.persistentDataPath;
        string file = path + filename;

        if (File.Exists(file)) {
            File.Delete(file);
        }

        lInstance.StartCoroutine("Loading", 0);

    }


}

[System.Serializable]
public struct MenuEventButton {

    public GameObject currentWindow;
    public GameObject newWindow;
    public EventTrigger button;
}
