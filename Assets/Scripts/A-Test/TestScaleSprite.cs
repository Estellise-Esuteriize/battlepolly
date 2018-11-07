using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScaleSprite : MonoBehaviour {



    public float movingGroundSpeed;

    public GameObject groundHolder;

    public Sprite[] grounds;


    public BoxCollider2D movementStopper;


    public bool moveGround;

    private SpriteRenderer[] sr;


    void Start() {

        /*sr = GetComponentsInChildren<SpriteRenderer>();

        if (sr.Length < 1) return;



        //transform.localScale.x = worldScreenWidth / width;

        //transform.localScale.y = worldScreenHeight / height;


        double worldScreenHeight = 2d * Camera.main.orthographicSize;
        double worldScreenWidth = Camera.main.orthographicSize * 2d * Screen.width / Screen.height;

        for (int i = 0; i < sr.Length; i++) {

            //sr[i].transform.localScale = new Vector3(1, 1, 1);

            Vector3 newPosition = Vector3.zero;
            float pos = (float)worldScreenWidth * i;
            //newPosition.x = (float)System.Math.Round(pos, 1);

            newPosition.x = Mathf.Round(pos);

            /*float width = sr[i].sprite.bounds.size.x;
            float height = sr[i].sprite.bounds.size.y;

            Vector3 newScale = transform.localScale;
            newScale.x = ((float)worldScreenWidth / width) + .0002f;
            newScale.y = ((int)worldScreenHeight / height);
            //transform.localScale = xWidth;
            //Vector3 yHeight = transform.localScale;

            sr[i].transform.localScale = newScale;

            print("Width " + width);
            print("Height " + height);

            print("QWE " + newPosition);

            print("World Screen Height " + worldScreenHeight);
            print("world Screen Width " + worldScreenWidth);
            

            sr[i].transform.localPosition = newPosition;

        }
        */
        InitGrounds();

        

    }


    void InitGrounds() {


        //double worldScreenHeight = 2d * Camera.main.orthographicSize;
        //double worldScreenWidth = Camera.main.orthographicSize * 2d * Screen.width / Screen.height;


        for (int i = 0; i < grounds.Length; i++) {

            Vector3 newPosition = Vector3.zero;

            float groundWidth = grounds[i].bounds.size.x;

            float distance = groundWidth * i;

            newPosition.x = distance;



            GameObject obs = Instantiate(groundHolder, transform.position, Quaternion.identity);
            obs.transform.SetParent(transform);
            obs.transform.position = newPosition;


            SpriteRenderer sprite = obs.GetComponent<SpriteRenderer>();
            sprite.sprite = grounds[i];

            if (i + 1 == grounds.Length) {
               
                Vector3 movementStopperSize = Vector3.one;
                movementStopperSize.y = grounds[i].bounds.size.y;

                movementStopper.size = movementStopperSize;

                Vector3 position = Vector3.zero;

                float spriteWidth = grounds[i].bounds.size.x;
               
                position.x = distance - ( spriteWidth / 2 );

                movementStopper.transform.position = position;

            }



        }
        


    }


    void Update() {

        if (moveGround) {
            transform.Translate(Vector3.left * Time.deltaTime);
        }



    }


   
}
