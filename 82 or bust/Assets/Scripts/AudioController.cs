using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioController : MonoBehaviour
{
    [SerializeField] List<AudioClip> audioClips;
    // Reference to the AudioMixer
    public AudioMixer audioMixer;
    [SerializeField] AudioSource sfx;
    [SerializeField] AudioSource bgm1;
    [SerializeField] AudioSource bgm2;
    [SerializeField] AudioSource restbgm;
    [SerializeField] float normalFrequency = 22000f;
    [SerializeField] float lowpassFrequency = 1500f;
    [SerializeField] float lowpassDuration = 1.0f;
    [SerializeField] float recoveryFrequency = 300f;
    [SerializeField] [Range(0f, 1f)] float blend = 0.5f;

    float fightBGMVolume = 1f; 

    // The name of the parameter in the AudioMixer that controls the low pass filter's cutoff frequency
    public string lowPassParamName = "LowpassFreq";

    public static AudioController Instance;

    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
        }
        Instance = this;
    }

    void Start()
    {
        // Check if the AudioMixer is assigned
        if (audioMixer != null)
        {
            // Modify the low pass cutoff frequency to 10,000 Hz
            // SetLowPassCutoffFrequency(cutoffFrequency);
        }
        else
        {
            Debug.LogError("AudioMixer is not assigned.");
        }

        bgm1.Play();
        bgm2.Play();
    }

    // Function to set the low pass filter's cutoff frequency
    public void SetLowPassCutoffFrequency(float frequency)
    {
        if (audioMixer != null)
        {
            // Set the cutoff frequency parameter in the AudioMixer
            audioMixer.SetFloat(lowPassParamName, frequency);
            // Debug.Log($"Low pass cutoff frequency set to {frequency} Hz.");
        }
    }

    public void PlaySlashSound()
    {
        sfx.PlayOneShot(audioClips[0]);
    }

    public void PlayDashSound()
    {
        sfx.PlayOneShot(audioClips[1]);
    }

    public void PlayPDodgeSound()
    {
        sfx.PlayOneShot(audioClips[2]);
    }

    private void Update()
    {
        //if (Input.GetMouseButtonDown(0))
        //{
        //    sfx.Play();
        //    StopAllCoroutines();
        //    StartCoroutine(LowpassFadeIn());
        //}

        bgm1.volume = blend * fightBGMVolume;
        bgm2.volume = (1 - blend) * fightBGMVolume;
        restbgm.volume = 1 - fightBGMVolume; 
    }

    public void PlayLowpassOneShot()
    {
        StartCoroutine(LowpassFadeIn());
    }

    IEnumerator LowpassFadeIn()
    {
        float freq = lowpassFrequency;
        SetLowPassCutoffFrequency(freq);
        yield return new WaitForSeconds(lowpassDuration);
        while (freq < normalFrequency)
        {
            yield return new WaitForFixedUpdate();
            SetLowPassCutoffFrequency(freq);
            freq += recoveryFrequency;
        }
        SetLowPassCutoffFrequency(normalFrequency);
    }

    public void FadeRestMusic(bool fadeIn)
    {
        StartCoroutine(BlendRestMusic(fadeIn));
    }

    IEnumerator BlendRestMusic(bool fadeIn, float rate = 1)
    {
        if (fadeIn)
        {
            restbgm.Play();
            while (fightBGMVolume > 0)
            {
                fightBGMVolume -= Time.fixedDeltaTime * rate;
                yield return new WaitForFixedUpdate();
            }
            fightBGMVolume = 0;
        } else
        {
            bgm1.Play();
            bgm2.Play();
            while (fightBGMVolume < 1)
            {
                fightBGMVolume += Time.fixedDeltaTime * rate;
                yield return new WaitForFixedUpdate();
            }
            fightBGMVolume = 1;
        }
    }
}
