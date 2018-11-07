using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GamePlayMainController : MonoBehaviour {


    public GameObject pauseWindow;
    public GameObject diedWindow;

    public Text currentHeart;
    public Text currentTrash;

    public EventTrigger pause;
    public EventTrigger music;

    public EventTrigger restart;
    public EventTrigger menu;
    public EventTrigger play;
    public EventTrigger quit;


    ButtonHelperController btnInstance;
    DataController dataInstance;

    void Start() {

        btnInstance = ButtonHelperController.instance;
        dataInstance = DataController.instance;

        DataFile data = dataInstance.dataFile;

        Inventory item = data.items[0];

        currentHeart.text = item.item_count.ToString();
        currentTrash.text = data.trash_cash.ToString();


    }



}
