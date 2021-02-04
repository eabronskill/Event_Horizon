using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Rewired;

public class MainMenuController : MonoBehaviour
{
    public GameObject _controlsMenu, _startButton, _controlsButton, _quitButton;

    public AudioSource _buttonSwitch;

    /// <summary>
    /// Maps the player number to the Rewired Player.
    /// </summary>
    public static Dictionary<int, int> _controllerIDToPlayerID = new Dictionary<int, int>();

    Rewired.Player _player1;
    
    private GameObject[] _buttons = new GameObject[3];
    private int _buttonsIter = 0;
    bool _controllerConnected;

    public void Awake()
    {
        // Subscribe to events
        ReInput.ControllerConnectedEvent += OnControllerConnected;
        ReInput.ControllerDisconnectedEvent += OnControllerDisconnected;
        ReInput.ControllerPreDisconnectEvent += OnControllerPreDisconnect;
    }

    void Start()
    {
        _buttons[0] = _startButton;
        _buttons[1] = _controlsButton;
        _buttons[2] = _quitButton;

        // Subscribe to events
        ReInput.ControllerConnectedEvent += OnControllerConnected;
        ReInput.ControllerDisconnectedEvent += OnControllerDisconnected;
        ReInput.ControllerPreDisconnectEvent += OnControllerPreDisconnect;

        // If any controllers are connected before the game starts, grab them now.
        // The events wont fire if they are already connected.
        foreach (Joystick cont in ReInput.controllers.Joysticks)
        {
            _controllerIDToPlayerID.Add(cont.id, cont.id);
            if (cont.id == 0)
            {
                _player1 = ReInput.players.GetPlayer(cont.id);
            }
        }
    }

    void Update()
    {
        _controllerConnected = _player1 != null && _player1.controllers.Joysticks.Count > 0;
        if (_controllerConnected)
        {
            ControllerInput();
        }
        else KeyboardInput();
    }

    void ControllerInput()
    {
        if (_player1.GetButtonDown("Select"))
        {
            _buttonSwitch.pitch = 1;
            _buttonSwitch.Play();
            if (_buttonsIter == 0)
            {
                PlayGame();
            }
            else if (_buttonsIter == 1)
            {
                OpenControlls();
            }
            else
            {
                ExitGame();
            }
        }
        else if (_player1.GetButtonDown("Up"))
        {
            _buttonSwitch.pitch = 2;
            _buttonSwitch.Play();
            _buttons[_buttonsIter].GetComponent<Image>().color = _buttons[_buttonsIter].GetComponent<Button>().colors.normalColor;
            if (_buttonsIter == 0)
            {
                _buttonsIter = 2;
            }
            else
            {
                _buttonsIter--;
            }
            _buttons[_buttonsIter].GetComponent<Image>().color = _buttons[_buttonsIter].GetComponent<Button>().colors.highlightedColor;
        }
        else if (_player1.GetButtonDown("Down"))
        {
            _buttonSwitch.pitch = 2;
            _buttonSwitch.Play();
            _buttons[_buttonsIter].GetComponent<Image>().color = _buttons[_buttonsIter].GetComponent<Button>().colors.normalColor;

            if (_buttonsIter == 2)
            {
                _buttonsIter = 0;
            }
            else
            {
                _buttonsIter++;
            }
            _buttons[_buttonsIter].GetComponent<Image>().color = _buttons[_buttonsIter].GetComponent<Button>().colors.highlightedColor;
        }
        else if (_controlsMenu.activeSelf && _player1.GetButtonDown("Back"))
        {
            _buttonSwitch.pitch = 1;
            _buttonSwitch.Play();
            _controlsMenu.SetActive(false);
        }
    }
    void KeyboardInput()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            _buttonSwitch.pitch = 1;
            _buttonSwitch.Play();
            if (_buttonsIter == 0)
            {
                PlayGame();
            }
            else if (_buttonsIter == 1)
            {
                OpenControlls();
            }
            else
            {
                ExitGame();
            }
        }

        if (_controlsMenu.activeSelf && (Input.GetKeyDown(KeyCode.B) || Input.GetKeyDown(KeyCode.Escape)))
        {
            _buttonSwitch.pitch = 1;
            _buttonSwitch.Play();
            _controlsMenu.SetActive(false);
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
                _player1 = ReInput.players.GetPlayer(args.controllerId);
                _buttons[0].GetComponent<Image>().color = _buttons[0].GetComponent<Button>().colors.highlightedColor;
            }
            _controllerIDToPlayerID.Add(args.controllerId, args.controllerId);
        }
        _controllerIDToPlayerID.Add(args.controllerId, args.controllerId);
    }

    // This function will be called when a controller is fully disconnected
    // You can get information about the controller that was disconnected via the args parameter
    void OnControllerDisconnected(ControllerStatusChangedEventArgs args)
    {
        Debug.Log("A controller was disconnected! Name = " + args.name + " Id = " + args.controllerId + " Type = " + args.controllerType);
        if (args.controllerType == ControllerType.Joystick)
        {
            _controllerIDToPlayerID.Remove(args.controllerId);
            if (args.controllerId == 0)
            {
                _player1 = null;
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

    public void PlayGame()
    {
        SceneManager.LoadScene("Character Select");
    }

    public void OpenControlls()
    {
        _controlsMenu.SetActive(true);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}