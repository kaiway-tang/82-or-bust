using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPEntity : MonoBehaviour
{
    [SerializeField] protected int HP, maxHP, entityID;
    public GameObject baseObj;
    // Start is called before the first frame update
    protected void Start()
    {
        if (maxHP == 0)
        {
            maxHP = HP;
        }

        if (!baseObj) { baseObj = gameObject; }
    }

    protected void FixedUpdate()
    {
        
    }

    public delegate void OnDamage();
    public static event OnDamage damageEvent;

    public const int ALIVE = 0, DEAD = 1, IGNORED = 2;
    public virtual int TakeDamage(int amount, int sourceID)
    {
        if (sourceID != 0 && sourceID == entityID) { return IGNORED; }

        HP -= amount;

        damageEvent?.Invoke();
        
        if (HP <= 0)
        {
            return DEAD;
        }
        return ALIVE;
    }
}
