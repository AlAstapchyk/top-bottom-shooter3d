using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class HealthHearts : MonoBehaviour
{
    [SerializeField] private AudioClip damageClip;
    [SerializeField] private AudioClip deathClip;

    [SerializeField] private int maxHearts = 3;
    [SerializeField] private bool isPlayer;
    [SerializeField] private float invulnerabilitySeconds = 1.5f;
    [SerializeField] private float invulnerabilityDeltaTime = 0.15f;
    [SerializeField] private int currentHearts;
    [SerializeField] private SoundsSource sounds;
    private bool isInvulnerable;
    public UnityEvent<int> OnHeartsChange = new UnityEvent<int>();
    public UnityEvent OnPlayerDeath = new UnityEvent();
    public UnityEvent<AudioClip> OnDamageOrDeathSound = new UnityEvent<AudioClip>();

    private void Awake()
    {
        isInvulnerable = false;
        currentHearts = maxHearts;
        if (sounds != null)
        {
            //Debug.Log("sounds are not null");
            OnDamageOrDeathSound?.AddListener(sounds.DeathSound);
        }
    }

    public void TakeDamage(int damage)
    {
        
        //GameObject particleGO = Instantiate(muzzleParticlePrefab, muzzleTransf.position, muzzleTransf.rotation);
        //var particleSystem = muzzleParticlePrefab.GetComponent<ParticleSystem>().main;
        //Destroy(particleGO, particleSystem.duration + particleSystem.startLifetime.constantMax);



        if (isInvulnerable) { return; }

        currentHearts -= damage;
        currentHearts = Mathf.Max(0, currentHearts);

        if (isPlayer) { EventManager.Instance.Health.OnPlayerHeartsChanged.Invoke(currentHearts); }
        OnHeartsChange?.Invoke(currentHearts);
        

        if (currentHearts <= 0)
        {
            if (isPlayer) {
                //Debug.Log("Player ded");
                EventManager.Instance.Health.OnPlayerDied?.Invoke(); 
                //OnPlayerDeath?.Invoke();
            }
            else { 
                gameObject.SetActive(false);
            }
            //Destroy(gameObject);
            OnDamageOrDeathSound?.Invoke(deathClip);

            return;
        }
        OnDamageOrDeathSound?.Invoke(damageClip);
        StartCoroutine(BecomeInvulnerable(invulnerabilitySeconds, invulnerabilityDeltaTime));

    }

    public void Heal(int amount)
    {
        currentHearts += amount;
        currentHearts = Mathf.Min(currentHearts, maxHearts);

        if (isPlayer) EventManager.Instance.Health.OnPlayerHealthChanged.Invoke(currentHearts);
        OnHeartsChange?.Invoke(currentHearts);
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
    public void SetHeartsToMax()
    {
        currentHearts = maxHearts;
    }

    public void SetCurrentHearts(int hearts)
    {
        currentHearts = hearts;
    }

    public int getMaxHearts() { return maxHearts; }
}