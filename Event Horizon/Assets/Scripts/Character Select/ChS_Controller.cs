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

    public int connectedControllers;
    
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
        //playerIDToPlayer.Add(1, ReInput.players.GetPlayer(0));
        //playerIDToPlayer.Add(2, ReInput.players.GetPlayer(1));
        //playerIDToPlayer.Add(3, ReInput.players.GetPlayer(2));
        //playerIDToPlayer.Add(4, ReInput.players.GetPlayer(3));
        foreach (Joystick cont in ReInput.controllers.Joysticks)
        {
            print("Controller (" + cont.id + ") found.");
            playerIDToPlayer.Add(cont.id, ReInput.players.GetPlayer(cont.id));
            


        }
        /*foreach (int id in MainMenuController.controllerIDToPlayerID.Keys)
        {
            playerIDToPlayer.Add(id, ReInput.players.GetPlayer(id));
            
            if (playerIDToPlayer[id].controllers.joystickCount != 1)
            {
                // Get the Joystick from ReInput
                Joystick joystick = ReInput.controllers.GetJoystick(0);

                // Assign Joystick to first Player that doesn't have any assigned
                //ReInput.controllers.AutoAssignJoystick(joystick);
                playerIDToPlayer[id].controllers.AddController(joystick, false);
            }
            print("JCount: " + playerIDToPlayer[id].controllers.joystickCount);
            print("ID: " + id);
        }*/
        //playerIDToPlayer = ReInput.players.GetPlayer(MainMenuController.controllerIDToPlayerID);

        // Subscribe to events
        ReInput.ControllerConnectedEvent += OnControllerConnected;
        ReInput.ControllerDisconnectedEvent += OnControllerDisconnected;
        ReInput.ControllerPreDisconnectEvent += OnControllerPreDisconnect;
    }

    void Update()
    {
        print("Connected Controllers: " + connectedControllers);
        print("PID to Player Count: " + playerIDToPlayer.Count);
        foreach (int id in playerIDToPlayer.Keys)
        {
            print("contains:" + id);
        }

        //Player 1 Logic
        if (playerIDToPlayer.ContainsKey(0) && playerIDToPlayer[0].controllers.Joysticks.Count > 0)
        {
            print("Player1");
            getInput(0);
        }
        //Player 2 Logic
        if (playerIDToPlayer.ContainsKey(1) && playerIDToPlayer[1].controllers.Joysticks.Count > 0)
        {
            print("Player2");
            getInput(1);
        }
        //Player 3 Logic
        if (playerIDToPlayer.ContainsKey(2) && playerIDToPlayer[2].controllers.Joysticks.Count > 0)
        {
            print("Player3");
            getInput(2);
        }
        //Player 4 Logic
        if (playerIDToPlayer.ContainsKey(3)) //&& playerIDToPlayer[3].controllers.Joysticks.Count > 0)
        {
            print("Player4");
            getInput(3);
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
            print("Select" + playerID);
            selectButtonClick(playerID+1);
        }
        if (playerIDToPlayer[playerID].GetButtonDown("Up"))
        {
            print("Up" + playerID);
            upButtonClick(playerID+1);
        }
        if (playerIDToPlayer[playerID].GetButtonDown("Down"))
        {
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

        // TODO: Load the first level.
        SceneManager.LoadScene("Level1");
        
    }

}
