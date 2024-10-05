using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DJumpRingFX : MonoBehaviour
{
    [SerializeField] int life;
    [SerializeField] SpriteRenderer rend;
    [SerializeField] Transform trfm;
    [SerializeField] float scaleRate;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        trfm.localScale *= scaleRate;
        Tools.AddAlpha(rend, -1f/life);
        life--;
        if (life < 0) { Destroy(gameObject); }
    }
}
