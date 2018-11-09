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

        playerHealth = 100;
        
    }


    IEnumerator Movement() {

        while (inGame && !gameOver) {

            Vector3 velocity = Vector3.zero;


            MoveHorizontal(ref velocity);
            MoveVertical(ref velocity);

            float sortingDir = Mathf.Sign(rgbody.transform.position.y);
            string sorting = Mathf.Abs(rgbody.transform.position.y).ToString();

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
        

        if (collision.gameObject.CompareTag(characterStopper)) {
            bossRoom = true;
        }
        else if (collision.CompareTag(PoolingKey.Enemies.ToString()) || collision.CompareTag(PoolingKey.Enemies.ToString())) {

            float myY = rgbody.transform.position.y + -.5f;
            float enemyY = collision.transform.position.y;
           
            if ((int)myY == (int)enemyY) {

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

