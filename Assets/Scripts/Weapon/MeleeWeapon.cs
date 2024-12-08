using UnityEngine;
public class MeleeWeapon : Weapon
{
    [Header("Melee Settings")]
    [SerializeField] private GameObject attackEffect; // Visual effects for the attack
    [field: SerializeField] public float splashAngle { get; private set; } = 1;
    [SerializeField] private float angleStep = 1f;  // Step between rays in FOV
    [SerializeField] private Transform attackOrigin;

    public override void Attack()
    {
        if (!CanAttack()) return;
        lastAttackTime = Time.time;

        if (attackEffect != null)
        {
            GameObject effect = Instantiate(attackEffect, transform.position, Quaternion.identity);
            Destroy(effect, 1f); // Destroy effect after 1 second
        }

        bool hitEnemy = false;

        int rayCount = Mathf.CeilToInt(splashAngle / angleStep); // Number of rays based on the angle
        for (int i = 0; i < rayCount; i++)
        {
            // Calculate the direction of each ray based on angle step
            float angle = -splashAngle / 2 + angleStep * i;

            Vector3 direction = attackOrigin.forward;  // The forward direction in local space
            direction = Quaternion.Euler(0, angle, 0) * direction;

            Vector3 rayEndpoint = direction * range;

            // Raycast to detect enemies within the cone
            if (Physics.Raycast(attackOrigin.position, direction, out RaycastHit hit, range))
            {
                if (hit.collider.CompareTag("Enemy"))
                {
                    hitEnemy = true;
                    // Apply damage to the enemy
                    if (hit.collider.TryGetComponent(out Health health))
                    {
                        health.TakeDamage(damage);
                        Debug.Log("Enemy hit by splash damage!");
                    }
                }
            }

            // Visualize the rays in the editor for debugging (optional)
            Debug.DrawLine(attackOrigin.position, attackOrigin.position + direction * range, hitEnemy ? Color.red : Color.green);
        }

        if (isHeldByPlayer) EventManager.Instance.Weapon.OnPlayerAttackEnd.Invoke(this);
    }

    // Convert angle to direction vector in world space
    private Vector3 AngleToDirection(float angle)
    {
        float radian = angle * Mathf.Deg2Rad;
        return new Vector3(Mathf.Sin(radian), 0, Mathf.Cos(radian)); // Return as 2D direction on the X-Z plane
    }
}