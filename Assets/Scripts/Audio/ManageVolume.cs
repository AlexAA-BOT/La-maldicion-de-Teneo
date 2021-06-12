using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class ManageVolume : MonoBehaviour
{
    [SerializeField] private AudioMixer masterVolume = null;
    private float value = 0.0f;
    [SerializeField] private Slider[] sliders = null;

    // Start is called before the first frame update
    void Start()
    {
        masterVolume.GetFloat("MasterVolume", out value);
        SetMasterVolume(value);
        sliders[0].value = value;
        masterVolume.GetFloat("MusicVolume", out value);
        SetMusicVolume(value);
        sliders[1].value = value;
        masterVolume.GetFloat("SoundEffectVolume", out value);
        SetSoundEffectVolume(value);
        sliders[2].value = value;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetMasterVolume(float sliderValue)
    {
        masterVolume.SetFloat("MasterVolume", sliderValue);
    }

    public void SetMusicVolume(float sliderValue)
    {
        masterVolume.SetFloat("MusicVolume", sliderValue);
    }

    public void SetSoundEffectVolume(float sliderValue)
    {
        masterVolume.SetFloat("SoundEffectVolume", sliderValue);
    }
}
