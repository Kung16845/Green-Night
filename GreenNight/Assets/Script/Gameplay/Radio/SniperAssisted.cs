using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SniperAssisted : MonoBehaviour
{
    [Header("Sniper Assistant Configuration")]
    public float assistantDuration = 10f; // Duration of assistance in seconds
    public float damage = 50f; // Damage dealt per shot
    public DamageType damageType = DamageType.HighcalliberBullet; // Type of damage
    public float shotCooldown = 1f; // Cooldown between shots in seconds
    public float abilityCooldown = 20f; // Cooldown till ability can be used again

    [Header("UI Elements")]
    public Button sniperButton; // Button to activate sniper
    public Slider cooldownSlider; // Slider to track cooldown

    private bool isAvailable = true;
    private bool isActive = false;
    private float cooldownTimer = 0f;

    private void Start()
    {
        if (sniperButton != null)
        {
            sniperButton.onClick.AddListener(ActivateSniperAssistance);
        }
        if (cooldownSlider != null)
        {
            cooldownSlider.maxValue = abilityCooldown;
            cooldownSlider.value = 0;
        }
    }

    private void Update()
    {
        if (!isAvailable)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownSlider != null)
            {
                cooldownSlider.value = abilityCooldown - cooldownTimer;
            }

            if (cooldownTimer <= 0f)
            {
                isAvailable = true;
                if (sniperButton != null)
                {
                    sniperButton.interactable = true;
                }
            }
        }
    }

    private void ActivateSniperAssistance()
    {
        if (!isAvailable) return;

        isAvailable = false;
        isActive = true;
        cooldownTimer = abilityCooldown;
        if (sniperButton != null)
        {
            sniperButton.interactable = false;
        }

        StartCoroutine(SniperAssistanceRoutine());
    }

    private IEnumerator SniperAssistanceRoutine()
    {
        float elapsed = 0f;

        while (elapsed < assistantDuration)
        {
            DealRandomDamage();
            yield return new WaitForSeconds(shotCooldown);
            elapsed += shotCooldown;
        }

        isActive = false;
    }

    private void DealRandomDamage()
    {
        // Find all zombies in the scene
        Zombie[] zombies = FindObjectsOfType<Zombie>();
        if (zombies.Length == 0) return;

        // Pick a random zombie
        Zombie target = zombies[Random.Range(0, zombies.Length)];
        if (target != null)
        {
            target.ZombieTakeDamage(damage, damageType);
            SoundManager.Instance.PlaySound("AWP");
        }
    }
}
