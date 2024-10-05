using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsController : MonoBehaviour
{
    [Header("Lasers")]
    [SerializeField] Transform laser1;
    [SerializeField] Transform laser2;

    [Header("Crosshair")]
    [SerializeField] Transform crosshairParent;
    [SerializeField] Transform crosshair;
    [SerializeField] Transform crossUp;
    [SerializeField] Transform crossDown;
    [SerializeField] Transform crossLeft;
    [SerializeField] Transform crossRight; 
    
    // Start is called before the first frame update
    void Start()
    {
        // SetLasers(Vector2.up, 2f, 0.5f);
        SpinCrosshair(1f, 0.5f);
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

    public void SpinCrosshair(float duration, float holdDuration = 0f)
    {
        StartCoroutine(CrosshairLock(duration, holdDuration));
    }

    IEnumerator CrosshairLock(float duration, float holdDuration)
    {
        crosshairParent.gameObject.SetActive(true);
        crosshair.right = transform.right; 
        // Rotate once, the cross converges in the middle
        float rotDelta = 360f / duration * Time.fixedDeltaTime;
        float crosshairDist = crossUp.localPosition.y;
        float elapsed = 0;
        while (elapsed < duration)
        {
            crosshair.Rotate(Vector3.forward * rotDelta);
            float curDist = Mathf.Lerp(crosshairDist, 0.1f, elapsed / duration);
            crossUp.position = new Vector3(transform.position.x, transform.position.y + curDist);
            crossDown.position = new Vector3(transform.position.x, transform.position.y - curDist);
            crossLeft.position = new Vector3(transform.position.x - curDist, transform.position.y);
            crossRight.position = new Vector3(transform.position.x + curDist, transform.position.y); 
            yield return new WaitForFixedUpdate();
            elapsed += Time.fixedDeltaTime;
        }

        yield return new WaitForSeconds(holdDuration);

        crossUp.position = new Vector3(transform.position.x, transform.position.y + crosshairDist);
        crossDown.position = new Vector3(transform.position.x, transform.position.y - crosshairDist);
        crossLeft.position = new Vector3(transform.position.x - crosshairDist, transform.position.y);
        crossRight.position = new Vector3(transform.position.x + crosshairDist, transform.position.y);
        crosshairParent.gameObject.SetActive(false); 
    }
}
