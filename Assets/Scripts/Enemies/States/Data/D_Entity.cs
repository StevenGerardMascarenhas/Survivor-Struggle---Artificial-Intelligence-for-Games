using UnityEngine;

[CreateAssetMenu(fileName = "newEntityData", menuName = "Data/Entity Data/Base Data")]
public class D_Entity : ScriptableObject
{
    public float wallCheckDistance = 0.2f;
    public float ledgeCheckDistance = 0.4f;

    public LayerMask whatIsGround;
    public LayerMask whatIsPlayer;

    public float minAgroDistance = 3f;
    public float maxAgroDistance = 4f;

    public float closeRangeActionDistance = 1f;

    public float maxHealth = 30;
    public float damageHopSpeed = 3f;

    public float groundCheckRadius = 0.3f;

    public float stunResistance = 3f;
    public float stunRecoveryTime = 1.5f;
}
