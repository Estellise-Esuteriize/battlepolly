using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

    public static GameController instance = null;

    [HideInInspector]
    public PoolerController pooler;

    void Awake() {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);


        pooler = GetComponent<PoolerController>();

    }





    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void BeforeSceneIsLoaded() {

        if(instance != null)
            instance.pooler.ResetPooledObjects();


    }



}
