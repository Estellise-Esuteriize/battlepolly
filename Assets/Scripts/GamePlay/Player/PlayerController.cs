using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : BasePlayer, IEnvironmentData {

    public GamePlayMainController gameplayController;

    public RuntimeAnimatorController[] characterAnim;

    public LayerMask wall;

    public Sprite deadSprite;

    private SpriteRenderer upperSprite;
    private SpriteRenderer lowerSprite;

    private bool bossRoom;

    [HideInInspector]
    public bool inGame;
    [HideInInspector]
    public bool gameOver;

    private string characterStopper = "CharacterStopper";

    private Dictionary<int, EnemyController> enemies;

    private GameplayManager gameManager;

    protected override void Awake() {
        base.Awake();

        verticalRayCount = 4;
    }


    protected override void Start() {
        base.Start();

        animator.runtimeAnimatorController = characterAnim[dataInstance.dataFile.character];
        wallMask = wall;
        playerHealth = dataInstance.dataFile.items[(int)Item.Heart].item_count;

        gameManager = GameplayManager.instance;

        enemies = new Dictionary<int, EnemyController>();

        upperSprite = transform.GetChild(0).GetComponent<SpriteRenderer>();
        lowerSprite = transform.GetChild(1).GetComponent<SpriteRenderer>();

        playerHealth = 1;
        
    }


    IEnumerator Movement() {

        while (inGame && !gameOver) {

            Vector3 velocity = Vector3.zero;


            MoveHorizontal(ref velocity);
            MoveVertical(ref velocity);

            upperSprite.sortingOrder = Mathf.RoundToInt((rgbody.transform.position.y + .5f) * -10);
            lowerSprite.sortingOrder = Mathf.RoundToInt((rgbody.transform.position.y + .5f) * -10) - 1;

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

#if UNITY_ANDROID || UNITY_IOS

#elif UNITY_STANDALONE || UNITY_EDITOR

            if (Input.GetKey(KeyCode.Space)) {

                animator.SetBool("Attack", true);

                yield return null;

                animator.SetBool("Attack", false);


            }

            yield return null;

#endif


        }
        

    }
   

    void MoveHorizontal(ref Vector3 velocity) {
        if (bossRoom)
            return;
#if UNITY_ANDROID || UNITY_IOS
        velocity.x = 1f;
#elif UNITY_EDITOR || UNITY_STANDALONE
        velocity.x = 1f;
#endif

    }

    void MoveVertical(ref Vector3 velocity) {
#if UNITY_ANDROID || UNITY_IOS

        //detect input detection from user


#elif UNITY_EDITOR || UNITY_STANDALONE

        velocity.y = Input.GetAxisRaw("Vertical");

#endif
    }

    protected new void OnTriggerEnter2D(Collider2D collision) {
        //base.OnTriggerEnter2D(collision);

        if (collision.gameObject.CompareTag(characterStopper)) {
            print("stop character movement");
            bossRoom = true;
        }
        else if (collision.CompareTag(PoolingKey.Enemies.ToString()) || collision.CompareTag(PoolingKey.Enemies.ToString())) {

            int myY = (int)rgbody.transform.position.y;
            int enemyY = (int)collision.transform.position.y;
           
            if (myY == enemyY - 1 || myY == enemyY + 1 || myY == enemyY) {

                EnemyController enemy = null;

                if (!enemies.ContainsKey(collision.gameObject.GetInstanceID())) {

                    enemy = collision.gameObject.GetComponent<EnemyController>();

                    enemies.Add(collision.gameObject.GetInstanceID(), enemy);

                }
                else {
                    enemy = enemies[collision.gameObject.GetInstanceID()];
                }

                enemy.TakeDamage();

            }

        }

    }
    



    public void CharacterStartPosition(Vector3 spawn) {

        transform.position = spawn;

    }

    protected override void PlayerHealth(int health) {
        

        if (playerHealth <= 0) {

            gameOver = true;

            float additionalY = deadSprite.bounds.max.y;

            Vector3 newPositon = rgbody.transform.position;

            newPositon.y -= additionalY;

            animator.SetBool(defaultDeadName, true);

            StartCoroutine("CharaDead", newPositon);

            //


        }


        //gameplayController.currentHeart.text = health.ToString();
    }

    IEnumerator CharaDead(Vector3 newPosition) {

        yield return new WaitForSeconds(1.25f);

        rgbody.transform.position = newPosition;

    }

}

