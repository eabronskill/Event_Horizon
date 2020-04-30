using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Rewired;


public class PauseMenuController : MonoBehaviour
{
    public GameObject ControlsMenu, selectButton, controlsButton, quitButton;
    public static Rewired.Player player1;

    private GameObject[] buttons = new GameObject[3];
    private int iter = 0;

    public static bool active = false;

    // Start is called before the first frame update
    void Start()
    {
        buttons[0] = (selectButton);
        buttons[1] = (controlsButton);
        buttons[2] = (quitButton);
        buttons[iter].GetComponent<Image>().color = buttons[iter].GetComponent<Button>().colors.highlightedColor;
    }

    // Update is called once per frame
    void Update()
    {
        if (this.gameObject.activeSelf)
        {
            Time.timeScale = 0;
        }
        
        if (player1.controllers.Joysticks.Count > 0)
        {
            if (player1.GetButtonDown("Interact"))
            {
                if (iter == 0)
                {
                    resumeGame();
                }
                else if (iter == 1)
                {
                    controls();
                }
                else
                {
                    exitGame();
                }
            }
            if (player1.GetButtonDown("Up"))
            {
                buttons[iter].GetComponent<Image>().color = buttons[iter].GetComponent<Button>().colors.normalColor;
                if (iter == 0)
                {
                    iter = 2;
                }
                else
                {
                    iter--;
                }
                buttons[iter].GetComponent<Image>().color = buttons[iter].GetComponent<Button>().colors.highlightedColor;
            }
            if (player1.GetButtonDown("Down"))
            {
                buttons[iter].GetComponent<Image>().color = buttons[iter].GetComponent<Button>().colors.normalColor;

                if (iter == 2)
                {
                    iter = 0;
                }
                else
                {
                    iter++;
                }
                buttons[iter].GetComponent<Image>().color = buttons[iter].GetComponent<Button>().colors.highlightedColor;
            }
            if (ControlsMenu.activeSelf && player1.GetButtonDown("Back"))
            {
                ControlsMenu.SetActive(false);
            }
            else if (player1.GetButtonDown("Back"))
            {
                Time.timeScale = 1;
                this.gameObject.SetActive(false);
            }
        }
    }

    private void resumeGame()
    {
        Time.timeScale = 1f;
        this.gameObject.SetActive(false);
        active = false;
    }

    private void controls()
    {
        ControlsMenu.SetActive(true);
    }

    private void exitGame()
    {
        MainMenuController.controllerIDToPlayerID = new System.Collections.Generic.Dictionary<int, int>();
        ChS_Model.idToCharacter = new System.Collections.Generic.Dictionary<int, ChS_Model.Character>();
        ChS_Controller.finalSelection = new System.Collections.Generic.Dictionary<string, int>();
        UIEventCOntroller.players = new System.Collections.Generic.Dictionary<string, GameObject>();
        SceneManager.LoadScene("MainMenu");
    }

    public Rewired.Player getPlayer()
    {
        return player1;
    }

    public void setPlayer(Rewired.Player player)
    {
        player1 = player;
    }
}
