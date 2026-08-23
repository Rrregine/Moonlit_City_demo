using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    //Stats
    [SerializeField]
    private int maxHealth = 20;

    private int currentHealth;

    //Detection
    [SerializeField]
    private float detectionRange = 5f;

    //Attack
    [SerializeField]
    protected float attackRange = 1.5f;

    [SerializeField]
    protected float moveSpeed = 2f;

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
        ChasePlayer();
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

                Debug.Log($"{name} : Player Detected!");
            }
        }
        else
        {
            if (playerDetected)
            {
                playerDetected = false;

                rb.linearVelocity = Vector2.zero;

                Debug.Log($"{name} : Player Lost!");
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
        if (!playerDetected)
            return;

        if (player == null)
            return;

        float distance = Vector2.Distance(
            transform.position,
            player.position
        );

        if (distance <= attackRange)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        Vector2 direction = (
            player.position - transform.position
        ).normalized;

        rb.linearVelocity = direction * moveSpeed;
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
