using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRepair : MonoBehaviour
{
    public Barrier barrier;
    public float timecount;
    public float maxtime;
    public float heal;
    public InventorySlot inventorySlot;
    private void Awake()
    {
        inventorySlot = FindAnyObjectByType<InventorySlot>();
    }
    private void Update()
    {
        
        var uiTool = inventorySlot.toolSlot;
        if (barrier != null && Input.GetKey(KeyCode.LeftAlt)&& uiTool.iDItems == 4)
        {
            if (timecount < maxtime)
            {
                timecount += Time.deltaTime;
            }
            else if (timecount >= maxtime)
            {
                barrier.HealHpBarrier(heal);
                inventorySlot.RemoveItem(4);
            }
        }
        else
            timecount = 0;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        var barrierTrigger = other.GetComponent<Barrier>();
        if (barrierTrigger != null)
        {
            barrier = barrierTrigger;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        barrier = null;
    }
}
