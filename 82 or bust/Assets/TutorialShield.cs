using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialShield : HPEntity
{
    [SerializeField] GameObject mouseIndicator;
    // Update is called once per frame
    void Update()
    {
        if (inSloMo && Input.GetMouseButtonDown(1))
        {
            Time.timeScale = 1;
            mouseIndicator.SetActive(false);
            Destroy(gameObject);
        }
    }

    bool inSloMo;
    public override int TakeDamage(int amount, int sourceID)
    {
        int result = base.TakeDamage(amount, sourceID);
        if (result == IGNORED) { return result; }

        Time.timeScale = 0.05f;
        inSloMo = true;
        mouseIndicator.SetActive(true);
        mouseIndicator.transform.position = Player.self.trfm.position + new Vector3(2,0.5f,0);

        return result;
    }
}
