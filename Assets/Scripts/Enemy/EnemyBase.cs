using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    // ---------- Stats ----------
    [SerializeField]
    private int maxHealth = 20;

    private int currentHealth;

    public bool IsDead { get; private set; }

    //Enemy Art 
    protected SpriteRenderer outlineRenderer;

    private CursorController cursorController;

    protected SpriteRenderer lockRenderer;


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

        //HP
        currentHealth = maxHealth;
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
}
