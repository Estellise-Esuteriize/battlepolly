using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GamePlayMainController : MonoBehaviour {


    public GameObject pauseWindow;
    public GameObject diedWindow;

    public Text currentHeart;
    public Text currentTrash;

    public EventTrigger pause;

    public EventTrigger restart;
    public EventTrigger menu;
    public EventTrigger play;
    public EventTrigger quit;

    public Text gameStartingText;

    public SpriteRenderer fairy;


    GameController instance;
    ButtonHelperController btnInstance;
    DataController dataInstance;
    GameplayManager playInstance;
    Loader linstance;

    IEnumerator Start() {

        instance = GameController.instance;

        while (instance == null) {
            instance = GameController.instance;

            yield return new WaitForSeconds(.1f);

        }

        instance.AddComponentForReference(this);
        
        btnInstance = GameController.instance.buttonHelper;
        dataInstance = GameController.instance.dataController;


        playInstance = instance.GetComponentForReference<GameplayManager>();
        linstance = instance.loader;

        while (playInstance == null) {

            playInstance = instance.GetComponentForReference<GameplayManager>();
            linstance = instance.loader;

            yield return new WaitForSeconds(.1f);
        }


        DataFile data = dataInstance.dataFile;

        Inventory item = data.items[0];

        currentHeart.text = "10";
        currentTrash.text = data.trash_cash.ToString();

        btnInstance.PointerUpTriggerEvent(pause, PauseGame);
        btnInstance.PointerUpTriggerEvent(play, ResumeGame);
        btnInstance.PointerUpTriggerEvent(quit, QuitGame);


        btnInstance.PointerUpTriggerEvent(restart, RestartGame);
        btnInstance.PointerUpTriggerEvent(menu, ReturnMenu);

        gameStartingText.transform.parent.gameObject.SetActive(true);

    }

    void PauseGame(PointerEventData data) {

        StartCoroutine("PausingGame");

    }

    void ResumeGame(PointerEventData data) {

        StartCoroutine("ResumingGame");

    }

    void QuitGame(PointerEventData data) {

        Application.Quit();

    }

    void RestartGame(PointerEventData data) {
        linstance.StartCoroutine("Loading", SceneManager.GetActiveScene().buildIndex);
    }

    void ReturnMenu(PointerEventData data) {
        linstance.StartCoroutine("Loading", 0);
    } 

    IEnumerator PausingGame() {

        yield return playInstance.StartCoroutine("PauseGame");

        pauseWindow.SetActive(true);
    }

    IEnumerator ResumingGame() {

        pauseWindow.SetActive(false);

        yield return new WaitForSeconds(.5f);

        yield return playInstance.StartCoroutine("ResumeGame");

    }

    IEnumerator GameOver() {

        yield return new WaitForSeconds(.5f);

        diedWindow.SetActive(true);

    }

    public void StartingGame(float time) {

        string sec = "Game Start\n" + ((int)time).ToString();

        gameStartingText.text = sec;

        if (time < .1f) {
            gameStartingText.transform.parent.gameObject.SetActive(false);
        }


    }

    IEnumerator ShowFairy(Vector3 fairyPosition) {

        Vector3 newPosition = fairyPosition;
        newPosition.y -= fairy.sprite.bounds.max.y / 2;

        fairy.gameObject.transform.position = newPosition;

        

        Color color = fairy.color;
        color.a = 0f;

        fairy.color = color;

        fairy.gameObject.SetActive(true);

        float startColor = 0f;

        while (startColor < .9f) {

            startColor += Time.deltaTime;

            Mathf.Clamp01(startColor);

            color.a = startColor;

            fairy.color = color;

            yield return null;

        }

        color.a = 1f;

        fairy.color = color;

        
    }

}
