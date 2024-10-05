using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : Hitbox
{
    [SerializeField] float speed;
    // Start is called before the first frame update
    protected new void Start()
    {
        base.Start();
        trfm = transform;
    }

    protected new void FixedUpdate()
    {
        base.FixedUpdate();
        trfm.position += trfm.right * speed;
    }

    private new void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        Destroy(gameObject);
    }
}
