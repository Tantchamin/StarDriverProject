using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuButtonScript : MonoBehaviour
{
    public GameObject tutorialPanel, mainMenuPanel, playMenuPanel;

    // Start is called before the first frame update
    void Start()
    {
        tutorialPanel.SetActive(false);
        playMenuPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenPlayMenu()
    {
        playMenuPanel.SetActive(true);
    }

    public void ClosePlayMenu()
    {
        playMenuPanel.SetActive(false);
    }

    public void OpenTutorial()
    {
        tutorialPanel.SetActive(true);
    }

    public void CloseTutorial()
    {
        tutorialPanel.SetActive(false);
    }


    public void ExitButton()
    {
        Application.Quit();
    }
}
