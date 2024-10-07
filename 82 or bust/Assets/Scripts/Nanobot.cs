using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Nanobot : MobileEntity
{
    [SerializeField] NavMeshAgent agent;
    public LayerMask raycastLayer;

    [SerializeField] float accl, maxSpeed, turnSpd, friction;
    [SerializeField] OnGround rightAntenna, leftAntenna;
    [SerializeField] Transform target;

    [SerializeField] int state = 0;
    const int SCATTERING = 0, SEEKING = 1, TRANSITION = 2;

    int life;

    Transform trfm;
    Vector3 agentPos;
    /*
     * If idle, transition into move state
     * At start of move state, find suitable target. 
     * During move state, If no target was found, move into idle state. Otherwise follow our target.
     * During move state, if the player is too close, move into attack state. 
     * During attack state, raycast directly away from the player and attempt to navigate to position of first collision.
     * During attack state, if player is too far, transition into idle state
     */

    // Start is called before the first frame update
    int difficulty;
    new void Start()
    {
        base.Start();
        trfm = transform;
        agent.transform.parent = null;

        trfm.Rotate(Vector3.forward * Random.Range(-100, 101));
        Scatter(trfm.position + trfm.up * -1, 250);

        difficulty = GameManager.self.difficulty;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.F))
        {
            Scatter(CursorObj.trfm.position, 150);
        }
    }

    private new void FixedUpdate()
    {
        base.FixedUpdate();
        Animate();
        life++;

        if (GameManager.self.difficulty > difficulty)
        {
            Destroy(baseObj);
        }

        if (state == SCATTERING)
        {
            HandleScattering();
        }
        else if (state == SEEKING)
        {
            HandleSeeking();
        }
        else if (state == TRANSITION)
        {
            HandleTransition();
        }
    }

    int transitionTmr;
    void HandleTransition()
    {
        transitionTmr--;
        if (transitionTmr < 1)
        {
            Seek();
        }

        ApplyDirectionalFriction(friction);
    }

    InactiveShell targetShell;
    public void Seek()
    {
        SetTarget();

        agent.transform.position = trfm.position;
        agent.enabled = true;
        state = SEEKING;
    }

    void SetTarget()
    {
        targetShell = LevelManager.GetClosestShell(trfm.position);
        if (targetShell) { target = targetShell.trfm; }
        else { target = LevelManager.self.defaultNanobotPos; }
    }

    void HandleSeeking()
    {
        if (!target) { SetTarget(); }

        agentPos = agent.transform.position;
        if (!Tools.InDistance(agentPos, trfm.position, .1f))
        {
            agent.transform.position = (trfm.position + agentPos) * .5f;
            agentPos = agent.transform.position;
        }
        agentPos.z = 0;

        if (rb.velocity.sqrMagnitude < maxSpeed * maxSpeed)
        {
            vect2 = (agentPos - trfm.position).normalized;
            rb.velocity += vect2 * accl;
        }
        ApplyDirectionalFriction(friction);

        Tools.FacePosition(trfm, agentPos, 0.1f, -90);

        if (Tools.BoxDist(trfm.position, target.position) < .4f)
        {
            if (targetShell) { targetShell.CollectNanobot(); }            
            Destroy(gameObject);
        }

        agent.SetDestination(target.position);
    }

    public void Scatter(Vector3 point, int duration = 50)
    {
        scatterPoint = point;
        scatterPoint.z = trfm.position.z;
        scatterTmr = duration;
        state = SCATTERING;
        agent.enabled = false;
        trfm.up = trfm.position - scatterPoint;
        rb.velocity = trfm.up * maxSpeed * .6f;

        trfm.Rotate(Vector3.forward * Random.Range(-22, 23));
    }

    Vector3 scatterPoint;
    int scatterTmr;
    void HandleScattering()
    {
        if (scatterTmr > 0) { scatterTmr--; }
        if (scatterTmr < 1)
        {
            state = TRANSITION;
            transitionTmr = 35;
            return;
        }

        if (rightAntenna.touchCount > 0) { trfm.Rotate(Vector3.forward * turnSpd); }
        else if (leftAntenna.touchCount > 0) { trfm.Rotate(Vector3.forward * -turnSpd); }

        vect2 = trfm.up;
        if (rightAntenna.touchCount < 1 || rightAntenna.touchCount < 1)
        {
            if (rb.velocity.sqrMagnitude < maxSpeed * maxSpeed) { rb.velocity += vect2 * accl; }
        }
        else { rb.velocity -= vect2 * accl; }

        ApplyDirectionalFriction(friction);
    }

    void Animate()
    {

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
        Vector2 directionAwayFromPlayer = (transform.position - Player.self.trfm.position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, directionAwayFromPlayer, Mathf.Infinity);

        if (hit.collider != null)
        {
            // Move towards the point of first collision
            agent.SetDestination(hit.point);
        }
    }

    [SerializeField] int damageTrauma;
    [SerializeField] GameObject damageFX;
    public override int TakeDamage(int amount, int sourceID)
    {
        if (life < 10) { return IGNORED; }
        int result = base.TakeDamage(amount, sourceID);
        if (result != HPEntity.IGNORED)
        {
            Instantiate(damageFX, trfm.position, Quaternion.identity);
            if (result == HPEntity.DEAD)
            {
                CameraManager.SetTrauma(damageTrauma);
                Destroy(baseObj);
            }
            else
            {
                CameraManager.SetTrauma(damageTrauma);
            }
        }
        return result;
    }
}
