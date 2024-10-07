using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreUI : MonoBehaviour
{
    TMP_Text scoreUI;
    // Start is called before the first frame update
    void Start()
    {
        scoreUI = GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        scoreUI.text = GameManager.self.score.ToString();
    }
}
