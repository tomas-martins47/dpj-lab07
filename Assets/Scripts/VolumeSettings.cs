using UnityEngine.Audio;
using UnityEngine;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] private AudioMixer mixer;

    // Mathf.Log10() converts the slider value (0.0001 to 1) into decibels (dB).
    // The minimum value cannot be 0 because Log10(0) is undefined.
    public void SetMusicVolume(float value)
    {
        mixer.SetFloat("MusicVolume", Mathf.Log10(value) * 20f);
    }

    public void SetSfxVolume(float value)
    {
        mixer.SetFloat("SFXVolume", Mathf.Log10(value) * 20f);
    }
}
