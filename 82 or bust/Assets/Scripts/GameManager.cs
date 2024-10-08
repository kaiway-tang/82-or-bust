using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject _exclamation;
    public static GameObject exclamation;
    [SerializeField] GameObject EndUI;

    static int assignID;
    public static GameManager self;
    public static Player player;
    public static Transform playerTrfm;
    public static PosTracker playerPosTracker;

    public int difficulty = 0;
    public bool playerDead = false;
    private int _score = 0;
    public int score {
        get
        {
            return _score;
        }
        set
        {
            if (!playerDead) _score = value;
        }
    }

    private void Awake()
    {
        exclamation = _exclamation;
        self = GetComponent<GameManager>();
    }

    private void Start()
    {
        self = GetComponent<GameManager>();
        playerDead = false;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.RightShift) && Input.GetKeyDown(KeyCode.Return))
        { SceneManager.LoadScene("ProcGen"); }
    }

    private void FixedUpdate()
    {
        if (inSloMo)
        {
            Time.timeScale += 0.025f;
            if (Time.timeScale >= 1)
            {
                Time.timeScale = 1;
                inSloMo = false;
            }
        }
    }

    public static int GetEntityID()
    {
        assignID++;
        return assignID;
    }

    static bool inSloMo;
    public static void SetSloMo(float percentage)
    {
        inSloMo = true;
        Time.timeScale = percentage;
    }

    public void EndGame()
    {
        EndUI.SetActive(true);
        playerDead = true;
    }

    public void Restart()
    {
        assignID = 0;
        inSloMo = false;
        Time.timeScale = 1;
        difficulty = 0;
        score = 0;
        playerDead = false;
        SceneManager.LoadScene("ProcGen");
    }
}
