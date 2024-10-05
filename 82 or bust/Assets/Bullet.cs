using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : Hitbox
{
    [SerializeField] float speed;
    Transform trfm;
    // Start is called before the first frame update
    void Start()
    {
        trfm = transform;
    }

    void FixedUpdate()
    {
        trfm.position += trfm.right * speed;
    }

    private new void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        Destroy(gameObject);
    }
}
