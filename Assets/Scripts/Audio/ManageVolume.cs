using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class ManageVolume : MonoBehaviour
{
    [SerializeField] private AudioMixer masterVolume = null;

    // Start is called before the first frame update
    void Start()
    {
        
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
