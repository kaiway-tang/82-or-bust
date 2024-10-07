using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] Transform camTrfm, systemTrfm;
    public static CameraManager self;
    // Start is called before the first frame update
    void Awake()
    {
        self = GetComponent<CameraManager>();
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftAlt))
        {
            if (Input.GetKeyDown(KeyCode.Alpha1)) { SetTrauma(10); SetDmgVig(0.1f); }
            if (Input.GetKeyDown(KeyCode.Alpha2)) { SetTrauma(20); SetDmgVig(0.2f); }
            if (Input.GetKeyDown(KeyCode.Alpha3)) { SetTrauma(40); SetDmgVig(0.4f); }
            if (Input.GetKeyDown(KeyCode.Alpha4)) { SetTrauma(80); SetDmgVig(0.8f); }
        }
    }

    bool every2;
    void FixedUpdate()
    {
        HandleTrauma();
        HandleFollowing();
        HandleFading();

        every2 = !every2;
        if (every2) { EveryTwo(); }
    }

    void EveryTwo()
    {
        //HandleTrauma();
    }

    #region MOVEMENT

    [SerializeField] Transform targetTrfm;
    [SerializeField] float followRate;
    void HandleFollowing()
    {
        systemTrfm.position += (targetTrfm.position - systemTrfm.position) * followRate;
    }

    #endregion

    #region SCREEN_SHAKE

    public static void SetTrauma(int pTrauma)
    {
        trauma = Mathf.Max(trauma, pTrauma);
    }
    public static void AddTrauma(int pTrauma)
    {
        trauma += pTrauma;
    }

    [SerializeField] float screenShakeIntensity, displacementIntensity, rotationIntensity;
    [SerializeField] int traumaStrengthCap;
    static int trauma;
    float effectiveIntensity, effectiveDisplacement, effectiveRotation;
    Vector3 displacement;
    void HandleTrauma()
    {
        HandleReset();

        if (trauma > 0)
        {
            effectiveIntensity = Mathf.Min(trauma * trauma, traumaStrengthCap * traumaStrengthCap) * screenShakeIntensity;
            effectiveDisplacement = effectiveIntensity * displacementIntensity;
            displacement = Tools.RandomOnUnitCircle() * effectiveDisplacement;
            camTrfm.position += displacement;

            effectiveRotation = effectiveIntensity * rotationIntensity;
            effectiveRotation = Random.Range(-effectiveRotation, effectiveRotation);
            camTrfm.Rotate(Vector3.forward * effectiveRotation);

            trauma--;
        }
    }

    [SerializeField] float displacementRR, rotationRR; //RR = reset rate
    [SerializeField] Transform restPos;

    void HandleReset()
    {
        camTrfm.position += (restPos.position - camTrfm.position) * displacementRR;
        Tools.LerpRotation(camTrfm, 0, rotationRR);
    }

    #endregion

    #region FADE_FX

    static float whiteFadeTarget, blackFadeTarget;
    static float whiteFadeRate, blackFadeRate;
    static bool whiteFading, blackFading, dmgVigFading, dodgeVigFading;
    public static void FadeWhite(float target, float rate = 0.02f)
    {
        whiteFadeTarget = target;
        whiteFadeRate = rate;
        whiteFading = true;
    }
    public static void FadeBlack(float target, float rate = 0.02f)
    {
        blackFadeTarget = target;
        blackFadeRate = rate;
        blackFading = true;
    }

    public static void SetDmgVig(float alpha)
    {
        Tools.SetAlpha(self.dmgVignette, alpha);
        dmgVigFading = true;
    }

    public static void SetDodgeVig(float alpha)
    {
        Tools.SetAlpha(self.dodgeVignette, alpha);
        dodgeVigFading = true;
    }

    [SerializeField] SpriteRenderer whiteCover, blackCover, dmgVignette, dodgeVignette;
    Color col;
    void HandleFading()
    {
        if (whiteFading)
        {
            if (whiteCover.color.a > whiteFadeTarget)
            {
                Tools.AddAlpha(whiteCover, -whiteFadeRate, whiteFadeTarget);
            }
            else if (whiteCover.color.a < whiteFadeTarget)
            {
                Tools.AddAlpha(whiteCover, whiteFadeRate, 0, whiteFadeTarget);
            }
            else
            {
                whiteFading = false;
            }
        }

        if (blackFading)
        {
            if (blackCover.color.a > blackFadeTarget)
            {
                Tools.AddAlpha(blackCover, -blackFadeRate, blackFadeTarget);
            }
            else if (blackCover.color.a < blackFadeTarget)
            {
                Tools.AddAlpha(blackCover, blackFadeRate, 0, blackFadeTarget);
            }
            else
            {
                blackFading = false;
            }
        }

        if (dmgVigFading)
        {
            Tools.AddAlpha(dmgVignette, -0.02f);
            if (dmgVignette.color.a <= 0)
            {
                dmgVigFading = false;
            }
        }

        if (dodgeVigFading)
        {
            Tools.AddAlpha(dodgeVignette, -0.02f);
            if (dodgeVignette.color.a <= 0)
            {
                dodgeVigFading = false;
            }
        }
    }

    public static void Restart()
    {
        whiteFading = false;
        blackFading = false;
        dmgVigFading = false;
    }

    #endregion
}
