using UnityEngine;

public class SlimeProjectile : MonoBehaviour
{
    private Vector2 targetPosition;
    private float speed;

    // Duration before the projectile is destroyed if it doesn't hit the player
    private float lifeTime = 5f;

    private void Start()
    {
        // Destroy the projectile after its lifetime expires
        Destroy(gameObject, lifeTime);
    }

    public void Initialize(Vector2 targetPos, float projectileSpeed)
    {
        targetPosition = targetPos;
        speed = projectileSpeed;

        // Rotate the projectile to face the target
        Vector2 direction = targetPosition - (Vector2)transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void Update()
    {
        // Move the projectile towards the target position
        Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerMovement player = other.GetComponent<PlayerMovement>();
        if (player != null)
        {
            // Stop the player's movement for one second
            player.StopMovementForDuration(1f);

            // Destroy the projectile
            Destroy(gameObject);
        }
    }
}
