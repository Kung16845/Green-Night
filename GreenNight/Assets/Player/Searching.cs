using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using UnityEngine;

public class Searching : MonoBehaviour
{

    // public BoxAmmoSpwaner boxAmmoSpwaner;
    public float timecount;
    public float maxtime;
    
    // Update is called once per frame
    void Update()
    {
  

        if(boxAmmoSpwaner != null && boxAmmoSpwaner.isCanSpawn.Value && Input.GetKey(KeyCode.F))
        {   
            if(timecount < maxtime)
            {
                timecount += Time.deltaTime;   
            }
            else if(timecount >= maxtime)
            {
                boxAmmoSpwaner.isCanSpawn.Value = false;
                boxAmmoSpwaner.SpawnedBagAmmoServerRpc();  
            }
        }
        else 
            timecount = 0;
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        var boxSpwaner = other.GetComponent<BoxAmmoSpwaner>();
        if (other.GetComponent<BoxAmmoSpwaner>() != null)
        {
            boxAmmoSpwaner = boxSpwaner;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        boxAmmoSpwaner = null;
    }
}
