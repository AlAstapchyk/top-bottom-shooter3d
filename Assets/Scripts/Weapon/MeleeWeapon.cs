using UnityEngine;
using System.Collections.Generic;

public class MeleeWeapon : Weapon
{
    [Header("Melee Settings")]
    [SerializeField] private GameObject attackEffect; // Visual effects for the attack
    [field: SerializeField] public float splashAngle { get; private set; } = 1;
    [SerializeField] private float angleStep = 1f;  // Step between rays in FOV

    public override void Attack()
    {
        if (!CanAttack()) return;
        lastAttackTime = Time.time;

        if (attackEffect != null)
        {
            GameObject effect = Instantiate(attackEffect, transform.position, Quaternion.identity);
            Destroy(effect, 1f); // Destroy effect after 1 second
        }

        HashSet<Health> damagedObjects = new HashSet<Health>(); // Track unique objects hit
        int rayCount = Mathf.CeilToInt(splashAngle / angleStep); // Number of rays based on the angle

        for (int i = 0; i < rayCount; i++)
        {
            // Calculate the direction of each ray based on angle step
            float angle = -splashAngle / 2 + angleStep * i;

            Vector3 direction = transform.forward;  // The forward direction in local space
            direction = Quaternion.Euler(0, angle, 0) * direction;

            // Raycast to detect objects with a Health component within the cone
            if (Physics.Raycast(transform.position, direction, out RaycastHit hit, range))
            {
                if (hit.collider.TryGetComponent(out Health health))
                {
                    // Add to the HashSet and apply damage if it's the first time this object is hit
                    if (damagedObjects.Add(health))
                    {
                        health.TakeDamage(damage);
                    }
                }
            }
        }

        if (isHeldByPlayer) EventManager.Instance.Weapon.OnPlayerAttackEnd.Invoke(this);
    }
}