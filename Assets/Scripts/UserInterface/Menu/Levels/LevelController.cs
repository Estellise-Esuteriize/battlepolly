using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelController : MonoBehaviour {

    public Sprite defaultLevelBackground;
    public Sprite defaultLevelImage;
    public Sprite defaultLevelDesign;

    public LevelDesign[] levelBox;

    public LevelHolder[] levelHolder;

    private int currentLevelPage;
    private int currentLevel;
    private int totalLevel;

    private DataController data;
    private ButtonHelperController bInstance;

    void Start() {

        data = DataController.instance;
        bInstance = ButtonHelperController.instance;

        totalLevel = data.dataFile.level.Length;

        currentLevel = data.dataFile.currentLevel;

        for (int z = 0; z < levelHolder.Length; z++) {
            levelHolder[z].holder.SetActive(false);
            for (int x = 0; x < levelHolder[z].levelholders.Length; x++) {
                for (int c = 0; c < levelHolder[z].levelholders[x].levelButton.Length; c++) {
                    levelHolder[z].levelholders[x].levelButton[c].levelButtonImage = levelHolder[z].levelholders[x].levelButton[c].levelButton.transform.GetChild(0).GetChild(0).GetComponent<Image>();
                }
            }
        }

        InitLevelController();

    }

    public void InitLevelController() {
        currentLevelPage = 0;

        int level = currentLevel % 9;

        int startShowLevel = currentLevel - ((level == 0) ? 9 : level);
        int endShowLevel = currentLevel;


        LevelHolder lvlholder = levelHolder[currentLevelPage];
        lvlholder.holder.gameObject.SetActive(true);

        for (int i = 0; i < 3; i++) {
            int lvlBox = (startShowLevel + (i * 3)) / 3;

            try {
                lvlholder.levelholders[lvlBox].levelBackground.sprite = levelBox[lvlBox].background;
                lvlholder.levelholders[lvlBox].levelDesign.sprite = levelBox[lvlBox].design;
            }
            catch (IndexOutOfRangeException ex) {
                print(ex.StackTrace);
            }
        }

        for (int i = startShowLevel; i < endShowLevel; i++) {

            int countBtn = ((i + 1) - 1) % 3;
            int countHolder = -1;
            countHolder += (countBtn == 0) ? 1 : 0;


            try {
                LevelButtonContent lvl = lvlholder.levelholders[countHolder].levelButton[countBtn];
                Button btn = lvl.levelButton.GetComponent<Button>();
                btn.interactable = true;
                lvl.levelButtonImage.sprite = data.dataFile.level[i].image;
                bInstance.PointerUpTriggerEventWithParameters(lvl.levelButton, (data) => { SetLevelButtonOnClick((PointerEventData)data, i); });
                //bInstance.PointerUpTriggerEvent(lvl.levelButton, SetLevelButtonOnClick);

             }
            catch (Exception ex) {
                if (ex is IndexOutOfRangeException || ex is NullReferenceException) {
                    print(ex.StackTrace);
                }
            }
        }


    }


    void SetLevelButtonOnClick(PointerEventData data, int level) {
        int lvl = this.data.dataFile.level[level].levelIndex;

        // Show Loading


        SceneManager.LoadScene(lvl);
    }

    
}


[System.Serializable]
public struct LevelHolder {

    public GameObject holder;
    public Level[] levelholders;

}

[System.Serializable]
public struct Level {

    public Image levelBackground;
    public Image levelDesign;
    public LevelButtonContent[] levelButton;

}
[System.Serializable]
public struct LevelButtonContent {
    public EventTrigger levelButton;
    public Image levelButtonImage;
}



[System.Serializable]
public struct LevelDesign {
    public Sprite background;
    public Sprite design;
}


