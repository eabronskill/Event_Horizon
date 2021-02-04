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
        if (CharacterSelectController._singlePlayer) return;
        buttons[iter].GetComponent<Image>().color = buttons[iter].GetComponent<Button>().colors.highlightedColor;
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameObject.activeSelf) return;
        else Time.timeScale = 0;

        print(ControlsMenu.activeSelf);

        //if (ControlsMenu.activeSelf && (player1.GetButtonDown("Back") || Input.GetKeyDown(KeyCode.Escape)))
        //{
        //    print("A");
        //    ControlsMenu.SetActive(false);
        //}
        //else if (player1.GetButtonDown("Back") || Input.GetKeyDown(KeyCode.Escape))
        //{
        //    print("B");
        //    Time.timeScale = 1;
        //    ControlsMenu.SetActive(false);
        //    gameObject.SetActive(false);
        //    return;
        //}

        if (CharacterSelectController._singlePlayer) return;

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
    }

    public void resumeGame()
    {
        Time.timeScale = 1f;
        this.gameObject.SetActive(false);
        active = false;
    }

    public void controls()
    {
        ControlsMenu.SetActive(true);
    }

    public void exitGame()
    {
        MainMenuController._controllerIDToPlayerID = new System.Collections.Generic.Dictionary<int, int>();
        CharacterSelectModel._idToCharacter = new System.Collections.Generic.Dictionary<int, CharacterSelectModel.Character>();
        CharacterSelectController._finalSelection = new System.Collections.Generic.Dictionary<string, int>();
        CharacterSelectController._singlePlayer = false;
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
