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

    public int enemyHealth;

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

        if (!gameObject.activeInHierarchy)
            return;

        if (enemy == Enemy.BossMonster && hasRendered)
            StartCoroutine("BossAttackPattern");
        
        StartCoroutine("Movement");
        

    }

    public void PauseEnemy() {

        if (!gameObject.activeInHierarchy)
            return;

        if (enemy == Enemy.BossMonster)
            StopCoroutine("BossAttackPattern");

        StopCoroutine("Movement");

    }

    void OnBecameVisible() {

        if (enemy != Enemy.BossMonster || hasRendered)
            return;

        hasRendered = true;

        StopCoroutine("BossAttackPattern");
        StartCoroutine("BossAttackPattern");
    }


    void OnBecameInvisible() {

        if (enemy != Enemy.BossMonster) {
            StopAllCoroutines();
            gameObject.SetActive(false);
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

        Vector3 position = Vector3.right * knockBack * Time.deltaTime;

        rgbody.MovePosition(rgbody.transform.position + position);


        if (enemyHealth <= 0) {

            boxCollider.enabled = false;

            anim.SetInteger("Health", enemyHealth);

            if (gameObject.tag == "Boss")
                playInstance.StartCoroutine("CompleteGame", transform);


            StopCoroutine("BossAttackPattern");

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

        bossCurrentPosition = transform.position;

        float randomSecWait = Random.Range(2f, 6f);

        yield return new WaitForSeconds(randomSecWait);

        yield return StartCoroutine("BossDetect");
        yield return StartCoroutine("BossMoveTowardsPlayer");


        if (enemyHealth > 0) {
             StartCoroutine("BossAttackPattern");
        }


    }

    IEnumerator BossDetect() {

        hitBufferList.Clear();

        float length = sprite.bounds.size.x;
        

        while (hitBufferList.Count < 1) {

            int count = rgbody.Cast(Vector2.left, contactFilter, hitBuffer, length);
            
            hitBufferList.Clear();

            for (int i = 0; i < count; i++) {
                hitBufferList.Add(hitBuffer[i]);
            }


            yield return null;
        }

      

    }

    IEnumerator BossMoveTowardsPlayer() {

        float offsetY = (attackBossSprite.bounds.size.y / 2.9f);

        Vector3 newPos = transform.position;
        newPos.y += offsetY;

        transform.position = newPos;

        anim.SetTrigger("Attack");

        yield return new WaitForSeconds(.001f);
        
        AnimatorClipInfo[] clipInfo = anim.GetCurrentAnimatorClipInfo(0);

        yield return new WaitForSeconds(clipInfo[0].clip.length);

        RaycastHit2D hit = hitBufferList[0];

        /*print(hit.normal + " Normal");
        print(hit.distance + " Distance");
        print(hit.normal.magnitude + " Magnitude");
        print(hit.transform.position.magnitude + " Transform Magnitude");
        print(hit.transform.position + " Transform Position");*/

        float distance = hit.distance;
        float originalDistance = distance;
        float speed = 2f;
        float additionalHeight = 1.5f;

        Vector3 moveToPosition = hit.transform.position;
        moveToPosition.x += hit.normal.magnitude;

        Vector3 bossNewPos = Vector3.zero;

        while (distance >   additionalHeight + .1f) {

            bossNewPos = Vector3.MoveTowards(transform.position, moveToPosition, originalDistance * speed * Time.deltaTime);
            bossNewPos.y = (offsetY + additionalHeight) + moveToPosition.y;
            transform.position = bossNewPos;

            distance = Vector3.Distance(transform.position, moveToPosition);
            distance += -offsetY;


            yield return null;

        }

        anim.SetTrigger("FallDown");

        yield return new WaitForSeconds(.001f);

        float sortingDir = Mathf.Sign(transform.position.y);
        string sorting = Mathf.Abs(transform.position.y + 7f).ToString();

        int sortingOrder;

        try {
            System.Int32.TryParse(sorting[0].ToString() + sorting[2].ToString(), out sortingOrder);

            int sortingOrdered = sortingOrder * ((sortingDir == -1) ? 1 : -1);

            sprite.sortingOrder = sortingOrdered;
        }
        catch (System.IndexOutOfRangeException ex) {
            ex.ToString();
            
            sprite.sortingOrder = 0;

        }

        clipInfo = anim.GetCurrentAnimatorClipInfo(0);

        yield return new WaitForSeconds(clipInfo[0].clip.length);

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

        yield return new WaitForSeconds(3f);
    
        transform.position = bossCurrentPosition;
        
    }



}
