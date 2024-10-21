using System.Collections.Generic;
using UnityEngine;

public enum CardEvent
{
    None,
    Fog,
    Supplydrop,
    PrisonBreak,
    Blackout,
    Rain,
    Swarm,
    Infectednest,
    HuntingFestival
}

[System.Serializable]
public class EventDuration
{
    public CardEvent cardEvent;
    public int minDuration;
    public int maxDuration;

    public int GetRandomDuration()
    {
        if (minDuration == maxDuration)
            return minDuration;
        else
            return Random.Range(minDuration, maxDuration + 1); // +1 because max is exclusive
    }
}

public class EventCard : MonoBehaviour
{
    public int DurationLeft;

    [Header("Event Settings")]
    public CardEvent cardEvent = CardEvent.None;

    [Header("Event Durations")]
    public List<EventDuration> eventDurations = new List<EventDuration>();

    void Start()
    {
        SetEventDuration();
    }

    void SetEventDuration()
    {
        EventDuration duration = eventDurations.Find(ed => ed.cardEvent == cardEvent);
        if (duration != null)
        {
            DurationLeft = duration.GetRandomDuration();
        }
        else
        {
            // Default duration if event is not found
            DurationLeft = 0;
        }
    }
}
