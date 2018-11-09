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

    private GameplayManager playInstance;

    private ContactFilter2D contactFilter;

    private bool takeDamage;

    private bool hasRendered;

    void Awake() {

        rgbody = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();

    }

    void Start() {

        playInstance = GameplayManager.instance;

        sprite.sprite = defaultSprite;
        anim.runtimeAnimatorController = enemyAnim;

        if (useSetBoxCollider) {
            boxCollider.size = sprite.bounds.size;
            boxCollider.offset = new Vector2(0, 0);
        }


        playInstance.SetEnemies(this);

        contactFilter.useTriggers = false;
        contactFilter.SetLayerMask(playerMask);
        contactFilter.useLayerMask = true;
    

    }
    
    

    public void StartEnemy() {

        if (enemy == Enemy.BossMonster)
            return;

        StartCoroutine("Movement");

    }

    void OnWillRenderObject() {

        if (enemy != Enemy.BossMonster || hasRendered)
            return;

        hasRendered = true;

        StopCoroutine("DetectPlayer");
        StartCoroutine("DetectPlayer");
       
    }

    public void TakeDamage() {

        if (enemy == Enemy.Obstacle || takeDamage)
            return;

        takeDamage = true;

        StopCoroutine("Movement");

        StartCoroutine("TakingDamage");


        StartCoroutine("Movement");
   

    }

    IEnumerator TakingDamage() {

        enemyHealth--;

        Vector3 position = Vector3.right * knockBack * Time.deltaTime;

        rgbody.MovePosition(rgbody.transform.position + position);


        if (enemyHealth <= 0) {

            boxCollider.enabled = false;

            anim.SetInteger("Health", enemyHealth);

            StartCoroutine("DamageAction", deathAnim.length);

        }

        yield return new WaitForSeconds(.3f);

        takeDamage = false;

    }


    IEnumerator DamageAction(float miniSec) {

        yield return new WaitForSeconds(miniSec);

        gameObject.SetActive(false);

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


    IEnumerator DetectPlayer() {

        RaycastHit2D[] hitBuffer = new RaycastHit2D[5];
        List<RaycastHit2D> hitBufferList = new List<RaycastHit2D>();

        float length = sprite.bounds.size.x;

        print(length);

        while (enemyHealth > 0) {
           

            yield return new WaitForSeconds(5f);


            int count = rgbody.Cast(Vector2.left, contactFilter, hitBuffer, length);

            print("Count : " + count);

            hitBufferList.Clear();

            for (int i = 0; i < count; i++) {
                hitBufferList.Add(hitBuffer[i]);
            }

            /*for (int i = 0; i < hitBufferList.Count; i++) {
                Vector2 normal = hitBufferList[i].normal;

                print(normal);
                print("Transform " + hitBuffer[i].transform.position);
                print(hitBufferList[i].distance);
                
            }*/

            if (hitBufferList.Count > 0) {

                anim.SetTrigger("Attack");

                float offsetY = (attackBossSprite.bounds.max.y / 1.5f) + .3f;
               
                Vector3 newPos = transform.position;
                newPos.y += offsetY;
                
                transform.position = newPos;

                yield return new WaitForSeconds(1f);





            }






            yield return null;
        }


    }

    
  
}
