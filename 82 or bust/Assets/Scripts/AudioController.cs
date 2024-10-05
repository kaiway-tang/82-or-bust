using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioController : MonoBehaviour
{
    // Reference to the AudioMixer
    public AudioMixer audioMixer;
    [SerializeField] AudioSource sfx;
    [SerializeField] float normalFrequency = 22000f;
    [SerializeField] float lowpassFrequency = 1500f;
    [SerializeField] float lowpassDuration = 1.0f;
    [SerializeField] float recoveryFrequency = 300f;

    // The name of the parameter in the AudioMixer that controls the low pass filter's cutoff frequency
    public string lowPassParamName = "LowpassFreq";

    // Desired cutoff frequency (10,000 Hz)
    private float cutoffFrequency = 10000f;

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

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            sfx.Play();
            StopAllCoroutines();
            StartCoroutine(LowpassFadeIn());
        }
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

    
}
