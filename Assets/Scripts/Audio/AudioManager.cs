using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    private AudioSource m_audioSource = null;
    [SerializeField] private TheRealBoss_Event bossScript = null;

    // Start is called before the first frame update
    void Start()
    {
        m_audioSource = GetComponent<AudioSource>();
        bossScript = FindObjectOfType<TheRealBoss_Event>();
    }

    public void StartAudio()
    {
        m_audioSource.Play();
    }

    public void StopAudio()
    {
        m_audioSource.Stop();
    }

    public void StartTheRealBoss()
    {
        bossScript.startEvent = true;
    }

}
