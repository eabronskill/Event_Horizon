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
        , upButton2, upButton3, upButton4, downButton1, downButton2, downButton3, downButton4, playButton, group1, group2, group3, group4;

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

    public AudioSource buttonClickSound;

    private int connectedControllers;
    private bool canPlay;

    public GameObject readytoPlay;
    public GameObject waitingforPlayers;

    void Awake()
    {
        // Inilialize model and view.
        model = new ChS_Model()
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

        // Subscribe to events
        ReInput.ControllerConnectedEvent += OnControllerConnected;
        ReInput.ControllerDisconnectedEvent += OnControllerDisconnected;
        ReInput.ControllerPreDisconnectEvent += OnControllerPreDisconnect;
    }

    void Start()
    {
        // Grab all the joysticks and assign players to them.
        foreach (Joystick cont in ReInput.controllers.Joysticks)
        {
            print("Controller (" + cont.id + ") found.");
            playerIDToPlayer.Add(cont.id, ReInput.players.GetPlayer(cont.id));
        }
        view.toggleGroupOn(group1);
        view.toggleGroupOn(group2);
        view.toggleGroupOn(group3);
        view.toggleGroupOn(group4);
        // Subscribe to events
        ReInput.ControllerConnectedEvent += OnControllerConnected;
        ReInput.ControllerDisconnectedEvent += OnControllerDisconnected;
        ReInput.ControllerPreDisconnectEvent += OnControllerPreDisconnect;
        playButton.GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
        readytoPlay.SetActive(false);
        waitingforPlayers.SetActive(true);
    }

    void Update()
    {
        print(connectedControllers);

        //Player 1 Logic
        if (playerIDToPlayer.ContainsKey(0) && playerIDToPlayer[0].controllers.Joysticks.Count > 0)
        {
            view.toggleGroupOn(group1);
            print("Player1");
            getInput(0);
        }
        else
        {
            view.toggleGroupOff(group1);
        }
        //Player 2 Logic
        if (playerIDToPlayer.ContainsKey(1) && playerIDToPlayer[1].controllers.Joysticks.Count > 0)
        {
            view.toggleGroupOn(group2);
            print("Player2");
            getInput(1);
        }
        else
        {
            view.toggleGroupOff(group2);
        }
        //Player 3 Logic
        if (playerIDToPlayer.ContainsKey(2) && playerIDToPlayer[2].controllers.Joysticks.Count > 0)
        {
            view.toggleGroupOn(group3); 
            print("Player3");
            getInput(2);
        }
        else
        {
            view.toggleGroupOff(group3);
        }
        //Player 4 Logic
        if (playerIDToPlayer.ContainsKey(3) && playerIDToPlayer[3].controllers.Joysticks.Count > 0)
        {
            view.toggleGroupOn(group4);
            print("Player4");
            getInput(3);
        }
        else
        {
            view.toggleGroupOff(group4);
        }

        int numNeeded = 0;
        // Check to see if all the characters have been selected. If one has not been selected, then don't load the level.
        foreach (int cID in model.getCIDtoC().Keys)
        {
            if (model.getCIDtoC()[cID].selected)
            {
                numNeeded++;
            }
        }
        print("NumNeeded: " + numNeeded);
        if (numNeeded == connectedControllers && connectedControllers != 0)
        {
            canPlay = true;
            playButton.GetComponent<Image>().color = new Color(1, 1, 1, 1f);
            readytoPlay.SetActive(true);
            waitingforPlayers.SetActive(false);
        }
        else
        {
            canPlay = false;
            playButton.GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
            readytoPlay.SetActive(false);
            waitingforPlayers.SetActive(true);
        }
    }

    void LateUpdate()
    {
        connectedControllers = ReInput.controllers.Joysticks.Count;
    }

    // This function will be called when a controller is connected
    // You can get information about the controller that was connected via the args parameter
    void OnControllerConnected(ControllerStatusChangedEventArgs args)
    {
        Debug.Log("A controller was connected! Name = " + args.name + " Id = " + args.controllerId + " Type = " + args.controllerType);

        if (args.controllerType == ControllerType.Joystick)
        {
            playerIDToPlayer.Add(args.controllerId, ReInput.players.GetPlayer(args.controllerId));
        }
    }
    
    // This function will be called when a controller is fully disconnected
    // You can get information about the controller that was disconnected via the args parameter
    void OnControllerDisconnected(ControllerStatusChangedEventArgs args)
    {
        Debug.Log("A controller was disconnected! Name = " + args.name + " Id = " + args.controllerId + " Type = " + args.controllerType);
        if (args.controllerType == ControllerType.Joystick)
        {
            playerIDToPlayer.Remove(args.controllerId);
        }
    }

    // This function will be called when a controller is about to be disconnected
    // You can get information about the controller that is being disconnected via the args parameter
    // You can use this event to save the controller's maps before it's disconnected
    void OnControllerPreDisconnect(ControllerStatusChangedEventArgs args)
    {
        Debug.Log("A controller is being disconnected! Name = " + args.name + " Id = " + args.controllerId + " Type = " + args.controllerType);
    }

    /// <summary>
    /// Calls the methods that correspond to the input from the Player with the input playerID.
    /// </summary>
    /// <param name="playerID"></param>
    private void getInput(int playerID)
    {
        if (playerIDToPlayer[playerID].GetButtonDown("Select"))
        {
            buttonClickSound.pitch = 1;
            buttonClickSound.Play();
            print("Select" + playerID);
            selectButtonClick(playerID+1);
        }
        if (playerIDToPlayer[playerID].GetButtonDown("Up"))
        {
            buttonClickSound.pitch = 2;
            buttonClickSound.Play();
            print("Up" + playerID);
            upButtonClick(playerID+1);
        }
        if (playerIDToPlayer[playerID].GetButtonDown("Down"))
        {
            buttonClickSound.pitch = 2;
            buttonClickSound.Play();
            print("Down" + playerID);
            downButtonClick(playerID+1);
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
        if (!canPlay)
        {
            return;
        }

        // Finalize the selections
        for (int i = 1; i <= 4; i++)
        {
            print("i: " + i);
            if (model.getCIDtoC()[model.getPIDtoCID()[i]].selected && (model.getCIDtoC()[model.getPIDtoCID()[i]].playerID == i))//!finalSelection.ContainsKey(model.getCIDtoC()[model.getPIDtoCID()[i]].characterIcon.name))
            {
                finalSelection.Add(model.getCIDtoC()[model.getPIDtoCID()[i]].characterIcon.name, model.getCIDtoC()[model.getPIDtoCID()[i]].playerID -1);
            }
        }
        buttonClickSound.pitch = 1;
        buttonClickSound.Play();
        // TODO: Load the first level.
        SceneManager.LoadScene("Level2");
        
    }

}
