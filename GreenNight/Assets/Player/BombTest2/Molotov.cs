using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Molotov : MonoBehaviour
{
    public float damage;
    public float damageInterval = 0.5f; // Damage interval in seconds
    public float duration = 5f; // Duration in seconds before the Molotov disappears
    private Coroutine damageCoroutine;

    private void Start()
    {
        // Start the countdown to destroy the Molotov after the specified duration
        StartCoroutine(DestroyAfterDuration());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var enemy = other.GetComponent<Enemy>();
        if (enemy != null)
        {
            // If the bomb hits an enemy, start applying damage over time
            if (damageCoroutine == null)
            {
                damageCoroutine = StartCoroutine(ApplyDamageOverTime(enemy));
            }
        }
    }

    private IEnumerator ApplyDamageOverTime(Enemy enemy)
    {
        while (true)
        {
            enemy.TakeDamageEnemyServerRpc(damage);
            yield return new WaitForSeconds(damageInterval);
        }
    }

    private IEnumerator DestroyAfterDuration()
    {
        yield return new WaitForSeconds(duration);
        DestroyMolotov();
    }

    private void DestroyMolotov()
    {
        // Destroy the Molotov object
        Destroy(gameObject);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        var enemy = other.GetComponent<Enemy>();
        if (enemy != null)
        {
            // If the enemy leaves the area, stop applying damage
            if (damageCoroutine != null)
            {
                StopCoroutine(damageCoroutine);
                damageCoroutine = null;
            }
        }
    }
}
