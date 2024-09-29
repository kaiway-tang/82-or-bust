using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    static int assignID;
    public static GameManager self;

    private void Start()
    {
        self = GetComponent<GameManager>();
    }

    public static int GetEntityID()
    {
        assignID++;
        return assignID;
    }

    public void Restart()
    {
        assignID = 0;
    }
}
