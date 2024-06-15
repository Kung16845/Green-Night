using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Barrier : MonoBehaviour
{
    public float currentHpBarrier;
    public float maxHpBarrier;
    public Slider sliderHp;
    private void Start() {
        currentHpBarrier = maxHpBarrier;
    }
    void UpdateHpBarrierUI()
    {
        if (currentHpBarrier != 0 )
        {
            
            float ratio = currentHpBarrier / maxHpBarrier;
            sliderHp.value = ratio;
        }
        
    }
    private void Update() 
    {   
        UpdateHpBarrierUI();
    }
    
    public void TakeDamageBarrier(float danage)
    {   
        currentHpBarrier -= danage;
        if(currentHpBarrier <= 0)
        {
            // SceneManager.LoadSceneAsync(0);
            Time.timeScale = 0;
        } 
    }

    public void HealHpBarrier(float heal)
    {
        currentHpBarrier += heal;
        if(currentHpBarrier > maxHpBarrier)
        {
            currentHpBarrier = maxHpBarrier;
        }
    }
 
    void DestroyBarrierServerRpc()
    {
       
        Destroy(gameObject);
    }

}
