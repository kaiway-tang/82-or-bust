using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Gate : MonoBehaviour
{
    [SerializeField] Key nextKey;
    [SerializeField] Transform lGate, rGate;
    [SerializeField] int animationTicks;
    [SerializeField] float moveRate;
    int state = 0;
    const int CLOSED = 0, OPEN = 1, CLOSING = 2, OPENING = 3;
    public Transform trfm;
    [SerializeField] bool isTutorialGate;
    // Start is called before the first frame update
    void Awake()
    {
        trfm = transform;
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

        if (state == OPEN && Player.self.trfm.position.y > trfm.position.y + 1)
        {
            Player.self.AddYVelocity(999, 20);
            if (isTutorialGate)
            {
                CameraManager.FadeBlack(1);
                Invoke("LoadProcGen", 1.5f);
            }
            else
            {
                LevelGenerator.Instance.GenerateLevel(3);
            }
            AudioController.Instance.SetLowPassCutoffFrequency(3000f);
            GameManager.self.score += 1000;
            Close();
        }
    }

    void LoadProcGen()
    {
        SceneManager.LoadScene("ProcGen");
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
