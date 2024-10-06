using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartEnemy : Enemy
{
    public enum EnemyStates
    {
        Idle,
        Approach,
        Attack
    }

    float timeElapsed = 0;
    EnemyStates _state;
    protected EnemyStates currentState
    {
        get
        {
            return _state; 
        }
        set
        {
            timeElapsed = 0f;
            OnStateTransitioned(_state, value);
            _state = value;
        }
    }

    protected void OnStateTransitioned(EnemyStates prev, EnemyStates next)
    {
        OnStateEnd(prev); 
        OnStateBegin(next);
    }

    protected virtual void OnStateBegin(EnemyStates state)
    {

    }

    protected virtual void OnStateEnd(EnemyStates state)
    {

    }

    protected virtual void OnStateUpdate(EnemyStates state, float curTime)
    {
        switch (state)
        {
            case EnemyStates.Idle:
                break;
            case EnemyStates.Approach:
                break;
            case EnemyStates.Attack:
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        timeElapsed += Time.deltaTime;
        OnStateUpdate(currentState, timeElapsed);
    }
}
