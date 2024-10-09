using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieGrunt : Zombie
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {     
        if (currentHp <= 0)
        {
            currentState = ZombieState.Dead;
            return;
        }
        if(HasReachedAttackPoint())
        {
            rb2D.velocity = Vector2.zero;
            ZombieAttack();
        }
        else
        {
            ZombieMoveFindBarrier();
        }
    }
}
