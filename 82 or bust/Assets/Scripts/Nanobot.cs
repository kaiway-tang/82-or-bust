using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Nanobot : SmartEnemy
{
    [SerializeField] Transform followTarget;
    NavMeshAgent agent;
    [SerializeField] float approachDistance = 10f;  // Distance to start attacking
    [SerializeField] float attackDistance = 3f;     // Distance to trigger attack
    public LayerMask raycastLayer;

    private Transform target;

    /*
     * If idle, transition into move state
     * At start of move state, find suitable target. 
     * During move state, If no target was found, move into idle state. Otherwise follow our target.
     * During move state, if the player is too close, move into attack state. 
     * During attack state, raycast directly away from the player and attempt to navigate to position of first collision.
     * During attack state, if player is too far, transition into idle state
     */

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        agent = GetComponent<NavMeshAgent>();
        currentState = EnemyStates.Idle;
    }

    


    //// Update is called once per frame
    //void Update()
    //{
    //    if (followTarget)
    //    {
    //        agent.SetDestination(followTarget.position);
    //    }
    //}

    protected override void OnStateBegin(EnemyStates state)
    {
        switch (state)
        {
            case EnemyStates.Idle:
                break;
            case EnemyStates.Approach:
                // Find a suitable target at the start of Approach state
                target = FindTarget();
                break;
            case EnemyStates.Attack:
                // In Attack state, we try to navigate away from the player
                RaycastAwayFromPlayer();
                break;
        }
    }

    protected override void OnStateEnd(EnemyStates state)
    {
        // Cleanup code for state transitions can be added here, if needed.
    }

    protected override void OnStateUpdate(EnemyStates state, float curTime)
    {
        switch (state)
        {
            case EnemyStates.Idle:
                // In Idle, we immediately move to approach state as described
                currentState = EnemyStates.Approach;
                break;

            case EnemyStates.Approach:
                if (target == null)
                {
                    // No target found, go back to idle state
                    currentState = EnemyStates.Idle;
                }
                else
                {
                    // Move towards target
                    agent.SetDestination(target.position);

                    // If within attack range, transition to attack state
                    //float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
                    //if (distanceToPlayer <= attackDistance)
                    //{
                    //    currentState = EnemyStates.Attack;
                    //}
                }
                break;

            case EnemyStates.Attack:
                float currentDistanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

                if (currentDistanceToPlayer > approachDistance)
                {
                    // Player is too far, transition to idle
                    currentState = EnemyStates.Idle;
                }
                // Raycast and navigate was handled in OnStateBegin, no additional movement logic here.
                break;
        }
    }

    private Transform FindTarget()
    {
        /*
         * Scan within a radius for any InactiveCore objects. If none are found, return null.
         * If any are found, choose the one requiring the least amount to activate and navigate to its position.
         */
        // Define the search radius
        float searchRadius = 20f;  // Adjust this as needed

        // Perform a 2D circle cast to find all colliders within the search radius
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, searchRadius, Vector2.zero);

        InactiveCore closestCore = null;
        float leastAmountToActivate = Mathf.Infinity;

        // Loop through all the hits to find InactiveCore objects
        foreach (RaycastHit2D hit in hits)
        {
            // Check if the hit object has an InactiveCore component
            InactiveCore core = hit.collider.GetComponent<InactiveCore>();
            if (core != null)
            {
                // Check how many more units the core needs for activation
                int remainingForActivation = core.GetAmountRemainingForActivation();

                // Choose the core with the least amount required for activation
                if (remainingForActivation > 0 && remainingForActivation < leastAmountToActivate)
                {
                    leastAmountToActivate = remainingForActivation;
                    closestCore = core;
                }
            }
        }
        Debug.Log(closestCore.gameObject.name);
        // If a suitable core was found, return its transform; otherwise, return null
        return closestCore != null ? closestCore.transform : null;
    }

    private void RaycastAwayFromPlayer()
    {
        // Raycast directly away from the player to escape
        Vector2 directionAwayFromPlayer = (transform.position - player.transform.position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, directionAwayFromPlayer, Mathf.Infinity);

        if (hit.collider != null)
        {
            // Move towards the point of first collision
            agent.SetDestination(hit.point);
        }
    }

}
