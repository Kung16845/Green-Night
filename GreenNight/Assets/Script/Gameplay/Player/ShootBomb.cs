using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ShootBomb : MonoBehaviour
{
    public List<GameObject> allbomb;
    public GameObject prefabBomb;
    public List<GameObject> listspawnedBombs;
    public Vector2 groundDispenseVelocity;
    public Vector2 verticalDispenseVelocity;
    public Transform trnsGun;
    public Transform trnsGunTip;
    private bool hasPressedG = false;
    public float maxChargeTime = 2f;
    public float minimumChargeTime = 0.75f;
    public float currentChargeTime = 0f;
    public UIInventory uIInventory;
    public int nubbombtype;

    private void Awake()
    {
        uIInventory = FindAnyObjectByType<UIInventory>();
    }

    void ChanageTypeBomb()
    {
        ItemData itemDataGrenade = uIInventory.listItemDataInventoryEqicment.FirstOrDefault(itemdata => itemdata.itemtype == Itemtype.Grenade);

        if(itemDataGrenade == null)
        {   
            prefabBomb = null;
            return;
        }
        
        prefabBomb = allbomb.FirstOrDefault(grenadeData => grenadeData.GetComponent<ItemClass>().idItem == itemDataGrenade.idItem);
        // if (bombtype != 0)
        // {
        //     if (bombtype == 1)
        //     {
        //         prefabBomb = allbomb.ElementAt(0);
        //         nubbombtype = 1;
        //     }
        //     else if (bombtype == 2)
        //     {
        //         prefabBomb = allbomb.ElementAt(1);
        //         nubbombtype = 2;
        //     }
        //     else if (bombtype == 3)
        //     {
        //         prefabBomb = allbomb.ElementAt(2);
        //         nubbombtype = 3;
        //     }
        // }
        // else
        // {
        //     prefabBomb = null;
        // }
    }

    void Update()
    {
        // ChanageTypeBomb();
        
        if (prefabBomb == null) return;

        if (Input.GetKeyDown(KeyCode.G) && !hasPressedG)
        {
            Debug.Log("G One Click");
            hasPressedG = true;
        }

        if (Input.GetKeyUp(KeyCode.G) && hasPressedG)
        {
            Debug.Log("G Released");
            if (currentChargeTime < minimumChargeTime)
            {
                ShootingBomb(groundDispenseVelocity * minimumChargeTime, verticalDispenseVelocity * minimumChargeTime);
            }
            else
            {
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
            currentChargeTime = Mathf.Min(currentChargeTime, maxChargeTime);
        }
    }

    void ShootingBomb(Vector2 ground, Vector2 vertical)
    {
        GameObject instantiatedBomb = SpawnedNewBomb();
        if (GetComponent<SpriteRenderer>().flipX)
        {
            instantiatedBomb.GetComponent<FakeHeightObject>().Initialize(
                -trnsGun.right * Random.Range(ground.x, ground.y),
                Random.Range(vertical.x, vertical.y));
        }
        else
        {
            instantiatedBomb.GetComponent<FakeHeightObject>().Initialize(
                trnsGun.right * Random.Range(ground.x, ground.y),
                Random.Range(vertical.x, vertical.y));
        }

        // ItemClass itemClassBomb = instantiatedBomb.GetComponent<ItemClass>();
        // uIInventory.RemoveItemData(itemClassBomb);
    }

    public GameObject SpawnedNewBomb()
    {
        GameObject instantiatedBomb = Instantiate(prefabBomb, trnsGunTip.position, Quaternion.identity);
        instantiatedBomb.GetComponent<FakeHeightObject>().shootBomb = this;
        listspawnedBombs.Add(instantiatedBomb);
        Debug.Log("Is Spawned Bomb");
        return instantiatedBomb;
    }

    public void DestroyNewBomb(GameObject bombToDestroy)
    {
        if (bombToDestroy == null) return;

        listspawnedBombs.Remove(bombToDestroy);
        Destroy(bombToDestroy);
    }
}
