using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

    public static GameController instance = null;

    [HideInInspector]
    public PoolerController pooler;
    [HideInInspector]
    public ButtonHelperController buttonHelper;
    [HideInInspector]
    public DataController dataController;
    [HideInInspector]
    public SoundController soundController;
    [HideInInspector]
    public Loader loader;

    [HideInInspector]
    public GameplayManager playController;

    public Dictionary<System.Type, Component> scripts;

    void Awake() {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        
        buttonHelper = new ButtonHelperController();
       
        pooler = GetComponent<PoolerController>();
          
        dataController = GetComponent<DataController>();
     
        soundController = GetComponent<SoundController>();

        buttonHelper.Init();

        scripts = new Dictionary<System.Type, Component>();
        

        
    }

    public bool SetLoader(Loader load) {
        if (loader == null) {
            loader = load;
            return false;
        }
        else
            return true;
    }


    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void BeforeSceneIsLoaded() {
        SceneManager.sceneUnloaded += SceneManager_sceneUnloaded;
    }

    private static void SceneManager_sceneUnloaded(Scene arg0) {

        if (instance != null) {
            
            instance.scripts = new Dictionary<System.Type, Component>();

        }
        
    }

    public void AddComponentForReference<T>(T component)
        where T : Component {

        if (!scripts.ContainsKey(typeof(T))) {
            scripts.Add(typeof(T), component);
        }
        

    }


    public void AddComponentForReference<T>(T component, out bool usingBool)
        where T : Component {

        if (!scripts.ContainsKey(typeof(T))) {

            scripts.Add(typeof(T), component);

            usingBool = false;
        }
        else {
            usingBool = true;
        }

        

    }

    public T GetComponentForReference<T>()
        where T : Component {

        if (scripts.ContainsKey(typeof(T)))
            return scripts[typeof(T)] as T;

        return null;
    }




}
