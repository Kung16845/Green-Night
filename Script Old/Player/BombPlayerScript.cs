using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;
public class BombPlayerScript : MonoBehaviour
{    public GameObject bombPrefab;
    public List<GameObject> allbombSpawned;
    public Transform firePoint;    
    public float throwForce; 
    public float maxChargeTime = 2f; 
    public float minimumhargeTime = 0.75f; 
    private bool isCharging = false; 
    public float currentChargeTime = 0f; 
    private Vector2 mousePosition; 
    public Camera cameraOwner;
    private bool hasPressedG = false;

    private void Update()
    {
       

        if (Input.GetKeyDown(KeyCode.G) && !hasPressedG) 
        {   
            Debug.Log("G One Click");
            hasPressedG = true;
        }

        if (Input.GetKeyUp(KeyCode.G) && hasPressedG)
        {
            Debug.Log("G Released");
            if (currentChargeTime < minimumhargeTime)
            {
                // If charge time is lower than max, use ThrowBombNotAimServerRpc
                ThrowBombNotAim();
            }
            else
            {
                // Otherwise, use ThrowBombAimingServerRpc
                ThrowBombAiming(throwForce * currentChargeTime, mousePosition, firePoint.position);
            }

            hasPressedG = false;
            currentChargeTime = 0;
        }

        mousePosition = cameraOwner.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetKey(KeyCode.G))
        {
            currentChargeTime += Time.deltaTime;
            isCharging = true;
            // Clamp the charge time to not exceed maxChargeTime
            currentChargeTime = Mathf.Min(currentChargeTime, maxChargeTime);
        }
        else
        {
            isCharging = false;
        }
    }
    
    
    void ThrowBombNotAim()
    {
        var bombspawned = SpawnedBomb();
        Rigidbody2D rb2dBomb = bombspawned.GetComponent<Rigidbody2D>();

        if (GetComponent<SpriteRenderer>().flipX)
            rb2dBomb.AddForce((-firePoint.right + Vector3.up) * throwForce, ForceMode2D.Impulse);
        else
            rb2dBomb.AddForce((firePoint.right + Vector3.up) * throwForce, ForceMode2D.Impulse);
    }

    
    void ThrowBombAiming(float thorwcharge, Vector2 direction, Vector2 startPoint)
    {
        var bombspawned = SpawnedBomb();
        Rigidbody2D rb2dBomb = bombspawned.GetComponent<Rigidbody2D>();
        Vector2 throwDirection = (direction - startPoint).normalized;

        rb2dBomb.AddForce(throwDirection * thorwcharge * 2, ForceMode2D.Impulse);
    }
    
    public GameObject SpawnedBomb()
    {
        GameObject bomb = Instantiate(bombPrefab, firePoint.position, Quaternion.identity);
        allbombSpawned.Add(bomb);
        // bomb.GetComponent<BombScript>().bombPlayerScript = this;
        

        return bomb;
    }
}
