using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastBagAmmo : MonoBehaviour
{
    public Transform grabPoint;
    public Transform rayPoint;
    public float rayRadius = 2.5f;
    public float holdTime = 2f;
    public float timer = 0f;
    private LayerMask objectLayer;
    // public BoxAmmoSpwaner boxSpwaner;
    private void Start()
    {
        objectLayer = LayerMask.GetMask("Objects");
        boxSpwaner = FindObjectOfType<BoxAmmoSpwaner>();
    }
    private void Update()
    {
       
        RaycastHit2D hitInfo = Physics2D.CircleCast(rayPoint.position, rayRadius, transform.right, rayRadius, objectLayer);
        if (hitInfo.collider.GetComponent<BagAmmo>() != null)
        {
            if (Input.GetKey(KeyCode.E))
            {
                timer += Time.deltaTime;
                if (timer >= holdTime)
                {
                    hitInfo.collider.GetComponent<BagAmmo>().isOpening.Value = true;
                }
            }

            else
                timer = 0;
        }
    }
}
