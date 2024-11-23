using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;
public class NpcMovement : MonoBehaviour
{
    public List<Transform> listWayPointWalk;
    public Transform currentWayPoint;
    public NavMeshAgent agent;
    public AnimationController animationController;
    public bool isMoving = false;
    public float timeCount = 0;
    public float maxTimeCount;
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animationController = GetComponent<AnimationController>();
        transform.rotation = Quaternion.Euler(0, 0, 0);
        isMoving = WalkContinue();
        
        agent.avoidancePriority = Random.Range(0, 100); // กำหนดค่าความสำคัญแบบสุ่ม
        agent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance; // ปิดการหลีกเลี่ยงสิ่งกีดขวาง

    }
    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Euler(0, 0, 0);
        if (isMoving)
        {
            WalkWaypoint();

            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                isMoving = WalkContinue();
            }

        }
        else
        {
            timeCount += Time.deltaTime;
            animationController.iswalk = false;
            if (timeCount >= maxTimeCount)
            {
                timeCount = 0;
                WalkWaypoint();
            }
        }

    }
    public void WalkWaypoint()
    {
        currentWayPoint = listWayPointWalk.ElementAt(Random.Range(0, listWayPointWalk.Count));
        agent.SetDestination(currentWayPoint.position);
        animationController.iswalk = true;
    }
    public bool WalkContinue()
    {   
        float randomValue = Random.Range(0f, 100f);
        maxTimeCount = Random.Range(5, 16);
        return randomValue >= 50.00f;
    }
}
