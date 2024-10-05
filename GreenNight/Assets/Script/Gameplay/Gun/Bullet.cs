using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CaliberType
{
    Low,
    Medium,
    High,
    Shotgun
}

public class Bullet : MonoBehaviour
{
    public float damage;
    public CaliberType caliberType; // Added caliber type
    public int penetrationCount = 0; // Keeps track of how many targets have been penetrated

    private void OnTriggerEnter2D(Collider2D other)
    {   
        Zombie zombie = other.GetComponent<Zombie>();
        if (zombie != null)
        {
            // Apply damage based on the caliber type
            switch (caliberType)
            {
                case CaliberType.Low:
                    zombie.ZombieTakeDamage(damage, DamageType.LowcaliberBullet);
                    break;
                case CaliberType.Medium:
                    zombie.ZombieTakeDamage(damage, DamageType.MediumcaliberBullet);
                    PenetrateTarget(1); // Penetrates 1 additional target
                    break;
                case CaliberType.High:
                    zombie.ZombieTakeDamage(damage, DamageType.HighcalliberBullet);
                    PenetrateTarget(5); // Penetrates 5 additional targets
                    break;
                case CaliberType.Shotgun:
                    zombie.ZombieTakeDamage(damage, DamageType.ShotgunPellet);
                    break;
            }

            if (caliberType == CaliberType.Medium || caliberType == CaliberType.High)
            {
                if (penetrationCount > 0)
                {
                    penetrationCount--;
                }
                else
                {
                    Destroy(this.gameObject);
                }
            }
            else
            {
                Destroy(this.gameObject);
            }
        }

        if (other.CompareTag("Wall"))
        {
            Destroy(this.gameObject);
        }
    }

    private void PenetrateTarget(int additionalPenetrations)
    {
        penetrationCount = additionalPenetrations;
    }
}

