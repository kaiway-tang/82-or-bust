using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WarpObj : MonoBehaviour
{
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Locomotor locomotor;
    public Transform trfm;
    // Start is called before the first frame update
    void Start()
    {

    }

    int timer;
    public void Activate()
    {
        gameObject.SetActive(true);
        trfm.parent = null;
        timer = 150;
    }

    private void FixedUpdate()
    {
        agent.SetDestination(Player.self.trfm.position);
        if ((Player.self.trfm.position - trfm.position).sqrMagnitude < 4)
        {
            EndWarp();
        }
        timer--;
        if (timer < 1)
        {
            EndWarp();
        }
    }

    void EndWarp()
    { 
        locomotor.EndWarp();
        gameObject.SetActive(false);
    }
}
