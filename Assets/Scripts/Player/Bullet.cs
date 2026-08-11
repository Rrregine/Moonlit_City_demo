using UnityEngine;

public class Bullet : MonoBehaviour
{
    private EnemyBase target;

    private Vector2 direction;

    [SerializeField]
    private float maxDistance = 5f;

    private Vector3 startPosition;

    [SerializeField]
    private float speed = 0.5f;

    private int damage;

    private bool hasTarget = false;

    // =========================
    // Target Bullet
    // =========================
    public void Initialize(EnemyBase enemy, int bulletDamage)
    {
        target = enemy;
        damage = bulletDamage;

        hasTarget = true;

        startPosition = transform.position;

        Debug.Log($"Bullet Damage : {damage}");
        Debug.Log($"Bullet Target : {enemy.name}");
    }

    // =========================
    // Free Bullet
    // =========================
    public void Initialize(Vector2 bulletDirection, int bulletDamage)
    {
        target = null;
        direction = bulletDirection.normalized;
        damage = bulletDamage;

        hasTarget = false;

        startPosition = transform.position;

        Debug.Log($"Free Bullet Damage : {damage}");
    }

    private void Update()
    {
        //Distance check
        if (Vector2.Distance(transform.position, startPosition) >= maxDistance)
        {
            Destroy(gameObject);
            return;
        }

        // Targeted bullet
        if (hasTarget)
        {
            if (target == null)
                return;

            transform.position = Vector2.MoveTowards(
                transform.position,
                target.transform.position,
                speed * Time.deltaTime
            );

            return;
        }

        // Free bullet
        transform.position +=
            (Vector3)direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        EnemyBase enemy = other.GetComponent<EnemyBase>();

        if (enemy == null)
            return;

        Debug.Log("Hit : " + enemy.name);

        enemy.TakeDamage(damage);

        Destroy(gameObject);
    }
}