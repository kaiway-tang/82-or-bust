using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public GameObject grid;
    public GameObject[] chunks;
    public static List<InactiveShell> shells;

    public static LevelManager self;
    public Transform defaultNanobotPos;
    // Start is called before the first frame update
    void Awake()
    {
        self = GetComponent<LevelManager>();
        shells = new List<InactiveShell>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NewShell(InactiveShell shell)
    {
        shells.Add(shell);
    }

    float ShellScore(Vector3 pos, InactiveShell shell)
    {
        return Tools.BoxDist(shell.trfm.position, pos) + (5 - shell.count) * 10;
    }

    public static InactiveShell GetClosestShell(Vector2 pos)
    {
        if (shells.Count < 1) { return null; }

        if (!shells[0]) { shells.RemoveAt(0); }

        int index = 0;
        float distance = self.ShellScore(pos, shells[0]);
        float curDist = 0;

        for (int i = 1; i < shells.Count; i++)
        {
            if (!shells[i]) { shells.RemoveAt(i); continue; }
            curDist = self.ShellScore(pos, shells[i]);
            if (curDist < distance)
            {
                index = i;
                distance = curDist;
            }
        }

        return shells[index];
    }
}
