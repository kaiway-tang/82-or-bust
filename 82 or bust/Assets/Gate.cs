using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    [SerializeField] Transform lGate, rGate;
    [SerializeField] int animationTicks;
    [SerializeField] float moveRate;
    int state = 0;
    const int CLOSED = 0, OPEN = 1, CLOSING = 2, OPENING = 3;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (state == OPEN) { Close(); }
            else if (state == CLOSED) { Open(); }
        }
    }

    int animationTmr;
    void FixedUpdate()
    {
        if (animationTmr > 0)
        {
            animationTmr--;
            if (state == OPENING)
            {
                lGate.position += Vector3.right * -moveRate;
                rGate.position += Vector3.right * moveRate;
                if (animationTmr < 1) { state = OPEN; }
            }
            if (state == CLOSING)
            {
                lGate.position += Vector3.right * moveRate;
                rGate.position += Vector3.right * -moveRate;
                if (animationTmr < 1) { state = CLOSED; }
            }
        }

        if (state == OPEN && Player.self.trfm.position.y > transform.position.y)
        {
            LevelGenerator.Instance.GenerateLevel(3);
            Close();
        }
    }

    public void Open()
    {
        if (state == OPEN || state == OPENING) { return; }
        state = OPENING;
        animationTmr = animationTicks;
    }
    public void Close()
    {
        if (state == CLOSED || state == CLOSING) { return; }
        state = CLOSING;
        animationTmr = animationTicks;
    }
}
