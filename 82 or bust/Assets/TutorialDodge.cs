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
                Player.self.mana = 300;
                rocket.GetComponent<Rocket>().speed = 0;
                if (Input.GetMouseButton(1))
                {
                    rocket.GetComponent<Rocket>().speed = 0.15f;
                    mouseIndicator.SetActive(false);
                    Player.self.movementLock--;
                    waitForRClick = false;
                    timer = 1000;
                }
            }
            else if (timer < 999)
            {
                Player.self.AddMana(-100);
                timer++;
                if (Vector3.Distance(Player.self.trfm.position, rocket.transform.position) < 1)
                {
                    rocket.GetComponent<Rocket>().speed = 0;
                    mouseIndicator.SetActive(true);
                    waitForRClick = true;
                }
            }
        }
        else if (Player.self.transform.position.x > transform.position.x)
        {
            Player.self.movementLock++;
            Player.self.AddMana(-9999);
            rocket.SetActive(true);
            timer = 1;
        }
    }
}
