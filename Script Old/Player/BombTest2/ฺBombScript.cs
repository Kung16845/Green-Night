using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BombScript : MonoBehaviour
{
   public float damage;
   public Collider2D bombCollider;

   private void Awake()
   {
      // Get the Collider2D component attached to this GameObject
      bombCollider = GetComponent<Collider2D>();
      StartCoroutine(DestroyAfterDuration());
   }

   private void OnTriggerEnter2D(Collider2D other)
   {
      var enemy = other.GetComponent<Enemy>();
      if (enemy != null)
      {
         enemy.TakeDamageEnemy(damage);
      }
   }
   private IEnumerator DestroyAfterDuration()
   {
      yield return new WaitForSeconds(0.2f);
      if (bombCollider != null)
      {
         bombCollider.enabled = false;  // Disable the collider on the bomb

      }
      yield return new WaitForSeconds(1.3f);
      Destroy(gameObject);
   }
}
