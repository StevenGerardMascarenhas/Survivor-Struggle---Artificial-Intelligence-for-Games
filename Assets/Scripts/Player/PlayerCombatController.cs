using UnityEngine;

public class PlayerCombatController : MonoBehaviour
{
    [SerializeField] private bool combatEnabled;
    [SerializeField] private float inputTimer, attack1Radius, attack1Damage;
    [SerializeField] private float stunDamageAmount = 1f;
    
    [SerializeField] private Transform meleeAttackHitBoxPos;
    [SerializeField] private Transform rangeAttackHitBoxPos;


    [SerializeField] private LayerMask whatIsDamageable;

    private bool gotInput, isMeleeAttack, isRangeAttack;

    private float lastInputTime = Mathf.NegativeInfinity;

    private AttackDetails attackDetails;

    private Animator anim;

    private PlayerController playerController;
    private PlayerStats playerStats;

    [SerializeField] private ObjectPool projectilePool;

    public float projectileDamage = 10f;
    public float projectileSpeed = 12f;
    public float projectileTravelDistance = 5f;


    private void Start()
    {
        anim = GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();
        playerStats = GetComponent<PlayerStats>();
        projectilePool = FindFirstObjectByType<ObjectPool>();

    }

    // Update is called once per frame
    void Update()
    {
        CheckCombatInput();
        CheckAttacks();
    }

    private void CheckCombatInput()
    {
        if (Input.GetMouseButtonDown(0) )
        {
            if (combatEnabled)
            {
                gotInput = true;
                lastInputTime = Time.time;
                isMeleeAttack = true;
            }
        }
        else if (Input.GetMouseButtonDown(1)) // Right mouse button for projectile
        {
            if (combatEnabled)
            {
                gotInput = true;
                lastInputTime = Time.time;
                isRangeAttack = true;
            }
        }
    }

    private void CheckAttacks()
    {
        if (gotInput)
        {
            if (isMeleeAttack)
            {
                gotInput = false;
                isMeleeAttack = false;
                anim.SetTrigger("hit");
            }
            else if(isRangeAttack)
            {
                gotInput = false;
                isRangeAttack = false;
                FireProjectile();
            }
        }

        if (Time.time >= lastInputTime + inputTimer)
        {
            //Wait for new input
            gotInput = false;
        }
    }

    private void CheckAttackHitBox()
    {
        Collider2D[] detectedObjects = Physics2D.OverlapCircleAll(meleeAttackHitBoxPos.position, attack1Radius, whatIsDamageable);

        attackDetails.damageAmount = attack1Damage;
        attackDetails.position = transform.position;
        attackDetails.stunDamageAmount = stunDamageAmount;

        foreach (Collider2D collider in detectedObjects)
        {
            if (collider.name == "Boss")
            {
                collider.SendMessage("TakeDamage", (int)attack1Damage);
            }
            else
            {
                collider.transform.parent.SendMessage("Damage", attackDetails);
            }
        }
    }

    private void FireProjectile()
    {
        if (combatEnabled)
        {
            GameObject projectilePoolGO = projectilePool.GetObject();
            projectilePoolGO.transform.SetPositionAndRotation(rangeAttackHitBoxPos.position, rangeAttackHitBoxPos.rotation);
            Projectile projectile = projectilePoolGO.GetComponent<Projectile>();
            if (projectile != null)
            {
                projectile.Initialize(projectilePool); // Link the pool to the projectile
                projectile.sendDamageToParent = true;
                projectile.isBullet = true;
                projectile.FireProjectile(projectileSpeed, projectileTravelDistance, projectileDamage);
            }
        }
    }

    private void Damage(AttackDetails attackDetails)
    {
        int direction;

        playerStats.DecreaseHealth(attackDetails.damageAmount);

        CharacterEvents.characterDamaged.Invoke(gameObject, (int)attackDetails.damageAmount);


        if (attackDetails.position.x < transform.position.x)
        {
            direction = 1;
        }
        else
        {
            direction = -1;
        }

        playerController.Knockback(direction);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(meleeAttackHitBoxPos.position, attack1Radius);
    }
}
