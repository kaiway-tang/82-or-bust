using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    [SerializeField] float accl, maxSpeed, minSpeed, turnSpeed;
    [SerializeField] GameObject explosion;
    public float speed;
    Transform trfm;
    // Start is called before the first frame update
    void Start()
    {
        trfm = transform;
        speed = minSpeed;
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
        Tools.FacePosition(trfm, Player.self.trfm.position, turnSpeed * speed, -90);
    }

    bool detonated = false;
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (detonated) { return; }
        if (Layers.AnyCollision(col.gameObject.layer))
        {
            Instantiate(explosion, trfm.position, Quaternion.identity);
            detonated = true;
            Destroy(gameObject);
        }
    }
}
