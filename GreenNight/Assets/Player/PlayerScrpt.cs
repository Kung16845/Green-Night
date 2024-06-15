using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;
using UnityEngine.UI;
public class PlayerScrpt : MonoBehaviour
{
    public float currentHp;
    public float maxHp;
    public float holdTime = 2f;
    public float timer = 0f;
    public BagAmmo bagAmmo;
    public BoxAmmoSpwaner boxSpwaner;
    public Slider timerBar;
    [SerializeField] private UIScript uiScript;
    private void Start()
    {
       

        boxSpwaner = FindObjectOfType<BoxAmmoSpwaner>();
        GameObject sliderObject = GameObject.FindGameObjectWithTag("Slider");
        if (sliderObject != null)
        {
            timerBar = sliderObject.GetComponent<Slider>();
        }

        UIScript[] allUIScripts = Resources.FindObjectsOfTypeAll<UIScript>();
        foreach (UIScript uiScriptall in allUIScripts)
        {
            // ตรวจสอบว่าตัว GameObject นั้น active หรือไม่
            if (!uiScriptall.gameObject.activeInHierarchy)
            {
                uiScript = uiScriptall;
            }

        }
        if (IsHost)
        {
            uiScript = GameObject.FindObjectOfType<UIScript>();

        }
        if (IsClient)
        {
            uiScript = GameObject.FindObjectOfType<UIScript>();
        }

        if (uiScript != null)
            uiScript.allPlayer.Add(this.gameObject);

    }
    private void Awake()
    {
        
        boxSpwaner = FindObjectOfType<BoxAmmoSpwaner>();
        GameObject sliderObject = GameObject.FindGameObjectWithTag("Slider");
        if (sliderObject != null)
        {
            timerBar = sliderObject.GetComponent<Slider>();
        }

        UIScript[] allUIScripts = Resources.FindObjectsOfTypeAll<UIScript>();
        foreach (UIScript uiScriptall in allUIScripts)
        {
            // ตรวจสอบว่าตัว GameObject นั้น active หรือไม่
            if (!uiScriptall.gameObject.activeInHierarchy)
            {
                uiScript = uiScriptall;
            }

        }
        if (IsHost)
        {
            uiScript = GameObject.FindObjectOfType<UIScript>();

        }
        if (IsClient)
        {
            uiScript = GameObject.FindObjectOfType<UIScript>();
        }

        if (uiScript != null)
            uiScript.allPlayer.Add(this.gameObject);
    }
    private void Update()
    {
        if (!IsOwner) return;

        if (bagAmmo != null)
        {
            if (Input.GetKey(KeyCode.E))
            {
                timer += Time.deltaTime;
                if (timer >= holdTime)
                {
                    ulong bagAmmoiDObject = bagAmmo.GetComponent<NetworkObject>().NetworkObjectId;
                    OpenBag(bagAmmoiDObject);
                }
            }

            else
                timer = 0;
        }
        UpdateTimerUI();

        if (bagAmmo != null && bagAmmo.isOpening.Value)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                bagAmmo.RefillAmmoServerRpc();
            }
        }

    }

    void UpdateTimerUI()
    {
        if (timer != 0 && !bagAmmo.isOpening.Value)
        {
            timerBar.gameObject.SetActive(true);
            float ratio = timer / holdTime;
            timerBar.value = ratio;
        }
        else
        {
            timerBar.gameObject.SetActive(false);
        }

    }
    
    public void OpenBag()
    {
        

        if (bagAmmo != null)
        {
            
            bagAmmo.isOpening.Value = true;
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, GetComponent<Collider2D>().bounds.size);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!IsOwner) return;


        if (other.GetComponent<BagAmmo>() != null)
        {

            this.bagAmmo = other.GetComponent<BagAmmo>();

        }

    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (!IsOwner) return;
        this.bagAmmo = null;

    }
    
    public void TakeDamagePlayer(float damage)
    {
        this.currentHp -= damage;

        if (this.currentHp <= 0)
        {
            

        }

    }
}
