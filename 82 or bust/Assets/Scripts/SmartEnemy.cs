using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartEnemy : MonoBehaviour
{
    public enum EnemyStates
    {
        Idle,
        Move,
        Attack
    }

    float timeElapsed = 0;
    EnemyStates _state;
    EnemyStates currentState
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

    protected void OnStateBegin(EnemyStates state)
    {

    }

    protected void OnStateEnd(EnemyStates state)
    {

    }

    protected void OnStateUpdate(EnemyStates state, float curTime)
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timeElapsed += Time.deltaTime;
        OnStateUpdate(currentState, timeElapsed);
    }
}
