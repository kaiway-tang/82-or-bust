using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    [SerializeField] float accl, maxSpeed, turnSpeed;
    [SerializeField] GameObject explosion;
    [SerializeField] float speed;
    Transform trfm;
    // Start is called before the first frame update
    void Start()
    {
        trfm = transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (speed < maxSpeed)
        {
            speed += accl;
            if (speed > maxSpeed) { speed = maxSpeed; }
        }

        trfm.position += trfm.up * speed;
        Tools.LerpRotation(trfm, Player.self.trfm.position, turnSpeed * speed, -90);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (Layers.AnyCollision(col.gameObject.layer))
        {
            Instantiate(explosion, trfm.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
