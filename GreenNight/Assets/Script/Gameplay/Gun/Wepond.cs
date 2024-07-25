using System.Collections;
using UnityEngine;

public class Weapond : MonoBehaviour
{
    // Weapon properties
    public int rateOfFire; // Rounds per minute
    public int handling; // Speed penalty and reload speed (1-100)
    public float accuracy; // Bullet precision (1-100)
    public int capacity; // Ammo in magazine (1-100)
    public float stability; // Recoil (1-100)
    public float damage; // Damage per bullet
    public bool fullAuto; // Full auto or not

    // Internal variables
    private int currentAmmo;
    private float fireRate;
    private float nextFireTime;
    private bool isReloading = false;

    // References
    public Transform firePoint; // Point from where bullets are fired
    public GameObject bulletPrefab; // Bullet prefab

    void Start()
    {
        currentAmmo = capacity;
        fireRate = 60f / rateOfFire; // Calculate fire rate in seconds per shot
    }

    void Update()
    {
        if (isReloading) return;

        if (fullAuto)
        {
            if (Input.GetMouseButton(0) && Time.time >= nextFireTime)
            {
                Shoot();
                nextFireTime = Time.time + fireRate;
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0) && Time.time >= nextFireTime)
            {
                Shoot();
                nextFireTime = Time.time + fireRate;
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(Reload());
        }
    }

    void Shoot()
    {
        if (currentAmmo <= 0)
        {
            Debug.Log("Out of ammo!");
            return;
        }

        currentAmmo--;

        // Instantiate bullet and set its direction
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        bulletScript.damage = damage;
        // Adjust bullet direction based on accuracy (higher accuracy means less spread)
        Vector2 bulletDirection = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - firePoint.position).normalized;
        float accuracySpread = (100 - accuracy) / 1000f; // Spread based on accuracy value
        bulletDirection += new Vector2(Random.Range(-accuracySpread, accuracySpread), Random.Range(-accuracySpread, accuracySpread));
        rb.velocity = bulletDirection * 20f; // Adjust bullet speed as necessary

        // Handle recoil (stability)
        float recoilAmount = (100f - stability) / 100f; // Recoil based on stability value
        transform.position += (Vector3)(-bulletDirection * recoilAmount);

        Debug.Log("Shot fired!");
    }

    IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log("Reloading...");
        yield return new WaitForSeconds(3f * (100f - handling) / 100f); // Adjust reload speed based on handling
        currentAmmo = capacity;
        isReloading = false;
        Debug.Log("Reloaded!");
    }
}
