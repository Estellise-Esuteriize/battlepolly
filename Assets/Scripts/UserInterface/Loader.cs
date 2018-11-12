using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Loader : MonoBehaviour {


    public static Loader instance = null;

    public Image loaderBlackScreen;

    public Image background;
    public Image loading;

    void Awake() {

        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

    }

    public void ShowBlackScreen(GameObject[] parms) {

        StartCoroutine("AnimateBlackScreen", parms);


    }

    IEnumerator AnimateBlackScreen(GameObject[] parms) {
        
        float animateTime = 1f;
        float animateTimeStart = 0f;

        float animateTimeEnd = 1.9f;

        Color color = loaderBlackScreen.color;
        color.a = 0f;

        loaderBlackScreen.color = color;

        loaderBlackScreen.gameObject.SetActive(true);

        while (animateTime < animateTimeEnd) {

            animateTimeStart += Time.deltaTime;

            float alphaColor = Mathf.PingPong(animateTimeStart, 1f);

            color.a = alphaColor;

            loaderBlackScreen.color = color;

            if (alphaColor > .9f) {
                color.a = 1f;
                loaderBlackScreen.color = color;

                parms[0].SetActive(false);
                parms[1].SetActive(true);
            }

            animateTime = animateTimeStart;

            yield return null;

        }

        color.a = 0f;
        loaderBlackScreen.color = color;

        loaderBlackScreen.gameObject.SetActive(false);


    }

    IEnumerator Loading(int level) {

        Color color = background.color;
        color.a = 0f;

        background.color = color;
        background.gameObject.SetActive(true);


        float transition = 0f;
        float speed = 1f;

        while (transition < .9f) {

            transition += speed * Time.deltaTime;
            Mathf.Clamp01(transition);

            color.a = transition;

            background.color = color;

            yield return null;

        }

        loading.gameObject.SetActive(true);

        yield return new WaitForSeconds(1f);

        AsyncOperation operation = SceneManager.LoadSceneAsync(level);

        while (!operation.isDone) {
            
            yield return new WaitForSeconds(operation.progress / .9f);

        }

        loading.gameObject.SetActive(false);

        transition = 1f;

        while (transition > .1f) {

            transition -= speed * Time.deltaTime;

            Mathf.Clamp01(transition);

            color.a = transition;

            background.color = color;

            yield return null;

        }

        background.gameObject.SetActive(false);

    }





}
