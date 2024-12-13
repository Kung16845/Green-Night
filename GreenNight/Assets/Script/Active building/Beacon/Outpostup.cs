using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening; 

public class Outpostup : MonoBehaviour
{
    public bool issetUpOutpost;
    public GameObject ActivOutsetUpUi;
    private OutpostSystem outpostSystem;
    private SpriteRenderer spriteRenderer;
    private bool inrange;
    void Start()
    {
        outpostSystem = FindObjectOfType<OutpostSystem>();
    }
     void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            inrange = true;
            spriteRenderer.DOColor(Color.green, 0.5f);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            inrange = false;
            spriteRenderer.DOColor(Color.white, 0.5f);
        }
    }
    private void Update()
    {

        if (inrange && Input.GetKeyDown(KeyCode.F))
        {
            if (ActivOutsetUpUi.activeSelf)
                ActivOutsetUpUi.SetActive(false);
            else
                ActivOutsetUpUi.SetActive(true);
        }
        else if (!inrange && ActivOutsetUpUi.activeSelf)
        {
            ActivOutsetUpUi.SetActive(false);
        }
    }
    public void AddOutpost()
    {
        outpostSystem.AddOutpostReward(Location.Mall, 10);
        issetUpOutpost = true;
    }
    public void RemoveOutpost()
    {
        outpostSystem.RemoveOutpostReward(Location.Mall);
        issetUpOutpost = false;
    }
}
