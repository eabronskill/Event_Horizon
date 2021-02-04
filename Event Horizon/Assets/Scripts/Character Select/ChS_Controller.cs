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
    Dictionary<int, Rewired.Player> _playerIDToPlayer = new Dictionary<int, Rewired.Player>();

    /// <summary>
    /// Will be used to map the players to the characters in the actual levels.
    /// </summary>
    public static Dictionary<string, int> _finalSelection = new Dictionary<string, int>();

    ChS_Model _model;
    ChS_View _view;

    public AudioSource _buttonClickSound;

    public static bool _singlePlayer; // Used for singleplayer functionality in other classes.
    int _connectedControllers; 
    int _charactersSelected;
    bool _canPlay;

    public GameObject _readytoPlay;
    public GameObject _waitingforPlayers;

    void Awake()
    {
        // Inilialize model and view.
        _model = new ChS_Model()
        {
            _tankIcon = tankIcon,
            _soldierIcon = soldierIcon,
            _rogueIcon = rogueIcon,
            _engineerIcon = engineerIcon
        };
        _model.Intitialize();
        _view = new ChS_View()
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
        _view.Initialize();

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
            _playerIDToPlayer.Add(cont.id, ReInput.players.GetPlayer(cont.id));
        }
        _view.ToggleGroupOn(group1);
        _view.ToggleGroupOn(group2);
        _view.ToggleGroupOn(group3);
        _view.ToggleGroupOn(group4);

        // Subscribe to events
        ReInput.ControllerConnectedEvent += OnControllerConnected;
        ReInput.ControllerDisconnectedEvent += OnControllerDisconnected;
        ReInput.ControllerPreDisconnectEvent += OnControllerPreDisconnect;

        playButton.GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
        _readytoPlay.SetActive(false);
        _waitingforPlayers.SetActive(true);
    }

    void Update()
    {
        ControllerInput();

        int numNeeded = 0;
        // Check to see if all the characters have been selected. If one has not been selected, then don't load the level.
        foreach (int cID in _model.GetCIDtoC().Keys)
        {
            if (_model.GetCIDtoC()[cID].selected)
            {
                numNeeded++;
            }
        }

        if (numNeeded >= _connectedControllers && _charactersSelected > 0)
        {
            _canPlay = true;
            playButton.GetComponent<Image>().color = new Color(1, 1, 1, 1f);
            _readytoPlay.SetActive(true);
            _waitingforPlayers.SetActive(false);
        }
        else
        {
            _canPlay = false;
            playButton.GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
            _readytoPlay.SetActive(false);
            _waitingforPlayers.SetActive(true);
        }
    }

    void LateUpdate()
    {
        _connectedControllers = ReInput.controllers.Joysticks.Count;
    }

    void ControllerInput()
    {
        _view.ToggleGroupOn(group1);
        //Player 1 Logic
        if (_playerIDToPlayer.ContainsKey(0) && _playerIDToPlayer[0].controllers.Joysticks.Count > 0)
        {
            GetInput(0);
        }
        //Player 2 Logic
        if (_playerIDToPlayer.ContainsKey(1) && _playerIDToPlayer[1].controllers.Joysticks.Count > 0)
        {
            _view.ToggleGroupOn(group2);
            print("Player2");
            GetInput(1);
        }
        else
        {
            _view.ToggleGroupOff(group2);
        }
        //Player 3 Logic
        if (_playerIDToPlayer.ContainsKey(2) && _playerIDToPlayer[2].controllers.Joysticks.Count > 0)
        {
            _view.ToggleGroupOn(group3);
            print("Player3");
            GetInput(2);
        }
        else
        {
            _view.ToggleGroupOff(group3);
        }
        //Player 4 Logic
        if (_playerIDToPlayer.ContainsKey(3) && _playerIDToPlayer[3].controllers.Joysticks.Count > 0)
        {
            _view.ToggleGroupOn(group4);
            print("Player4");
            GetInput(3);
        }
        else
        {
            _view.ToggleGroupOff(group4);
        }
    }

    // This function will be called when a controller is connected
    // You can get information about the controller that was connected via the args parameter
    void OnControllerConnected(ControllerStatusChangedEventArgs args)
    {
        Debug.Log("A controller was connected! Name = " + args.name + " Id = " + args.controllerId + " Type = " + args.controllerType);

        if (args.controllerType == ControllerType.Joystick)
        {
            _playerIDToPlayer.Add(args.controllerId, ReInput.players.GetPlayer(args.controllerId));
        }
    }
    
    // This function will be called when a controller is fully disconnected
    // You can get information about the controller that was disconnected via the args parameter
    void OnControllerDisconnected(ControllerStatusChangedEventArgs args)
    {
        Debug.Log("A controller was disconnected! Name = " + args.name + " Id = " + args.controllerId + " Type = " + args.controllerType);
        if (args.controllerType == ControllerType.Joystick)
        {
            _playerIDToPlayer.Remove(args.controllerId);
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
    private void GetInput(int playerID)
    {
        if (_playerIDToPlayer[playerID].GetButtonDown("Select"))
        {
            _buttonClickSound.pitch = 1;
            _buttonClickSound.Play();
            print("Select" + playerID);
            SelectButtonClick(playerID+1);
        }
        if (_playerIDToPlayer[playerID].GetButtonDown("Up"))
        {
            _buttonClickSound.pitch = 2;
            _buttonClickSound.Play();
            print("Up" + playerID);
            UpButtonClick(playerID+1);
        }
        if (_playerIDToPlayer[playerID].GetButtonDown("Down"))
        {
            _buttonClickSound.pitch = 2;
            _buttonClickSound.Play();
            print("Down" + playerID);
            DownButtonClick(playerID+1);
        }
        if (_playerIDToPlayer[playerID].GetButtonDown("Play"))
        {
            print("Play" + playerID);
            PlayButtonClick();
        }
    }

    /// <summary>
    /// Called when a Select button is clicked.
    /// </summary>
    /// <param name="playerID"></param>
    /// <param name="classID"></param>
    public void SelectButtonClick(int playerID)
    {
        // If button has already been selected, call the unselect logic.
        if (_view._playerToSelectBtn[playerID].GetComponentInChildren<Text>().GetComponent<Text>().text == "Unselect")
        {
            _model.UnselectCharacter(playerID);
            _view.CharacterUnselected(playerID);
            _charactersSelected--;
        }
        else
        {
            if (_model.SelectCharacter(playerID))
            {
                _view.CharacterSelected(playerID);
                _charactersSelected++;
            }
        }
    }

    /// <summary>
    /// Called when a Up button is clicked.
    /// </summary>
    /// <param name="playerID"></param>
    public void UpButtonClick(int playerID)
    {
        _view.NextCharacter(playerID, _model.PreviousCharacter(playerID));
    }

    /// <summary>
    /// Called when a Down button is clicked.
    /// </summary>
    /// <param name="playerID"></param>
    public void DownButtonClick(int playerID)
    {
        _view.NextCharacter(playerID, _model.NextCharacter(playerID));
    }

    /// <summary>
    /// Called when the Play button is clicked.
    /// </summary>
    public void PlayButtonClick()
    {
        if (!_canPlay) return;

        // Finalize the selections
        for (int i = 1; i <= 4; i++)
        {
            if (_model.GetCIDtoC()[_model.GetPIDtoCID()[i]].selected && (_model.GetCIDtoC()[_model.GetPIDtoCID()[i]].playerID == i))
            {
                _finalSelection.Add(_model.GetCIDtoC()[_model.GetPIDtoCID()[i]].characterIcon.name, _model.GetCIDtoC()[_model.GetPIDtoCID()[i]].playerID -1);
                print(_finalSelection.Keys.Count);
            }
        }
        _buttonClickSound.pitch = 1;
        _buttonClickSound.Play();
        if (_connectedControllers == 0) _singlePlayer = true;
        SceneManager.LoadScene("Level1");
    }
}
