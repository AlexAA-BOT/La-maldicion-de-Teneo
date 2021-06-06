using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Credits_Script : MonoBehaviour
{
    [SerializeField] GameObject credits = null;
    [SerializeField] GameObject music = null;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeToMusic()
    {
        credits.SetActive(false);
        music.SetActive(true);
    }

    public void ChangeToCredits()
    {
        music.SetActive(false);
        credits.SetActive(true);
    }

}
