using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu_Bestiario : MonoBehaviour
{
    [SerializeField] private GameObject[] pages = null;
    [SerializeField] private GameObject[] buttons = null;

    private int numPage = 0;

    // Start is called before the first frame update
    void Start()
    {
        buttons[0].SetActive(false);
        foreach (GameObject page in pages)
        {
            if (page == pages[numPage])
            {
                page.SetActive(true);
            }
            else
            {
                page.SetActive(false);
            }
        }
    }

    public void pagesMore()
    {
        numPage++;
        ChangePage();
    }

    public void pagesLess()
    {
        numPage--;
        ChangePage();
    }

    private void ChangePage()
    {
        foreach(GameObject page in pages)
        {
            if(page == pages[numPage])
            {
                page.SetActive(true);
            }
            else
            {
                page.SetActive(false);
            }
        }

        if(numPage == 0)
        {
            buttons[0].SetActive(false);
            buttons[1].SetActive(true);
        }
        else if(numPage == pages.Length - 1)
        {
            buttons[0].SetActive(true);
            buttons[1].SetActive(false);
        }
        else
        {
            foreach(GameObject buton in buttons)
            {
                buton.SetActive(true);
            }
        }

    }

}
