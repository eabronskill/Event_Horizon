using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Rewired;

/// <summary>
/// Handles the input from the players in the Character Select screen.
/// </summary>
public class ChS_Controller : MonoBehaviour
{
    public GameObject player1Hover, player2Hover, player3Hover, player4Hover, tankIcon, soldierIcon, 
        rogueIcon, engineerIcon, selectButton1, selectButton2, selectButton3, selectButton4, upButton1
        , upButton2, upButton3, upButton4, downButton1, downButton2, downButton3, downButton4, playButton;

    /// <summary>
    /// Maps the player number to the Rewired Player.
    /// </summary>
    private Dictionary<int, Rewired.Player> playerIDToPlayer = new Dictionary<int, Rewired.Player>();

    /// <summary>
    /// Will be used to map the players to the characters in the actual levels.
    /// </summary>
    public static Dictionary<string, int> finalSelection = new Dictionary<string, int>();

    private ChS_Model model;
    private ChS_View view;

    private bool loadedNextScene = false;
    
    void Awake()
    {
        // Inilialize model and view.
        model = new ChS_Model
        {
            tankIcon = tankIcon,
            soldierIcon = soldierIcon,
            rogueIcon = rogueIcon,
            engineerIcon = engineerIcon
        };
        model.intitialize();
        view = new ChS_View()
        {
            player1Hover = player1Hover,
            player2Hover = player2Hover,
            player3Hover = player3Hover,
            player4Hover = player4Hover,
            tankIcon = tankIcon,
            soldierIcon = soldierIcon,
            rogueIcon = rogueIcon,
            engineerIcon = engineerIcon,
            selectButton1 = selectButton1,
            selectButton2 = selectButton2,
            selectButton3 = selectButton3,
            selectButton4 = selectButton4,
            upButton1 = upButton1,
            upButton2 = upButton2,
            upButton3 = upButton3,
            upButton4 = upButton4,
            downButton1 = downButton1,
            downButton2 = downButton2,
            downButton3 = downButton3,
            downButton4 = downButton4
        };
        view.initialize();
    }

    void Start()
    {
        playerIDToPlayer.Add(1, ReInput.players.GetPlayer(0));
        playerIDToPlayer.Add(2, ReInput.players.GetPlayer(1));
        playerIDToPlayer.Add(3, ReInput.players.GetPlayer(2));
        playerIDToPlayer.Add(4, ReInput.players.GetPlayer(3));
    }

    void Update()
    {
        if (!loadedNextScene)
        {
            //Player 1 Logic
            if (playerIDToPlayer[1].controllers.Joysticks.Count > 0)
            {
                getInput(1);
            }
            //Player 2 Logic
            if (playerIDToPlayer[2].controllers.Joysticks.Count > 0)
            {
                getInput(2);
            }
            //Player 3 Logic
            if (playerIDToPlayer[3].controllers.Joysticks.Count > 0)
            {
                getInput(3);
            }
            //Player 4 Logic
            if (playerIDToPlayer[4].controllers.Joysticks.Count > 0)
            {
                getInput(4);
            }
            
        }
    }

    /// <summary>
    /// Calls the methods that correspond to the input from the Player with the input playerID.
    /// </summary>
    /// <param name="playerID"></param>
    private void getInput(int playerID)
    {
        if (playerIDToPlayer[playerID].GetButtonDown("Select"))
        {
            print("Select" + playerID);
            selectButtonClick(playerID);
        }
        if (playerIDToPlayer[playerID].GetButtonDown("Up"))
        {
            print("Up" + playerID);
            upButtonClick(playerID);
        }
        if (playerIDToPlayer[playerID].GetButtonDown("Down"))
        {
            print("Down" + playerID);
            downButtonClick(playerID);
        }
        if (playerIDToPlayer[playerID].GetButtonDown("Play"))
        {
            print("Play" + playerID);
            playButtonClick();
        }
    }

    /// <summary>
    /// Called when a Select button is clicked.
    /// </summary>
    /// <param name="playerID"></param>
    /// <param name="classID"></param>
    public void selectButtonClick(int playerID)
    {
        // If button has already been selected, call the unselect logic.
        if (view.playerToSelectBtn[playerID].GetComponentInChildren<Text>().GetComponent<Text>().text == "Unselect")
        {
            model.unselectCharacter(playerID);
            view.characterUnselected(playerID);
        }
        else
        {
            if (model.selectCharacter(playerID))
            {
                view.characterSelected(playerID);
            }
        }
    }

    /// <summary>
    /// Called when a Up button is clicked.
    /// </summary>
    /// <param name="playerID"></param>
    public void upButtonClick(int playerID)
    {
        view.nextCharacter(playerID, model.previousCharacter(playerID));
    }

    /// <summary>
    /// Called when a Down button is clicked.
    /// </summary>
    /// <param name="playerID"></param>
    public void downButtonClick(int playerID)
    {
        view.nextCharacter(playerID, model.nextCharacter(playerID));
    }

    /// <summary>
    /// Called when the Play button is clicked.
    /// </summary>
    public void playButtonClick()
    {
        // Check to see if all the characters have been selected. If one has not been selected, then don't load the level.
        foreach (int cID in model.getCIDtoC().Keys)
        {
            if (model.getCIDtoC()[cID].selected == false)
            {
                return;
            }
        }

        // Finalize the selections
        for (int i = 1; i <= 4; i++)
        {
            finalSelection.Add(model.getCIDtoC()[model.getPIDtoCID()[i]].characterIcon.name, i - 1);
        }

        loadedNextScene = true;

        // TODO: Load the first level.
        SceneManager.LoadScene("Elliot's");
        
    }

}
