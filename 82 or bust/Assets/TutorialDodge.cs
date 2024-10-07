using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialDodge : MonoBehaviour
{
    [SerializeField] GameObject rocket, mouseIndicator;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    bool waitForRClick;
    [SerializeField] int freezeTmr;
    int timer;
    void FixedUpdate()
    {
        if (timer > 0)
        {
            if (waitForRClick)
            {
                if (Input.GetMouseButton(1))
                {
                    Player.self.movementLock--;
                    Player.self.AddMana(9999);
                    Player.self.DashRollCast();
                }
            }
            else
            {
                Player.self.AddMana(-100);
                timer++;
                if (Vector3.Distance(Player.self.trfm.position, rocket.transform.position) < 1)
                {
                    rocket.GetComponent<Rocket>().speed = 0;
                    waitForRClick = true;
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        Player.self.movementLock++;
        Player.self.AddMana(-9999);
        rocket.SetActive(true);
    }
}
