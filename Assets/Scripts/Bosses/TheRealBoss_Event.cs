using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheRealBoss_Event : MonoBehaviour
{
    [HideInInspector] public bool startEvent = false;
    private float timer = 0.0f;
    [SerializeField] private float timeToStartMusic = 5.0f;
    private bool isMusic = false;
    [SerializeField] private float timeToStartBoss = 8.0f;
    [SerializeField] private AudioSource m_audioSource = null;
    [SerializeField] private AudioClip bossEntry = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(startEvent)
        {
            if(timer >= timeToStartBoss)
            {

                timer += Time.deltaTime;
            }
            else if(timer >= timeToStartMusic && !isMusic)
            {
                m_audioSource.Stop();
                m_audioSource.PlayOneShot(bossEntry);
                isMusic = true;
                timer += Time.deltaTime;
            }
            else
            {
                timer += Time.deltaTime;
            }
        }
    }
}
