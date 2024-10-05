using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PDodgeRingFX : MonoBehaviour
{
    [SerializeField] SpriteRenderer rend;
    [SerializeField] Sprite[] sprites;
    [SerializeField] float rate;

    int curSprite;
    float time;
    void Update()
    {
        time += Time.deltaTime;
        if (time >= rate)
        {
            if (curSprite >= sprites.Length)
            {
                Destroy(gameObject);
                return;
            }

            time -= rate;
            rend.sprite = sprites[curSprite];
            curSprite++;            
        }
    }
}
