using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    [SerializeField]
    private int maxHealth = 20;

    private int currentHealth;

    [SerializeField]
    protected float moveSpeed = 2f;

    // ---------- AI ----------
    [SerializeField]
    private EnemyState currentState = EnemyState.Patrol;

    //Detection
    [SerializeField]
    private float detectionRange = 5f;

    //Attack
    [SerializeField]
    protected float attackRange = 1.5f;

    //Patrol
    [SerializeField]
    private Transform[] patrolPoints;

    private int currentPatrolIndex = 0;
    private int patrolDirection = 1;

    public bool IsDead { get; private set; }

    //Enemy Art 
    protected SpriteRenderer outlineRenderer;

    private CursorController cursorController;

    protected SpriteRenderer lockRenderer;

    //Player Detection
    private Transform player;

    private bool playerDetected = false;

    private Rigidbody2D rb;

    //Alert
    [SerializeField]
    private float alertDuration = 1f;

    private SpriteRenderer alertRenderer;

    private float alertTimer = 0f;


    protected virtual void Awake()
    {
        //Hover Outline
        outlineRenderer = transform.Find("Outline")
            .GetComponent<SpriteRenderer>();

        outlineRenderer.enabled = false;

        cursorController = FindFirstObjectByType<CursorController>();

        cursorController.SetNormalCursor();

        //LockIn 
        lockRenderer = transform.Find("LockIcon")
            .GetComponent<SpriteRenderer>();

        lockRenderer.enabled = false;

        //Alert
        alertRenderer = transform.Find("AlertIcon")
            .GetComponent<SpriteRenderer>();

        alertRenderer.enabled = false;

        //HP
        currentHealth = maxHealth;

        //Detection
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            player = playerObject.transform;
        }

        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        DetectPlayer();
        UpdateAlert();

        UpdateState();
    }

    private void UpdateState()
    {
        switch (currentState)
        {
            case EnemyState.Patrol:
                Patrol();
                break;

            case EnemyState.Chase:
                ChasePlayer();
                break;

            case EnemyState.Attack:
                Attack();
                break;

            case EnemyState.Dead:
                break;
        }
    }

    //Hover On & Off
    public virtual void Hover()
    {
        outlineRenderer.enabled = true;
        cursorController.SetSwordCursor();

        //lockRenderer.enabled = true;
    }

    public virtual void ExitHover()
    {
        outlineRenderer.enabled = false;
        cursorController.SetNormalCursor();
    }

    //LockIn On & Off
    public virtual void Lock()
    {
        lockRenderer.enabled = true;
    }

    public virtual void Unlock()
    {
        lockRenderer.enabled = false;
    }

    //HP
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        Debug.Log($"{name} HP : {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        IsDead = true;

        currentState = EnemyState.Dead;

        Debug.Log($"{name} Died!");

        Destroy(gameObject);
    }

    //Player detection
    private void DetectPlayer()
    {
        if (player == null)
            return;

        float distance = Vector2.Distance(
            transform.position,
            player.position
        );

        if (distance <= detectionRange)
        {
            if (!playerDetected)
            {
                playerDetected = true;

                ShowAlert();

                currentState = EnemyState.Chase;

                //Debug.Log($"{name} : Player Detected!");
            }
        }
        else
        {
            if (playerDetected)
            {
                playerDetected = false;

                rb.linearVelocity = Vector2.zero;

                currentState = EnemyState.Patrol;

                //Debug.Log($"{name} : Player Lost!");
            }
        }
    }

    //Show Alert
    private void ShowAlert()
    {
        alertRenderer.enabled = true;
        alertTimer = alertDuration;
    }

    private void UpdateAlert()
    {
        if (!alertRenderer.enabled)
            return;

        alertTimer -= Time.deltaTime;

        if (alertTimer <= 0f)
        {
            alertRenderer.enabled = false;
        }
    }

    private void ChasePlayer()
    {
        if (!playerDetected || player == null)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        float distance = Vector2.Distance(
            transform.position,
            player.position
        );

        //Debug.Log(
        //    $"{name} Distance: {distance:F2} | Attack Range: {attackRange:F2}"
        //);

        if (distance <= attackRange)
        {
            //Debug.Log($"{name} ENTERED ATTACK RANGE - STOP");

            rb.linearVelocity = Vector2.zero;
            return;
        }

        Vector2 direction = (
            player.position - transform.position
        ).normalized;

        rb.linearVelocity = direction * moveSpeed;
    }

    private void Patrol()
    {
        if (patrolPoints == null || patrolPoints.Length == 0)
            return;

        Transform targetPoint = patrolPoints[currentPatrolIndex];

        Vector2 direction = (
            targetPoint.position - transform.position
        ).normalized;

        rb.linearVelocity = direction * moveSpeed;

        float distance = Vector2.Distance(
            transform.position,
            targetPoint.position
        );

        if (distance < 0.1f)
        {
            currentPatrolIndex += patrolDirection;

            if (currentPatrolIndex >= patrolPoints.Length)
            {
                currentPatrolIndex = patrolPoints.Length - 2;
                patrolDirection = -1;
            }
            else if (currentPatrolIndex < 0)
            {
                currentPatrolIndex = 1;
                patrolDirection = 1;
            }
        }
    }

    private void Attack()
    {
        // Attack behavior will be implemented later.
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;

        Gizmos.DrawWireSphere(
            transform.position,
            detectionRange
        );

        // Attack Range
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(
            transform.position,
            attackRange
        );
    }
}
