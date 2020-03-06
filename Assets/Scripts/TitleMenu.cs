using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleMenu : MonoBehaviour
{
    public GameObject panel;

    private void Start()
    {
        if (panel != null)
            panel.SetActive(false);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }
    
    public void ToggleCredits()
    {
        if (panel != null)
            panel.SetActive(!panel.activeInHierarchy);
    }

    private void HideCredits()
    {
        if (panel != null)
            panel.SetActive(false);
    }
}
