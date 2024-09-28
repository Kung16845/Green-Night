using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

[Serializable]
public class NpcClass 
{
    [Header("Resoure Per Day ")]
    public int foodPerDay;
    public int bed;
    
    [Header("Stat Npc")]
    
    [Range(1,10)]
    public float hp; 
    [Range(1, 100)]
    public float morale;
    [Range(1, 10)]
    public int endurance;
    [Range(1, 10)]
    public int combat;
    [Range(1, 10)]
    public int speed;
    public float recoverSpeed;
    public bool isActive;
    [Header("Player Link")]
    [Range(6,12)]
    public int countInventorySlot;
    public int idnpc;
    public SpecialistRoleNpc roleNpc;
    public string leaderSkill;
    [Header("ID Coutume")]
    public int idHead;
    public int idBody;
    public int idFeed;
}
public enum SpecialistRoleNpc
{
    Handicraft,
    Maintainance,
    Network,
    Scavenger,
    Military_training,
    Chemical,
    Doctor,
    Entertainer


}
public class HeadCoutume : MonoBehaviour
{
    public int idHead;
    public Sprite spriteHead;
}
public class BodyCoutume : MonoBehaviour
{
    public int idBody;
    public Sprite spriteBody;
    public Sprite spriteLeftArm;
    public Sprite spriteRightArm;
    public Sprite spriteShoulderLeft;
    public Sprite spriteShoulderRight;

}
public class FeedCoutume: MonoBehaviour
{
    public int idFeed;
    public Sprite spriteUpperLagRight;
    public Sprite spriteUpperLagLeft;
    public Sprite spriteLowerLagRight;
    public Sprite spriteLowerLagLeft;
}
