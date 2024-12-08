using System.Collections;
using UnityEngine;

public class HealthHearts : MonoBehaviour
{
    [SerializeField] private int maxHearts = 3;
    [SerializeField] private bool isPlayer;
    [SerializeField] private float invulnerabilitySeconds = 1.5f;
    [SerializeField] private float invulnerabilityDeltaTime = 0.15f;
    [SerializeField] private int currentHearts;
    private bool isInvulnerable;

    private void Awake()
    {
        isInvulnerable = false;
        currentHearts = maxHearts;
    }

    public void TakeDamage(int damage)
    {
        if (isInvulnerable) { return; }

        currentHearts -= damage;
        currentHearts = Mathf.Max(0, currentHearts);

        if (isPlayer) { EventManager.Instance.Health.OnPlayerHeartsChanged.Invoke(currentHearts); }

        if (currentHearts <= 0)
        {
            if (isPlayer) EventManager.Instance.Health.OnPlayerDied.Invoke();
            //Destroy(gameObject);
            gameObject.SetActive(false);
            return;
        }

        StartCoroutine(BecomeInvulnerable(invulnerabilitySeconds, invulnerabilityDeltaTime));

    }

    public void Heal(int amount)
    {
        currentHearts += amount;
        currentHearts = Mathf.Min(currentHearts, maxHearts);

        if (isPlayer) EventManager.Instance.Health.OnPlayerHealthChanged.Invoke(currentHearts);
    }

    private IEnumerator BecomeInvulnerable(float invSeconds, float invDeltaTime)
    {
        //Debug.Log("Player is invulnerable");
        isInvulnerable = true;

        for (float i = 0; i <= invSeconds; i += invDeltaTime)
        {
            // Invulnerability animation or logic
            yield return new WaitForSeconds(invDeltaTime);
        }

        isInvulnerable = false;
        //Debug.Log("Player is no longer... invulnerable");

    }

    public int getMaxHearts() { return maxHearts; }
}