using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : BasePlayer, IEnvironmentData {

    public RuntimeAnimatorController[] characterAnim;

    public PlayerControl moveUp;
    public PlayerControl moveDown;
    public PlayerControl attack;

    private GamePlayItem heartItem;

    private Animator joystickAnimator;

    public LayerMask wall;

    public Sprite deadSprite;

    private SpriteRenderer upperSprite;
    private SpriteRenderer lowerSprite;

    private bool bossRoom;

    [HideInInspector]
    public int hitTracker;
    [HideInInspector]
    public bool inGame;
    [HideInInspector]
    public bool gameOver;

    private string characterStopper = "CharacterStopper";


    GamePlayMainController playUIController;
    GameplayManager playController;
    SoundController sound;
    GameController instance;

    protected override void Awake() {
        base.Awake();

        verticalRayCount = 4;


    }


    protected override IEnumerator Start() {
        yield return base.Start();

        instance = GameController.instance;

        while (instance == null) {
            instance = GameController.instance;

            yield return new WaitForSeconds(.1f);

        }

        instance.AddComponentForReference(this);

        dataInstance = instance.dataController;
        sound = instance.soundController;
    
        playController = instance.GetComponentForReference<GameplayManager>();
        playUIController = instance.GetComponentForReference<GamePlayMainController>();

        while (playController == null || playUIController == null) {

            playController = instance.GetComponentForReference<GameplayManager>();
            playUIController = instance.GetComponentForReference<GamePlayMainController>();

            yield return new WaitForSeconds(.1f);
        }


        animator.runtimeAnimatorController = characterAnim[dataInstance.dataFile.character];
        wallMask = wall;
        playerHealth = dataInstance.dataFile.items[(int)Item.Heart].item_count;


        upperSprite = transform.GetChild(0).GetComponent<SpriteRenderer>();
        lowerSprite = transform.GetChild(1).GetComponent<SpriteRenderer>();

        joystickAnimator = transform.Find("Controls/Controller/PlayerMovement").GetComponent<Animator>();

        heartItem = transform.Find("Controls/Controller/Inventory/HeartItem").GetComponent<GamePlayItem>();

        playerHealth = 100;

    }


    IEnumerator Movement() {


        while (inGame && !gameOver) {

            Vector3 velocity = Vector3.zero;


            MoveHorizontal(ref velocity);
            MoveVertical(ref velocity);

            float sortingDir = Mathf.Sign(rgbody.transform.position.y + -.7f);
            string sorting = Mathf.Abs(rgbody.transform.position.y + -.7f).ToString();

            int sortingOrder;

            try {
                System.Int32.TryParse(sorting[0].ToString() + sorting[2].ToString(), out sortingOrder);

                int sortingOrdered = sortingOrder * ((sortingDir == -1) ? 1 : -1);
                
                upperSprite.sortingOrder = sortingOrdered + 1;
                lowerSprite.sortingOrder = sortingOrdered;
            }
            catch (System.IndexOutOfRangeException ex) {
                ex.ToString();

                upperSprite.sortingOrder = 0 + 1;
                lowerSprite.sortingOrder = 0;

            }

            MovePlayer(velocity);

            yield return null;
        }

        if (!inGame && !gameOver) {
            // game complete


        }
        else if (!inGame && gameOver) {
            // game over
        }

    }

    IEnumerator Attack() {
        


        while (inGame && !gameOver) {

            /*
             * Code in the following line are for testing user attack input on mobile
             * -> start
             * 
             * /
             
            if (attack.isPressed) {

                animator.SetBool("Attack", true);

                yield return null;

                animator.SetBool("Attack", false);

                attack.isPressed = false;
            }

            /*
             * -> end 
             */


            /*
             * Code in the following lines are for live
             * -> uncoment lines if building app
             * -> start
             */

#if UNITY_ANDROID || UNITY_IOS

            if (attack.isPressed) {

                animator.SetBool("Attack", true);

                yield return null;

                animator.SetBool("Attack", false);

                attack.isPressed = false;
            
                sound.PlayAttackSfx();
                
            }


#elif UNITY_STANDALONE || UNITY_EDITOR

            if (Input.GetKey(KeyCode.Space)) {

                animator.SetBool("Attack", true);

                sound.PlayAttackSfx();

                yield return new WaitForSeconds(.1f);

                animator.SetBool("Attack", false);

            }


#endif
            /*
             * -> end 
             */
            yield return null;

        }
        

    }

    void MoveHorizontal(ref Vector3 velocity) {

        if (bossRoom)
            return;

        velocity.x = 1f;

    }

    void MoveVertical(ref Vector3 velocity) {

        /*
         * Code in the following line are for testing
         * -> start
         * /

        velocity.y = moveUp.movingPressed + (moveDown.movingPressed * -1f);

        joystickAnimator.SetFloat("Movement", velocity.y);

        /*
         * -> end
         */

        /*
         * Code in the following line are for building
         * uncomment if building the game
         * -> start
         */

#if UNITY_ANDROID || UNITY_IOS

        velocity.y = moveUp.movingPressed + (moveDown.movingPressed * -1f);
        
        joystickAnimator.SetFloat("Movement", velocity.y);

#elif UNITY_EDITOR || UNITY_STANDALONE

        velocity.y = Input.GetAxisRaw("Vertical");

        joystickAnimator.SetFloat("Movement", velocity.y);

#endif
        /*
         * -> end
         */

    }



    protected new void OnTriggerEnter2D(Collider2D collision) {

        if (bossRoom)
            return;

        bool isStopper = collision.gameObject.CompareTag(characterStopper);

        if (isStopper) {
       
            bossRoom = true;

        }

    }


    public void StopMovement() {
        Vector3 m = Vector3.zero;
        MovePlayer(m);
    }


    public void CharacterStartPosition(Vector3 spawn) {

        transform.position = spawn;

    }

    protected override void PlayerHealth(int health) {

        /*
         * Code in the following lines are for live
         * -> Uncoment this if building app
         * -> start
         */
            DataFile data = dataInstance.dataFile;

            Inventory items = data.items[0];

            playerHealth = items.item_count;
            playerHealth = (playerHealth <= 0) ? playerHealth : --playerHealth;

            items.item_count = playerHealth;

            data.items[0] = items;

            dataInstance.dataFile = data;

            playUIController.currentHeart.text = playerHealth.ToString();

            heartItem.UsedItem(playerHealth);
         /*
          * -> end
          */

        /*
         * Code in the following lines are for testing the app
         * -> comment this if testing
         * -> start
         * /

        playerHealth--;

        /*
         * -> end
         */

        if (playerHealth <= 0) {

            gameOver = true;

            animator.SetBool("Attack", false);

            float additionalY = deadSprite.bounds.max.y;

            Vector3 newPositon = rgbody.transform.position;

            newPositon.y -= additionalY;

            animator.SetBool(defaultDeadName, true);

            upperSprite.sortingOrder = 10000;
            lowerSprite.sortingOrder = 9999;

            StartCoroutine("CharaDead", newPositon);

            playController.StartCoroutine("GameOver");
            playUIController.StartCoroutine("GameOver");


        }

        hitTracker++;

        //gameplayController.currentHeart.text = health.ToString();
    }

    IEnumerator CharaDead(Vector3 newPosition) {

        yield return new WaitForSeconds(1.25f);

        rgbody.transform.position = newPosition;

    }

}

