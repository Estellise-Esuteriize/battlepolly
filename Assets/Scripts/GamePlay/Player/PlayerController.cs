using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : BasePlayer, IEnvironmentData {

    public RuntimeAnimatorController[] characterAnim;

    public int initialHealth = 10;

    [HideInInspector]
    public PlayerControl moveUp;
    [HideInInspector]
    public PlayerControl moveDown;
    [HideInInspector]
    public PlayerControl attack;

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
    [HideInInspector]
    public bool pause;

    private bool isAttacking;

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

        moveUp = transform.Find("Controls/Controller/PlayerMovement/ArrowUp").GetComponent<PlayerControl>();
        moveDown = transform.Find("Controls/Controller/PlayerMovement/ArrowDown").GetComponent<PlayerControl>();
        attack = transform.Find("Controls/Controller/PlayerAttack").GetComponent<PlayerControl>();

        playerHealth = initialHealth;

        inGame = true;
        pause = false;
        gameOver = false;

    }


    IEnumerator Movement() {


        while (inGame && !gameOver && !pause) {

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
        


        while (inGame && !gameOver && !pause) {

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

            if (attack.isPressed && !isAttacking) {

                isAttacking = true;

                animator.SetBool("Attack", isAttacking);

                yield return new WaitForSeconds(.1f);

                sound.PlayAttackSfx();
            
                yield return new WaitForSeconds(.2f);

                isAttacking = false;

                animator.SetBool("Attack", isAttacking);

                attack.isPressed = false;
                
            }


#elif UNITY_STANDALONE || UNITY_EDITOR

            if (Input.GetKeyUp(KeyCode.Space) && !isAttacking) {

                isAttacking = true;

                animator.SetBool("Attack", isAttacking);

                yield return new WaitForSeconds(.1f);

                sound.PlayAttackSfx();

                yield return new WaitForSeconds(.2f);

                isAttacking = false;

                animator.SetBool("Attack", isAttacking);
               
            }


#endif
            /*
             * -> end 
             */
            yield return null;

        }
        if (pause) {

            isAttacking = false;

            animator.SetBool("Attack", isAttacking);


            pause = false;
            
        }
        else if (!inGame && !gameOver) {

            isAttacking = false;

            animator.SetBool("Attack", isAttacking);
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

        print(velocity.y);
        
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

        hitTracker++;

        playUIController.currentHeart.text = health.ToString();
            
        if (health <= 0) {

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


        //gameplayController.currentHeart.text = health.ToString();
    }

    IEnumerator CharaDead(Vector3 newPosition) {

        yield return new WaitForSeconds(1.25f);

        rgbody.transform.position = newPosition;

    }


    public void AddHeart() {
        playerHealth++;
        playUIController.currentHeart.text = playerHealth.ToString();
    }
}

