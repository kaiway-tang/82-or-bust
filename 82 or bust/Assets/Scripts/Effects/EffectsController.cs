using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsController : MonoBehaviour
{
    [SerializeField] Transform laser1;
    [SerializeField] Transform laser2;
    
    // Start is called before the first frame update
    void Start()
    {
        SetLasers(Vector2.up, 2f, 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetLasers(Vector2 convergeDir, float convergeDuration, float holdDuration = 0f)
    {
        StartCoroutine(ConvergeLasers(convergeDir, convergeDuration, holdDuration));
    }

    IEnumerator ConvergeLasers(Vector2 dir, float duration, float holdDuration)
    {
        laser1.gameObject.SetActive(true);
        laser2.gameObject.SetActive(true);
        Vector2 perp = Vector2.Perpendicular(dir.normalized);
        Vector2 laser1Dir = (dir + perp).normalized;
        Vector2 laser2Dir = (dir - perp).normalized;

        float elapsed = 0;

        while (elapsed < duration)
        {
            laser1.up = Vector2.Lerp(laser1Dir, dir, elapsed / duration);
            laser2.up = Vector2.Lerp(laser2Dir, dir, elapsed / duration);
            //laser1.up = laser1Dir;
            //laser2.up = laser2Dir;
            yield return new WaitForFixedUpdate();
            elapsed += Time.fixedDeltaTime;
        }
        yield return new WaitForSeconds(holdDuration);
        laser1.gameObject.SetActive(false);
        laser2.gameObject.SetActive(false);
    }
}
