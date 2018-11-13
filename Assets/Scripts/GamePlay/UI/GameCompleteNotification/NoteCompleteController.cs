using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NoteCompleteController : MonoBehaviour {


    public Sprite unfilledStar;
    public Sprite filledStar;


    public EventTrigger restart;
    public EventTrigger menu;

    public Image[] stars;

    GameController instance;
    ButtonHelperController buttonHelper;
    Loader loader;
    PlayerController playerController;

    RectTransform rect;

    IEnumerator Start() {

        instance = GameController.instance;

        instance.AddComponentForReference(this);

        buttonHelper = instance.buttonHelper;

        playerController = instance.GetComponentForReference<PlayerController>();
        loader = instance.loader;

        while (playerController == null || loader == null) {

            playerController = instance.GetComponentForReference<PlayerController>();
            loader = instance.loader;

            yield return new WaitForSeconds(.1f);
        }

        rect = GetComponent<RectTransform>();


        buttonHelper.PointerUpTriggerEvent(restart, RestartGame);
        buttonHelper.PointerUpTriggerEvent(menu, Menu);

        gameObject.SetActive(false);

    }

    public void ShowNotif() {
        rect.offsetMax = Vector2.zero;
        rect.offsetMin = Vector2.zero;

        int hit = playerController.hitTracker;
        int limit = (hit > 1 && hit < 5) ? 2 : (hit >= 5) ? 1 : 3;

        for (int i = 0; i < limit; i++) {
            stars[i].sprite = filledStar;
        }


        gameObject.SetActive(true);
    }





    void RestartGame(PointerEventData data) {
        loader.StartCoroutine("Loading", SceneManager.GetActiveScene().buildIndex);
    }

    void Menu(PointerEventData data) {
        loader.StartCoroutine("Loading", 0);
    }



}
