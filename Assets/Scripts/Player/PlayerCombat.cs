using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
    // ---------- Inspector ----------
    [SerializeField]
    private LayerMask enemyLayer;

    [SerializeField]
    private GameObject bulletPrefab;

    // ------ Attack -------
    [SerializeField]
    private int attackDamage = 1;

    [SerializeField]
    private float attackSpeed = 2f;

    private float attackCooldown = 0f;

    private bool autoAttack = false;

    private bool noTargetWarningShown = false;

    private bool autoAttackInterrupted = false;


    // ---------- Target ----------
    private EnemyBase hoveredTarget;
    private EnemyBase currentAttackTarget;

    // -------- Attack Range -------
    [SerializeField]
    private GameObject interactionRangeIndicator;

    [SerializeField]
    private float interactionRadius = 5f;

    [SerializeField]
    private float targetRangeDisplayTime = 0.5f;

    private float targetRangeDisplayTimer = 0f;

    // ---------- Input ----------
    private PlayerControls controls;

    private PlayerController playerController;

    // --------- Cursor ----------
    private CursorController cursorController;

    // ---------- Debug ----------
    private int attackCount = 0;

    private void Awake()
    {
        controls = new PlayerControls();

        cursorController = FindFirstObjectByType<CursorController>();

        playerController = GetComponent<PlayerController>();
    }

    private void Start()
    {
        interactionRangeIndicator.transform.localScale = new Vector3(
            interactionRadius * 2f,
            interactionRadius * 2f,
            1f
        );

        HideInteractionRange();
    }

    private void OnEnable()
    {
        controls.Gameplay.Enable();
    }

    private void OnDisable()
    {
        controls.Gameplay.Disable();
    }

    private void Update()
    {
        attackCooldown -= Time.deltaTime;

        UpdateHover();
        UpdateAttackInput();

        UpdateAutoAttack();
        UpdateTargetRange();

        UpdateInteractionRangeDisplay();

        AutoFire();
    }

    /// Detect which enemy the mouse is hovering over.
    private void UpdateHover()
    {
        Vector3 mouseWorldPosition = GetMouseWorldPosition();

        Collider2D hit = Physics2D.OverlapPoint(mouseWorldPosition, enemyLayer);

        if (hit != null)
        {
            EnemyBase enemy = hit.GetComponent<EnemyBase>();

            if (enemy != null)
            {
                if (hoveredTarget != enemy)
                {
                    if (hoveredTarget != null)
                    {
                        hoveredTarget.ExitHover();
                    }

                    hoveredTarget = enemy;
                    hoveredTarget.Hover();

                    ShowInteractionRange();

                }

                if (IsInInteractionRange(enemy))
                {
                    cursorController.SetSwordCursor();
                }
                else
                {
                    cursorController.SetEyeCursor();
                }

                return;
            }
        }

        if (hoveredTarget != null)
        {
            hoveredTarget.ExitHover();
            hoveredTarget = null;

            HideInteractionRange();
        }
    }

    /// Handle left mouse attack input.
    private void UpdateAttackInput()
    {
        if (!controls.Gameplay.Attack.WasPressedThisFrame())
            return;

        if (hoveredTarget != null &&
            IsInInteractionRange(hoveredTarget))
        {
            SetTarget(hoveredTarget);
        }
        else
        {
            EnemyBase nearestEnemy = FindNearestEnemy();

            if (nearestEnemy != null)
            {
                SetTarget(nearestEnemy);
            }
        }

        RequestAttack();
    }

    /// Change attack target.
    private void SetTarget(EnemyBase target)
    {
        if (currentAttackTarget == target)
        {
            Debug.Log($"Already Locked : {target.name}");
            return;
        }

        if (currentAttackTarget != null)
        {
            currentAttackTarget.Unlock();
        }

        currentAttackTarget = target;

        currentAttackTarget.Lock();

        ShowInteractionRange();
        targetRangeDisplayTimer = targetRangeDisplayTime;

        Debug.Log($"Lock Target : {target.name}");
    }

    private void RequestAttack()
    {
        if (attackCooldown > 0f)
            return;

        attackCount++;

        if (currentAttackTarget != null)
        {
            FireBullet();
        }
        else
        {
            FireBullet(playerController.FacingDirection);
        }

        attackCooldown = 1f / attackSpeed;
    }

    private void FireBullet()
    {
        //Debug.Log("Create Bullet");

        GameObject bulletObject = Instantiate(
            bulletPrefab,
            transform.position,
            Quaternion.identity
        );

        Bullet bullet = bulletObject.GetComponent<Bullet>();

        bullet.Initialize(
            currentAttackTarget,
            attackDamage
        );

        //Debug.Log("Bullet Created");
    }

    private void FireBullet(Vector2 direction)
    {
        GameObject bulletObject = Instantiate(
            bulletPrefab,
            transform.position,
            Quaternion.identity
        );

        Bullet bullet = bulletObject.GetComponent<Bullet>();

        bullet.Initialize(
            direction,
            attackDamage
        );
    }

    // Convert mouse position from screen space to world space.
    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePosition = Mouse.current.position.ReadValue();

        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

        worldPosition.z = 0;

        return worldPosition;
    }

    // ON & OFF Attack Range Indicator
    private void ShowInteractionRange()
    {
        interactionRangeIndicator.SetActive(true);
    }

    private void HideInteractionRange()
    {
        interactionRangeIndicator.SetActive(false);
    }

    private bool IsInInteractionRange(EnemyBase enemy)
    {
        float distance = Vector2.Distance(
            transform.position,
            enemy.transform.position
        );

        //Debug.Log($"{enemy.name} Distance = {distance}");

        return distance <= interactionRadius;
    }

    // Auto Attack 
    private void UpdateAutoAttack()
    {
        if (!controls.Gameplay.AutoAttack.WasPressedThisFrame())
            return;

        autoAttack = !autoAttack;

        autoAttackInterrupted = false;
        noTargetWarningShown = false;

        Debug.Log($"Auto Attack : {autoAttack}");
    }

    private void AutoFire()
    {
        if (!autoAttack)
            return;

        if (currentAttackTarget == null || currentAttackTarget.IsDead)
        {
            EnemyBase nearestEnemy = FindNearestEnemy();

            if (nearestEnemy != null)
            {
                SetTarget(nearestEnemy);
                noTargetWarningShown = false;
            }
            else
            {
                if (!noTargetWarningShown)
                {
                    Debug.Log("NO TARGET");
                    noTargetWarningShown = true;
                }

                return;
            }
        }

        RequestAttack();
    }

    // Auto attack finds the nearest enemy
    private EnemyBase FindNearestEnemy()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(
            transform.position,
            interactionRadius,
            enemyLayer
        );

        EnemyBase nearestEnemy = null;
        float nearestDistance = Mathf.Infinity;

        foreach (Collider2D collider in enemies)
        {
            EnemyBase enemy = collider.GetComponent<EnemyBase>();

            if (enemy == null)
                continue;

            float distance = Vector2.Distance(
                transform.position,
                enemy.transform.position
            );

            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestEnemy = enemy;
            }
        }

        return nearestEnemy;
    }

    // Unlock enemy out of range
    private void UpdateTargetRange()
    {
        if (currentAttackTarget == null)
            return;

        if (!IsInInteractionRange(currentAttackTarget))
        {
            Debug.Log($"Target Out Of Range : {currentAttackTarget.name}");

            currentAttackTarget.Unlock();
            currentAttackTarget = null;

            if (autoAttack)
            {
                autoAttack = false;
                autoAttackInterrupted = true;

                Debug.Log("Auto Attack Interrupted");
            }
        }
    }

    //Dispay of the interaction range
    private void UpdateInteractionRangeDisplay()
    {
        if (targetRangeDisplayTimer <= 0f)
            return;

        targetRangeDisplayTimer -= Time.deltaTime;

        if (targetRangeDisplayTimer <= 0f)
        {
            HideInteractionRange();
        }
    }
}