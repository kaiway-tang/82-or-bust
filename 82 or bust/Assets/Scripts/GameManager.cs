using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public void Restart()
    {
        assignID = 0;
        inSloMo = false;
        Time.timeScale = 1;
    }
}
