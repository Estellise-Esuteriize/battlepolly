using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour {

    public Sprite[] upperPartCharacter;
    public Sprite[] lowerPartCharacter;
    
    private SpriteRenderer upperSprite;
    private SpriteRenderer lowerSprite;

    private int chara = -1;

    private DataController dataInstance;

    void Start() {

        dataInstance = GameController.instance.dataController;

        upperSprite = transform.GetChild(0).GetComponent<SpriteRenderer>();
        lowerSprite = transform.GetChild(1).GetComponent<SpriteRenderer>();

        chara = dataInstance.dataFile.character;
        
        upperSprite.sprite = upperPartCharacter[chara];
        lowerSprite.sprite = lowerPartCharacter[chara];
        
    }
    
}
