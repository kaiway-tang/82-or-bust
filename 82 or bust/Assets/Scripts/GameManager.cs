using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    static int assignID;
    public static GameManager self;
    public static Player player;
    public static Transform playerTrfm;

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
