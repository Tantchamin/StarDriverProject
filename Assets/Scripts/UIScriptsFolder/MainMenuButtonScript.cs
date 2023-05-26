using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuButtonScript : MonoBehaviour
{
    public GameObject tutorialPanel, mainMenuPanel, playMenuPanel, loginPanel, quickMatchPanel, matchTypePanel;

    // Start is called before the first frame update
    void Start()
    {
        tutorialPanel.SetActive(false);
        playMenuPanel.SetActive(false);
        loginPanel.SetActive(false);
        quickMatchPanel.SetActive(false);
        matchTypePanel.SetActive(false);
    }

    public void OpenPlayMenu()
    {
        playMenuPanel.SetActive(true);
    }

    public void QuickMatch()
    {
        quickMatchPanel.SetActive(true);
    }

    public void LobbyMatch()
    {
        loginPanel.SetActive(true);
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

    public void OpenThisGameObject(GameObject gameObject)
    {
        gameObject.SetActive(true);
    }

    public void CloseThisGameObject(GameObject gameObject)
    {
        gameObject.SetActive(false);
    }
}
