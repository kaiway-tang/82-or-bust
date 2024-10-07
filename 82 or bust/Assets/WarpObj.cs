using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WarpObj : MonoBehaviour
{
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Locomotor locomotor;
    public Transform trfm;
    [SerializeField] SpriteRenderer rend;
    // Start is called before the first frame update
    void Start()
    {

    }

    int timer;
    public void Activate()
    {
        gameObject.SetActive(true);
        trfm.parent = null;
        trail.emitting = true;
        rend.enabled = true;
        ptclSys.Play();
        timer = 150;
    }

    private void FixedUpdate()
    {
        if (objDisableDelay > 0)
        {
            objDisableDelay--;
            if (objDisableDelay == 0) { gameObject.SetActive(false); }
            return;
        }

        agent.SetDestination(Player.self.trfm.position);
        if (Tools.BoxDist(trfm.position, Player.self.trfm.position) < 2)
        {
            EndWarp();
        }
        timer--;
        if (timer < 1)
        {
            EndWarp();
        }

        
    }

    int objDisableDelay;
    [SerializeField] TrailRenderer trail;
    [SerializeField] ParticleSystem ptclSys;
    [SerializeReference] GameObject warpEndFX;
    void EndWarp()
    { 
        if (locomotor) { locomotor.EndWarp(); } else { Destroy(gameObject); }
        rend.enabled = false;
        trail.emitting = false;
        ptclSys.Stop();
        objDisableDelay = 25;
        Instantiate(warpEndFX, trfm.position, Quaternion.identity);
        //gameObject.SetActive(false);
    }
}
