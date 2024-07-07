using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class ShootBomb : MonoBehaviour
{
    public List<GameObject> allbomb;
    public GameObject prefabBomb;
    public List<GameObject> listspawnedBombs;
    // Start is called before the first frame update
    public Vector2 groundDispenseVelocity;
    public Vector2 verticalDispenseVelocity;
    public Transform trnsGun;
    public Transform trnsGunTip;
    private bool hasPressedG = false;
    public float maxChargeTime = 2f;
    public float minimumhargeTime = 0.75f;
    public float currentChargeTime = 0f;
    public InventorySlot inventorySlot;
    public int nubbombtype;
    private void Awake()
    {
        inventorySlot = FindAnyObjectByType<InventorySlot>();
    }
    void ChanageTypeBomb()
    {
        var listbombSlot = inventorySlot.listbombSlot;
       
        var bombtype = listbombSlot.FirstOrDefault(nameUi => nameUi.iDItems != 0)?.iDItems ?? 0;
        
        if (bombtype != 0 )
        {
            if (bombtype == 1)
            {
                prefabBomb = allbomb.ElementAt(0);
                nubbombtype=1;
            }
            else if (bombtype == 2)
            {
                prefabBomb = allbomb.ElementAt(1);
                nubbombtype = 2;
            }
            else if (bombtype == 3)
            {
                prefabBomb = allbomb.ElementAt(2);
                nubbombtype = 3;
            }
        }
        else
        {
            prefabBomb = null;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
        ChanageTypeBomb();
        if (prefabBomb == null) return;

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
                // If charge time is lower than minimum, use minimum charge
                ShootingBomb(groundDispenseVelocity * minimumhargeTime, verticalDispenseVelocity * minimumhargeTime);
            }
            else
            {
                // Otherwise, use current charge time
                if (currentChargeTime < maxChargeTime)
                {
                    ShootingBomb(groundDispenseVelocity * currentChargeTime, verticalDispenseVelocity * currentChargeTime);
                }
                else
                {
                    ShootingBomb(groundDispenseVelocity * maxChargeTime, verticalDispenseVelocity * maxChargeTime);
                }
            }

            hasPressedG = false;
            currentChargeTime = 0;
        }

        if (Input.GetKey(KeyCode.G))
        {
            currentChargeTime += Time.deltaTime;

            // Clamp the charge time to not exceed maxChargeTime
            currentChargeTime = Mathf.Min(currentChargeTime, maxChargeTime);
        }
    }

  
    void ShootingBomb(Vector2 ground, Vector2 vertical)
    {

        GameObject insantiatedBomb = SpawnedNewBomb();
        if (GetComponent<SpriteRenderer>().flipX)
            insantiatedBomb.GetComponent<FakeHeightObject>().Initialize(-trnsGun.right * Random.Range(ground.x, ground.y),
        Random.Range(vertical.x, vertical.y));
        else
            insantiatedBomb.GetComponent<FakeHeightObject>().Initialize(trnsGun.right * Random.Range(ground.x, ground.y),
        Random.Range(vertical.x, vertical.y));

        inventorySlot.RemoveItem(nubbombtype);
        nubbombtype = 0;
    }
    public GameObject SpawnedNewBomb()
    {
        GameObject insantiatedBomb = Instantiate(prefabBomb, trnsGunTip.position, Quaternion.identity);
        insantiatedBomb.GetComponent<FakeHeightObject>().shootBomb = this;
        listspawnedBombs.Add(insantiatedBomb);
        
        Debug.Log("Is Spawned Bomb");
        return insantiatedBomb;
    }
    
    
}
