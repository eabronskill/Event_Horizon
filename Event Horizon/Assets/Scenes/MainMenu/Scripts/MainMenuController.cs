using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Rewired;

public class MainMenuController : MonoBehaviour
{
    public GameObject ControlsMenu, selectButton, controlsButton, quitButton;

    /// <summary>
    /// Maps the player number to the Rewired Player.
    /// </summary>
    public static Dictionary<int, int> controllerIDToPlayerID = new Dictionary<int, int>();

    Rewired.Player player1;
    
    private GameObject[] buttons = new GameObject[3];
    private int iter = 0;
    private int connectedControllers;

    public void Awake()
    {
        // Subscribe to events
        ReInput.ControllerConnectedEvent += OnControllerConnected;
        ReInput.ControllerDisconnectedEvent += OnControllerDisconnected;
        ReInput.ControllerPreDisconnectEvent += OnControllerPreDisconnect;
    }

    void Start()
    {
        //player1 = ReInput.players.GetPlayer(0);
        buttons[0] = (selectButton);
        buttons[1] = (controlsButton);
        buttons[2] = (quitButton);

        buttons[iter].GetComponent<Image>().color = buttons[iter].GetComponent<Button>().colors.highlightedColor;

        // Subscribe to events
        ReInput.ControllerConnectedEvent += OnControllerConnected;
        ReInput.ControllerDisconnectedEvent += OnControllerDisconnected;
        ReInput.ControllerPreDisconnectEvent += OnControllerPreDisconnect;

        foreach (Joystick cont in ReInput.controllers.Joysticks)
        {
            print("Controller (" + cont.id + ") found.");
            controllerIDToPlayerID.Add(cont.id, cont.id);
            if (cont.id == 0)
            {
                player1 = ReInput.players.GetPlayer(cont.id);
            }
                

        }
    }

    void Update()
    {
        connectedControllers = ReInput.controllers.Controllers.Count;
        print("CID to PID: " + controllerIDToPlayerID.Count);
        print("Connected Controllers: " + connectedControllers);
        if (player1.controllers.Joysticks.Count > 0)
        {
            if (player1.GetButtonDown("Select"))
            {
                if(iter == 0)
                {
                    playGame();
                }
                else if(iter == 1)
                {
                    options();
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
        }
    }

    // This function will be called when a controller is connected
    // You can get information about the controller that was connected via the args parameter
    void OnControllerConnected(ControllerStatusChangedEventArgs args)
    {
        Debug.Log("A controller was connected! Name = " + args.name + " Id = " + args.controllerId + " Type = " + args.controllerType);
        if (args.controllerType == ControllerType.Joystick)
        {
            if (args.controllerId == 0)
            {
                player1 = ReInput.players.GetPlayer(args.controllerId);
            }
            controllerIDToPlayerID.Add(args.controllerId, args.controllerId);
        }
       
    }

    // This function will be called when a controller is fully disconnected
    // You can get information about the controller that was disconnected via the args parameter
    void OnControllerDisconnected(ControllerStatusChangedEventArgs args)
    {
        Debug.Log("A controller was disconnected! Name = " + args.name + " Id = " + args.controllerId + " Type = " + args.controllerType);
        if (args.controllerType == ControllerType.Joystick)
        {
            controllerIDToPlayerID.Remove(args.controllerId);
            if (args.controllerId == 0)
            {
                player1 = null;
            }
            
        }
        
    }

    // This function will be called when a controller is about to be disconnected
    // You can get information about the controller that is being disconnected via the args parameter
    // You can use this event to save the controller's maps before it's disconnected
    void OnControllerPreDisconnect(ControllerStatusChangedEventArgs args)
    {
        Debug.Log("A controller is being disconnected! Name = " + args.name + " Id = " + args.controllerId + " Type = " + args.controllerType);
    }


    private void playGame()
    {
        SceneManager.LoadScene("Character Select");
    }

    private void options()
    {
        ControlsMenu.SetActive(true);
    }

    private void exitGame()
    {
        Application.Quit();
    }
}