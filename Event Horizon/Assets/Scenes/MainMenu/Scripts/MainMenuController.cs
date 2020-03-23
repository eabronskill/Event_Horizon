using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public GameObject MainMenu;
    public GameObject ControlsMenu;

    public void playGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void options()
    {
        MainMenu.SetActive(false);
        ControlsMenu.SetActive(true);
    }

    public void exitGame()
    {
        Application.Quit();
    }
}