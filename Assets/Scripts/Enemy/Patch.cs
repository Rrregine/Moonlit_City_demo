using UnityEngine;

public class Patch : EnemyBase
{
    [Header("Patch Attack")]

    [SerializeField]
    private int attackDamage = 1;

    [SerializeField]
    private float attackCooldown = 1f;

    private float attackTimer = 0f;

    private PlayerHealth playerHealth;

    protected override void Awake()
    {
        base.Awake();

        GameObject playerObject =
            GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            playerHealth =
                playerObject.GetComponent<PlayerHealth>();
        }
    }

    protected override void Attack()
    {
        base.Attack();

        if (playerHealth == null)
            return;

        if (currentState != EnemyState.Attack)
            return;

        attackTimer -= Time.deltaTime;

        if (attackTimer > 0f)
            return;

        playerHealth.TakeDamage(attackDamage);

        attackTimer = attackCooldown;

        Debug.Log($"{name} : Attack Player!");
    }

    protected override void OnEnterAttack()
    {
        attackTimer = 0f;
    }
}