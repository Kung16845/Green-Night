using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;
public class FakeHeightObject : MonoBehaviour
{
    public GameObject areaBombDamage;
    public Transform trnsObject;
    public Transform trnsBody;
    public Transform trnsShadow;
    public float gravity = -10;
    public Vector2 groundVelocity;
    public float verticalVelocity;
    private float lastIntialVerticalVelocity;
    public bool isGrounded;
    public float arttime;
    public ShootBomb shootBomb;
    
    private void Update()
    {
        UpdatePosition();
        CheckGroundHit();
    }
    public void Initialize(Vector2 groundVelocity, float verticalVelocity)
    {

        this.groundVelocity = groundVelocity;
        this.verticalVelocity = verticalVelocity;
        lastIntialVerticalVelocity = verticalVelocity;
    }
    void UpdatePosition()
    {
        if (!isGrounded)
        {
            verticalVelocity += gravity * Time.deltaTime;
            trnsBody.position += new Vector3(0, verticalVelocity, 0) * Time.deltaTime;
        }
        trnsObject.position += (Vector3)groundVelocity * Time.deltaTime;

    }
    void CheckGroundHit()
    {
        if (trnsBody.position.y < trnsObject.position.y && !isGrounded)
        {   
            trnsBody.position = trnsObject.position;
            isGrounded = true;
            Stick();
        }
    }

    public void Stick()
    {
        groundVelocity = Vector2.zero;
        ulong networkid = GetComponent<NetworkObject>().NetworkObjectId;
        StartCoroutine(DetonateAfterDelay(networkid, arttime)); // Detonate after 1 second
    }

    private IEnumerator DetonateAfterDelay(ulong networkid, float delay)
    {
        yield return new WaitForSeconds(delay);

        SpawnAreaDamageBomb();
        shootBomb.DestroyNewBombServerRpc(networkid);
    }

    public void SpawnAreaDamageBomb()
    {
        
        GameObject areaDamge = Instantiate(areaBombDamage,transform.position, Quaternion.identity);
       
        
       
    }


}
