using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    public int count;
    private void OnTriggerEnter2D(Collider2D col)
    {
        count++;
    }
    private void OnTriggerExit2D(Collider2D col)
    {
        count--;
    }
}
