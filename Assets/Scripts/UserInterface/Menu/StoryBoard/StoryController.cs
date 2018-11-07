using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StoryController : MonoBehaviour {

    public GameObject storyWindow;
    public GameObject characterSelection;

    public Image transition;
    public Image imageHolder;
    public StorySet[] stories;
    public Text textHolder;
    public EventTrigger nextStoryButton;

    public EventTrigger maleCharacterButton;
    public EventTrigger femaleCharacterButton;


    private int currentPage = 0;
    private int maxPage = 0;

    private ButtonHelperController bInstance;

    private bool isStillTransitioning = true;
    private bool doneFadeInText = false;

    private Transform level;
    private DataController data;

    void Start() {
        data = DataController.instance;
        bInstance = ButtonHelperController.instance;

        bInstance.PointerUpTriggerEvent(nextStoryButton, OnClickNext);
        bInstance.PointerUpTriggerEvent(maleCharacterButton, SelectMaleCharacter);
        bInstance.PointerUpTriggerEvent(femaleCharacterButton, SelectFemaleCharacter);

        maxPage = stories.Length;

        imageHolder.sprite = stories[0].image;

    }


    public void SetTransition(Transform currentWindow, Transform lvl) {

        currentPage = 0;

        level = lvl;

        StartCoroutine("OpenStories", currentWindow);


    }

    void OnClickNext(PointerEventData data) {

        if (currentPage + 1 < maxPage && !isStillTransitioning) {

            StartCoroutine("NextPage");
        }
        else if (isStillTransitioning) {
            doneFadeInText = true;
        }
        else {
            if (!isStillTransitioning) {
                StartCoroutine("ShowCharacterSelection");
            }

        }

    }

    IEnumerator ShowCharacterSelection() {

        DataFile newData = data.dataFile;
        newData.first_run = false;

        data.dataFile = newData;

        yield return new WaitForSeconds(.3f);
        storyWindow.SetActive(false);
        characterSelection.SetActive(true);

    }

    IEnumerator NextPage() {
        currentPage++;

        doneFadeInText = false;
        isStillTransitioning = true;

        textHolder.text = "";
        imageHolder.sprite = stories[currentPage].image;


        yield return new WaitForSeconds(.3f);


        yield return StartCoroutine("FadeInText");
        isStillTransitioning = false;
    }

    IEnumerator OpenStories(Transform currentWindow) {

        GameObject[] parms = new GameObject[] { currentWindow.gameObject, storyWindow };

        yield return StartCoroutine("Transition", parms);
        yield return StartCoroutine("FadeInImage");

        yield return StartCoroutine("FadeInText");

        isStillTransitioning = false;
    }

    /* moving image from left to right */
    /*IEnumerator MoveImage() {

        bool isStillMoving = true;

        float movementSpeed = 500;

        int lastPage = currentPage - 1;
        int newPage = currentPage;
        
        RectTransform image = imageHolder.GetComponent<RectTransform>();

        while (isStillMoving) {

            Vector2 offSetMin = image.offsetMin;
            Vector2 offSetMax = image.offsetMax;

            offSetMin.x -= movementSpeed * Time.deltaTime;
            offSetMax.x -= movementSpeed * Time.deltaTime;


            image.offsetMin = offSetMin;
            image.offsetMax = offSetMax;

            if (image.offsetMin.x <= 0f || image.offsetMax.x <= 0f) 
                isStillMoving = false;
            


            yield return null;
        }
        image.offsetMin = Vector2.zero;
        image.offsetMax = Vector2.zero;

        if (lastPage > -1) {

            imageHolder.sprite = stories[((currentPage + 1) < maxPage) ? currentPage + 1 : 0].image;

            imageHolder.transform.SetAsFirstSibling();

        }
    }
    */

    IEnumerator FadeInImage() {

        float fadeInTime = 0f;
        float speed = .5f;

        Image image = imageHolder.GetComponent<Image>();

        Color color = image.color;
        color.a = 0f;

        image.color = color;

        while (fadeInTime < 1f) {

            fadeInTime += Time.deltaTime * speed;

            color.a = fadeInTime;

            image.color = color;

            yield return null;

        }
    }

    IEnumerator FadeInText() {

        float colorFloat = 0.1f;
        float fadeSpeedMultipliar = 50f;

        string text = Strings.GetStoryString(currentPage);
        string shownText = "";

        int letterCount = 0;
        int colorInt = 0;



        while (letterCount < text.Length && !doneFadeInText) {

            if (colorFloat < 1.0f) {
                colorFloat += Time.deltaTime * fadeSpeedMultipliar;
                colorInt = (int)(Mathf.Lerp(0.01f, 1.0f, colorFloat) * 255f);

                textHolder.text = shownText + "<color=#FFFFFF" + string.Format("{0:X}", colorInt) + ">" + text[letterCount] + "</color>";

            }
            else {
                colorFloat = 0.1f;
                shownText += text[letterCount];
                letterCount++;
            }

            yield return null;

        }

        if (doneFadeInText) {

            while (letterCount < text.Length) {

                textHolder.text = shownText + text[letterCount];

                shownText += text[letterCount];
                letterCount++;

            }

        }

    }



    IEnumerator Transition(GameObject[] window) {

        bool isStillTransitioning = true;
        float isTransitioningTime = 1f;

        Color transitionColor = transition.color;
        transitionColor.a = 0f;

        transition.color = transitionColor;

        float transitionStart = 0f;

        transition.gameObject.SetActive(true);


        while (isStillTransitioning || isTransitioningTime > .1f) {

            transitionStart += Time.deltaTime;

            float trans = Mathf.PingPong(transitionStart, 1f);

            transitionColor.a = trans;
            transition.color = transitionColor;

            if (trans > .9f) {
                // disable current window
                // show new window
                // set is transitionning to false
                transitionColor.a = 1f;
                transition.color = transitionColor;

                window[0].SetActive(false);
                window[1].SetActive(true);

                isStillTransitioning = false;
            }
            else if (!isStillTransitioning)
                isTransitioningTime = trans;

            yield return null;

        }

        transitionColor.a = 0f;
        transition.color = transitionColor;

        transition.gameObject.SetActive(false);

    }
    

    void SelectMaleCharacter(PointerEventData data) {

        SelectCharacter(1);
    }

    void SelectFemaleCharacter(PointerEventData data) {

        SelectCharacter(-1);

    }


    void SelectCharacter(int chara) {
        GameObject[] parms = new GameObject[] { characterSelection, level.gameObject };

        DataFile dta = data.dataFile;

        dta.character = chara;
        dta.first_run = false;

        data.dataFile = dta;

        StartCoroutine("Transition", parms);
    }


    /*
     * Repositioning image for moving image
     * 
    void RepositionImages(int page) {
        
        Vector2 offSetMin = Vector2.zero;
        Vector2 offSetMax = Vector2.zero;

        RectTransform imageHolderTransform = imageHolder.GetComponent<RectTransform>();

        float reposition = imageHolderTransform.rect.width;

        offSetMin.x = reposition;
        offSetMax.x = reposition;
                
        imageHolderTransform.offsetMin = offSetMin;
        imageHolderTransform.offsetMax = offSetMax;
                
    }
    */
    

    [System.Serializable]
    public struct StorySet {

        public Sprite image;

        public int imageNo;

        public string text;

    }

}
