using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private AttackDetails attackDetails;

    private float speed;
    private float travelDistance;
    private float xStartPos;
    public bool sendDamageToParent = false;
    public bool isBullet = false;

    [SerializeField]
    private float gravity;
    [SerializeField]
    private float damageRadius;

    private Rigidbody2D rb;

    private bool isGravityOn;
    private bool hasHitGround;

    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private LayerMask whatIsPlayer;
    [SerializeField] private Transform damagePosition;

    private ObjectPool objectPool; // Reference to the pool

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        rb.gravityScale = 0.0f;
        rb.linearVelocity = transform.right * speed;

        isGravityOn = false;

        xStartPos = transform.position.x;
    }

    private void Update()
    {
        if (!isBullet && !hasHitGround)
        {
            attackDetails.position = transform.position;

            if (isGravityOn)
            {
                float angle = Mathf.Atan2(rb.linearVelocityY, rb.linearVelocityX) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }
        }
    }

    private void FixedUpdate()
    {
        if (!hasHitGround)
        {
            Collider2D damageHit = Physics2D.OverlapCircle(damagePosition.position, damageRadius, whatIsPlayer);
            Collider2D groundHit = Physics2D.OverlapCircle(damagePosition.position, damageRadius, whatIsGround);

            if (damageHit)
            {
                if (damageHit.name == "Boss")
                {
                    Debug.Log(attackDetails.damageAmount);
                    damageHit.SendMessage("TakeDamage", (int)attackDetails.damageAmount);
                }
                else
                {
                    if (sendDamageToParent)
                    {
                        damageHit.transform.parent.SendMessage("Damage", attackDetails);
                    }
                    else
                    {
                        damageHit.transform.SendMessage("Damage", attackDetails);
                    }
                }
                //Destroy(gameObject);
            }

            float currentProjectileDistance = Mathf.Abs(xStartPos - transform.position.x);

            if (isBullet)
            {
                // Destroy the bullet instantly after traveling its maximum distance
                if (currentProjectileDistance >= travelDistance)
                {
                    //Destroy(gameObject);
                    ReturnToPool();
                }
            }
            else 
            {
                if (groundHit)
                {
                    hasHitGround = true;
                    rb.gravityScale = 0f;
                    rb.linearVelocity = Vector2.zero;

                    // the arrow is destroyed after a while
                    //Destroy(gameObject, 5f);
                    ReturnToPool(5f);

                }


                if (currentProjectileDistance >= travelDistance && !isGravityOn)
                {
                    isGravityOn = true;
                    rb.gravityScale = gravity;
                }
            }
        }
    }

    public void Initialize(ObjectPool pool)
    {
        objectPool = pool;
    }

    public void ReturnToPool(float time=0f)
    {
        if (objectPool != null)
        {
            objectPool.ReturnObject(gameObject);
        }
        else
        {
            Destroy(gameObject, time); // Fallback if no pool is assigned
        }
    }


    public void FireProjectile(float speed, float travelDistance, float damage)
    {
        this.speed = speed;
        this.travelDistance = travelDistance;
        attackDetails.damageAmount = damage;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(damagePosition.position, damageRadius);
    }
}