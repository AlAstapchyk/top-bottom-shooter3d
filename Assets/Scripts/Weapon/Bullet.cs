using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private bool isRicochet;
    [SerializeField] private GameObject impactParticlePrefab; // Reference to the impact particle prefab
    private Rigidbody bulletRigidbody;
    private float speed;
    private int damage;
    private float range;
    private float distanceTravelled;

    private void Awake()
    {
        bulletRigidbody = GetComponent<Rigidbody>();
    }

    // Set values from the Weapon when instantiated
    public void SetValues(float speed, int damage, float range)
    {
        this.speed = speed;
        this.damage = damage;
        this.range = range;

        // Apply the speed to the bullet's initial velocity
        bulletRigidbody.linearVelocity = transform.forward * speed;
    }

    private void Update()
    {
        distanceTravelled += bulletRigidbody.linearVelocity.magnitude * Time.deltaTime;

        if (distanceTravelled >= range)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject go = collision.gameObject;
        if (impactParticlePrefab != null && go.tag == "Obstacle")
        {
            Instantiate(impactParticlePrefab, collision.contacts[0].point, Quaternion.LookRotation(collision.contacts[0].normal));
        }

        if (go.TryGetComponent(out EnemyMovement enemyMovement))
        {
            enemyMovement.TriggerBulletHitReaction();
        }

        if (go.TryGetComponent(out Health health))
        {
            health.TakeDamage(damage);
            Destroy(gameObject);
        }

        if (!isRicochet) Destroy(gameObject);
    }
}