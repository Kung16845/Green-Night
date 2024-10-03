using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    public float currentSpeed = 5f;
    private bool isMovementStopped = false;

    void Update()
    {
        if (!isMovementStopped)
        {
            float vertical = Input.GetAxis("Vertical");
            float horizontal = Input.GetAxis("Horizontal");
            Vector2 direction = new Vector2(horizontal, vertical);
            transform.Translate(direction * currentSpeed * Time.deltaTime);
        }
    }

    public void StopMovementForDuration(float duration)
    {
        StartCoroutine(StopMovementCoroutine(duration));
    }

    private IEnumerator StopMovementCoroutine(float duration)
    {
        isMovementStopped = true;
        yield return new WaitForSeconds(duration);
        isMovementStopped = false;
    }
}
