using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoryController : MonoBehaviour {

    public float transitionSpeed = .2f;

    public Image transition;

        





    public void SetTransition() {
        
        StartCoroutine("Transition");
    }



    IEnumerator Transition() {

        bool isStillTransitioning = true;
        float isTransitioningTime = 1f;


        Color transitionColor = transition.color;
        transitionColor.a = 0f;

        transition.color = transitionColor;

        transitionSpeed = 0f;

        float knownDistance = 1f - 0f;
        float howManySecondsForLoop = knownDistance / transitionSpeed;

        float pongTime = howManySecondsForLoop;


        transition.gameObject.SetActive(true);

    

        while (isStillTransitioning || isTransitioningTime > .1f) {

            transitionSpeed += Time.deltaTime;

            float trans = Mathf.PingPong(transitionSpeed, 1f);

            transitionColor.a = trans;
            transition.color = transitionColor;

            if (trans > .9f) {
                // disable current window
                // show new window
                // set is transitionning to false
                isStillTransitioning = false;
            }
            else if (!isStillTransitioning)
                isTransitioningTime = trans;



            print(trans);

            yield return null;
        }

        transitionColor.a = 0f;
        transition.color = transitionColor;

        gameObject.SetActive(false);

    }


}
