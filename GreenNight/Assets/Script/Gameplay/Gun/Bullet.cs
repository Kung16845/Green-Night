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
    public CaliberType caliberType;
    private int penetrationCount = 0; // Tracks how many penetrations the bullet can do

    // This method will initialize penetration count based on the caliber
    public void InitializePenetration()
    {
        switch (caliberType)
        {
            case CaliberType.Low:
                penetrationCount = 0;  // Low caliber can't penetrate
                break;
            case CaliberType.Medium:
                penetrationCount = 1;  // Medium caliber can penetrate 1 target
                break;
            case CaliberType.High:
                penetrationCount = 5;  // High caliber can penetrate 2 targets
                break;
            case CaliberType.Shotgun:
                penetrationCount = 0;  // Shotgun pellets can't penetrate
                break;
        }
    }

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
                    Destroy(this.gameObject); // Low caliber bullets are destroyed on first impact
                    break;

                case CaliberType.Medium:
                    zombie.ZombieTakeDamage(damage, DamageType.MediumcaliberBullet);
                    HandlePenetration();
                    break;

                case CaliberType.High:
                    zombie.ZombieTakeDamage(damage, DamageType.HighcalliberBullet);
                    HandlePenetration();
                    break;

                case CaliberType.Shotgun:
                    zombie.ZombieTakeDamage(damage, DamageType.ShotgunPellet);
                    Destroy(this.gameObject); // Shotgun pellets are destroyed on first impact
                    break;
            }
        }

        if (other.CompareTag("Wall"))
        {
            Destroy(this.gameObject); // Destroy the bullet if it hits a wall
        }
    }

    private void HandlePenetration()
    {
        if (penetrationCount > 0)
        {
            penetrationCount--; // Decrease penetration count on hit
            switch (caliberType)
            {
                case CaliberType.Medium:
                damage *= 0.5f;
                break; 
            }
        }
        else
        {
            Destroy(this.gameObject); // Destroy bullet if it has no penetrations left
        }
    }
}


