using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject _exclamation;
    public static GameObject exclamation;

    static int assignID;
    public static GameManager self;
    public static Player player;
    public static Transform playerTrfm;
    public static PosTracker playerPosTracker;

    private void Awake()
    {
        exclamation = _exclamation;
    }

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
