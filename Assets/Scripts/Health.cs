using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private bool isPlayer;
    [SerializeField] private ParticleSystem damageParticles;
    [SerializeField] private Transform particleSpawnPoint;

    private int currentHealth;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Max(0, currentHealth);

        if (damageParticles != null && particleSpawnPoint != null)
        {
            Instantiate(damageParticles, particleSpawnPoint.position, particleSpawnPoint.rotation);
        }

        if (isPlayer) EventManager.Instance.Health.OnPlayerHealthChanged.Invoke(currentHealth);

        if (currentHealth == 0)
        {
            if (isPlayer) EventManager.Instance.Health.OnPlayerDied.Invoke();
            Destroy(gameObject);
        }
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Min(currentHealth, maxHealth);

        if (isPlayer) EventManager.Instance.Health.OnPlayerHealthChanged.Invoke(currentHealth);
    }
}