using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Enemy {
    Obstacle, MiniMonster, MediumMonster, PlasticMonster, BossMonster
}

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
public class EnemyController : MonoBehaviour {

    public LayerMask playerMask;

    public Enemy enemy;

    public Sprite attackBossSprite;

    public float enemySpeed;

    public float knockBack;

    [Range(0, 5)]
    public float stopMovementPosition;

    public int defaultEnemyHealth;

    public bool useSetBoxCollider;
    


    public AnimationClip deathAnim;

    public Sprite defaultSprite;

    public RuntimeAnimatorController enemyAnim;

    private Animator anim;

    private SpriteRenderer sprite;

    private Rigidbody2D rgbody;

    private BoxCollider2D boxCollider;

    private Vector3 movement;
    private Vector3 bossCurrentPosition;

    private GameplayManager playInstance;

    private ContactFilter2D contactFilter;

    private bool takeDamage;

    private bool hasRendered;

    private int enemyHealth;

    private RaycastHit2D[] hitBuffer = new RaycastHit2D[5];
    private List<RaycastHit2D> hitBufferList = new List<RaycastHit2D>(5);

    GameController instance;

    void Awake() {

        rgbody = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();

    }

    IEnumerator Start() {

        instance = GameController.instance;

        while (instance == null) {

            instance = GameController.instance;

            yield return new WaitForSeconds(.1f);

        }

        playInstance = instance.GetComponentForReference<GameplayManager>();

        while (playInstance == null) {

            playInstance = instance.GetComponentForReference<GameplayManager>();

            yield return new WaitForSeconds(.1f);
        }


        playInstance.SetEnemies(this);

        sprite.sprite = defaultSprite;
        anim.runtimeAnimatorController = enemyAnim;

        if (useSetBoxCollider) {
            boxCollider.size = sprite.bounds.size;
            boxCollider.offset = new Vector2(0, 0);
        }

        contactFilter.useTriggers = false;
        contactFilter.SetLayerMask(playerMask);
        contactFilter.useLayerMask = true;
    
    }

    void OnEnable() {

        boxCollider.enabled = true;

        enemyHealth = defaultEnemyHealth;

        takeDamage = false;

        StartCoroutine("OnStart");
    }

    IEnumerator OnStart() {

        if (instance != null) {
            playInstance = instance.GetComponentForReference<GameplayManager>();

            while (playInstance == null) {

                playInstance = instance.GetComponentForReference<GameplayManager>();

                yield return new WaitForSeconds(.1f);
            }

            playInstance.SetEnemies(this);
        }

    }

    public void StartEnemy() {

        if (!gameObject.activeInHierarchy || enemy == Enemy.Obstacle)
            return;

        if (enemy == Enemy.BossMonster || hasRendered) {
            StopCoroutine("BossAttackPattern");
            StopCoroutine("BossMovement");
            StopCoroutine("BossDetect");
            StopCoroutine("BossMoveTowardsPlayer");
            //
            StartCoroutine("BossAttackPattern");
        }
        else if(enemy != Enemy.BossMonster){
            StartCoroutine("Movement");
        }
        


    }

    public void PauseEnemy() {

        if (!gameObject.activeInHierarchy)
            return;

        if (enemy == Enemy.BossMonster) {
            StopCoroutine("BossAttackPattern");
            StopCoroutine("BossMovement");
            StopCoroutine("BossDetect");
            StopCoroutine("BossMoveTowardsPlayer");
        }
        else 
            StopCoroutine("Movement");

    }

    IEnumerator OnBecameVisible() {

        if (enemy != Enemy.BossMonster || hasRendered) 
            yield break;
        
        hasRendered = true;

        StopCoroutine("BossAttackPattern");
        StopCoroutine("BossMovement");
        StopCoroutine("BossDetect");
        StopCoroutine("BossMoveTowardsPlayer");
        yield return new WaitForSeconds(1f);
        StartCoroutine("BossAttackPattern");
        
    }


    void OnBecameInvisible() {

        if (enemy != Enemy.BossMonster) {
            gameObject.SetActive(false);
        }
        else if (hasRendered) {
            StopAllCoroutines();
            hasRendered = false;
        }

    }

    public void TakeDamage() {

        if (enemy == Enemy.Obstacle || takeDamage)
            return;

        takeDamage = true;

        StopCoroutine("Movement");

        StartCoroutine("TakingDamage");


        StartCoroutine("Movement");

    }

    public void BossTakeDamage() {

        if (takeDamage)
            return;

        takeDamage = true;

        anim.SetTrigger("Got_Hit");

        StartCoroutine("TakingDamage");

    }

    IEnumerator TakingDamage() {
        
        enemyHealth--;

        Vector3 position = Vector3.right;
        position.x *= knockBack;

        position *= Time.deltaTime;

        transform.position = transform.position + position;

        if (enemyHealth <= 0) {

            boxCollider.enabled = false;

            anim.SetInteger("Health", enemyHealth);

            if (gameObject.tag == "Boss")
                playInstance.StartCoroutine("CompleteGame", transform);

            bossCurrentPosition = transform.position;

            StopCoroutine("BossAttackPattern");
            StopCoroutine("BossMoveTowardsPlayer");

            yield return new WaitForSeconds(deathAnim.length);

            gameObject.SetActive(false);


        }

        yield return new WaitForSeconds(.3f);

        takeDamage = false;

    }


    IEnumerator Movement() {
       
        yield return new WaitForSeconds(.5f);

        float xPosition = transform.position.x;


        while (xPosition > stopMovementPosition) {

            Vector3 position = Vector3.left * enemySpeed * Time.deltaTime;

            rgbody.MovePosition(rgbody.transform.position + position);
 
            yield return null;
        }


        gameObject.SetActive(false);

    }


    IEnumerator BossAttackPattern() {

        hitBuffer = new RaycastHit2D[5];
        hitBufferList.Clear();
        
        float randomSecWait = Random.Range(2f, 6f);

        StartCoroutine("BossMovement");

        yield return new WaitForSeconds(randomSecWait);

        yield return StartCoroutine("BossDetect");
        yield return StartCoroutine("BossMoveTowardsPlayer");


        if (enemyHealth > 0) {
             StartCoroutine("BossAttackPattern");
        }

    }

    IEnumerator BossMovement() {

        bool isMoving = true;

        float maxY = 1.07f;
        float minY = -1.25f;

        float minDistance = .0001f;

        float moveToY = Random.Range(minY, maxY);

        Vector3 lastPosition = transform.position;

        while (isMoving) {

            Vector3 toWards = transform.position;
            toWards.y = moveToY;

            Vector3 movement = Vector3.MoveTowards(transform.position, toWards, enemySpeed * Time.deltaTime);
            
            transform.position = movement;

            float distance = Vector3.Distance(toWards, transform.position);


            if (distance < minDistance) 
                isMoving = false;

            

            yield return null;

        }
       
        StartCoroutine("BossMovement");
    }


    IEnumerator BossDetect() {

        hitBufferList.Clear();

        float length = sprite.bounds.size.x + 2f;

        bool isDetecting = true;

        while (isDetecting) {

            int count = rgbody.Cast(Vector2.left, contactFilter, hitBuffer, length);
            
            hitBufferList.Clear();

            if (count > 0) {


                isDetecting = false;

                for (int i = 0; i < count; i++) {
                    hitBufferList.Add(hitBuffer[i]);
                }
            }

            yield return null;
        }

        StopCoroutine("BossMovement");

        bossCurrentPosition = transform.position;

    }

    IEnumerator BossMoveTowardsPlayer() {
        
        float offsetY = (attackBossSprite.bounds.size.y / 2.9f);

        Vector3 newPos = transform.position;
        newPos.y += offsetY;

        transform.position = newPos;

        anim.SetTrigger("Attack");

        yield return new WaitForSeconds(.01f);
        
        AnimatorClipInfo[] clipInfo = anim.GetCurrentAnimatorClipInfo(0);

        yield return new WaitForSeconds(clipInfo[0].clip.length);

        RaycastHit2D hit = hitBufferList[0];

        float distance = hit.distance;
        float originalDistance = distance;
        float speed = 2f;
        float additionalHeight = 1.5f;

        Vector3 moveToPosition = hit.transform.position;
        moveToPosition.x += hit.normal.magnitude;
        moveToPosition.y += -defaultSprite.bounds.size.y / 8f;

        float sortingDir = Mathf.Sign(moveToPosition.y);
        string sorting = Mathf.Abs(moveToPosition.y).ToString();

        int sortingOrder;

        try {
            System.Int32.TryParse(sorting[0].ToString() + sorting[2].ToString(), out sortingOrder);

            int sortingOrdered = sortingOrder * ((sortingDir == -1) ? 1 : -1);

            sprite.sortingOrder = sortingOrdered + 1;
        }
        catch (System.IndexOutOfRangeException ex) {
            ex.ToString();
            sprite.sortingOrder = 0 + 1;
        }


        Vector3 bossNewPos = Vector3.zero;

        while (distance > additionalHeight + .1f) {

            bossNewPos = Vector3.MoveTowards(transform.position, moveToPosition, originalDistance * speed * Time.deltaTime);
            bossNewPos.y = (offsetY + additionalHeight) + moveToPosition.y;
            transform.position = bossNewPos;

            distance = Vector3.Distance(transform.position, moveToPosition);
            distance += -offsetY;

            yield return null;

        }

        anim.SetTrigger("FallDown");

        yield return new WaitForSeconds(.01f);

        clipInfo = anim.GetCurrentAnimatorClipInfo(0);

        yield return new WaitForSeconds(clipInfo[0].clip.length - 0.02f);

        bossNewPos = transform.position;
        bossNewPos.y =  moveToPosition.y + additionalHeight;

        transform.position = bossNewPos;

        moveToPosition = transform.position + (Vector3.right * 2);

        distance = Vector3.Distance(transform.position, moveToPosition);
        originalDistance = distance;

        while (distance > .1f) {

            bossNewPos = Vector3.MoveTowards(transform.position, moveToPosition, originalDistance * speed * Time.deltaTime);
            transform.position = bossNewPos;

            distance = Vector3.Distance(transform.position, moveToPosition);

            yield return null;
        }

        yield return new WaitForSeconds(2.5f);
    
        transform.position = bossCurrentPosition;
        
    }
   


}
