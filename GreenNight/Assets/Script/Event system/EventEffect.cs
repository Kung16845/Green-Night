using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventEffect : MonoBehaviour
{
    [Header("CurrentEvent")]
    public CardEvent cardEvent = CardEvent.None;
    public GameObject Fog;

    void Start()
    {
        ActivaEvent();
    }
    private void ActivaEvent()
    {
        switch (cardEvent)
        {
            case CardEvent.Fog:
                Fog.SetActive(true);
                Debug.Log("Fog event activated.");
                break;

            case CardEvent.Swarm:
                // Implement the effect for Swarm here
                Debug.Log("Swarm event activated.");
                break;

            case CardEvent.Infectednest:
                // Implement the effect for Infectednest here
                Debug.Log("Infected Nest event activated.");
                break;

            default:
                // Optional: Handle cases where the event is None or not specified
                Debug.Log("No event or unhandled event type.");
                break;
        }
    }
}