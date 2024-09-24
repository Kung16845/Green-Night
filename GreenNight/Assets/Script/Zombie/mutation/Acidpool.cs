using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcidPool : MonoBehaviour
{
    private float barrierDamagePerTick;
    private float zombieDamagePerTick;
    private float duration;
    private float radius;
    private float damageInterval;

    private Dictionary<Zombie, Coroutine> zombieDamageCoroutines = new Dictionary<Zombie, Coroutine>();

    public void Initialize(float barrierDamage, float zombieDamage, float duration, float radius, float interval)
    {
        this.barrierDamagePerTick = barrierDamage;
        this.zombieDamagePerTick = zombieDamage;
        this.duration = duration;
        this.radius = radius;
        this.damageInterval = interval;
        this.transform.localScale = new Vector3(radius, radius, 1f);

        StartCoroutine(ApplyDamageOverTime());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Zombie zombie = other.GetComponent<Zombie>();
        if (zombie != null && !zombieDamageCoroutines.ContainsKey(zombie))
        {
            Coroutine coroutine = StartCoroutine(ApplyDamageOverTimeToZombie(zombie));
            zombieDamageCoroutines.Add(zombie, coroutine);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Zombie zombie = other.GetComponent<Zombie>();
        if (zombie != null && zombieDamageCoroutines.ContainsKey(zombie))
        {
            StopCoroutine(zombieDamageCoroutines[zombie]);
            zombieDamageCoroutines.Remove(zombie);
        }
    }

    private IEnumerator ApplyDamageOverTimeToZombie(Zombie zombie)
    {
        while (zombie != null && zombie.gameObject != null)
        {
            zombie.ZombieTakeDamage(zombieDamagePerTick, DamageType.Acid);
            yield return new WaitForSeconds(damageInterval);
        }
        zombieDamageCoroutines.Remove(zombie);
    }

    private IEnumerator ApplyDamageOverTime()
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }

        Destroy(this.gameObject);
    }

    private void OnDestroy()
    {
        foreach (var coroutine in zombieDamageCoroutines.Values)
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
            }
        }
        zombieDamageCoroutines.Clear();
    }
}
