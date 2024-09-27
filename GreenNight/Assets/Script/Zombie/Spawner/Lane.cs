using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Lane
{
    public int laneIndex;
    public Transform spawnPoint;
    public Transform attackPoint;
    public List<Transform> waypoints;
}
