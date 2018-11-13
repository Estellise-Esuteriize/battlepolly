using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(BoxCollider2D))]
public abstract class BasePlayer : MonoBehaviour {

    public float speed;

    public bool useDefaultBoxCollider;

    public Vector3 boxColliderOffset;
    public Vector3 boxColliderSize;


    public RuntimeAnimatorController[] runtimeAnimatorController;

    protected string defaultGotHitName = "GotHit";
    protected string defaultDeadName = "Dead";
    protected string defualtMovementName = "Movement";

    protected int playerHealth = 1;

    protected Animator animator;
    protected Rigidbody2D rgbody;
    [SerializeField]
    protected BoxCollider2D boxCollider;


    protected LayerMask wallMask;

    protected int verticalRayCount;

    private RaycastOrigins raycastOrigins;

    private float verticalRaySpacing;
    private const float skinWidth = 0.015f;
    
    protected DataController dataInstance;
    protected GameController gameInstance;

    [HideInInspector]
    public bool playerDamageable;

    protected virtual void Awake() {

        animator = GetComponent<Animator>();
        rgbody = GetComponent<Rigidbody2D>();

    }

    protected virtual IEnumerator Start() {

        if (!useDefaultBoxCollider) {
            boxCollider.offset = boxColliderOffset;
            boxCollider.size = boxColliderSize;
        }

        CalculateRaySpacing();
        playerDamageable = true;

        yield return null;
    }

    protected virtual void MovementAnimation(string name, float value) {
        animator.SetFloat(name, value);
    }


    protected virtual void OnCollisionEnter2D(Collision2D collision) {

        if (!playerDamageable)
            return;

        bool isEnemies = collision.gameObject.CompareTag(PoolingKey.Enemies.ToString());
        bool isBoss = collision.gameObject.CompareTag(PoolingKey.Boss.ToString());
        
        if (isEnemies || isBoss) {
            PlayerTakeDamage(defaultGotHitName);
        }
    }


    protected virtual void OnTriggerEnter2D(Collider2D collision) {
    
       
    }

    protected virtual void OnTriggerExit2D(Collider2D collision) {
        
    }


    protected virtual void PlayerTakeDamage(string name) {


        if (playerHealth > 0) {

            animator.SetTrigger(name);

            playerHealth--;

            PlayerHealth(playerHealth);

        }



    }

    protected virtual void MovePlayer(Vector3 velocity) {

        float movementAnimation = Mathf.Abs(velocity.x) + Mathf.Abs(velocity.y);

        animator.SetFloat(defualtMovementName, movementAnimation);

        UpdateRaycastOrigins();

        CheckWall(ref velocity);

        rgbody.MovePosition(rgbody.transform.position + velocity * speed * Time.deltaTime);
        
    }


    protected IEnumerator AnimateAttack(string name, bool attack) {
       
        animator.SetBool(name, attack);

        yield return new WaitForSeconds(.5f);

        attack = false;

        animator.SetBool(name, attack);

    }

    void CheckWall(ref Vector3 velocity) {

        float directionY = Mathf.Sign(velocity.y);
        float rayLength = Mathf.Abs(velocity.y) + skinWidth;
       

        for (int i = 0; i < verticalRayCount; i++) {

            Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
            rayOrigin += Vector2.right * (verticalRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, wallMask);

            Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength, Color.red);

            if (hit) {
                velocity.y = (hit.distance - skinWidth) * directionY;
                rayLength = hit.distance;
            }
        }
    }

    void CalculateRaySpacing() {
        Bounds bounds = boxCollider.bounds;
        bounds.Expand(skinWidth * -2f);

        verticalRayCount = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);

        verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
    }

    void UpdateRaycastOrigins() {
        Bounds bounds = boxCollider.bounds;
        bounds.Expand(skinWidth * -2f);


        raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
        raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
        raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);

    }

    protected abstract void PlayerHealth(int health);


    struct RaycastOrigins {


        public Vector2 bottomLeft, bottomRight;
        public Vector2 topLeft, topRight;
    }


}
