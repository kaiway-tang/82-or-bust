using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InactiveCore : MonoBehaviour
{
    [SerializeField] GameObject activePrefab;
    [SerializeField] int amountForActivation = 1;
    int curAmount = 0;

    public void InhabitCore()
    {
        ++curAmount;
        if (curAmount >= amountForActivation)
        {
            Instantiate(activePrefab, transform.position, Quaternion.identity);
            // Can add effects here as needed
            Destroy(gameObject);
        }
    }

    public int GetAmountRemainingForActivation()
    {
        return amountForActivation - curAmount;
    }
}
