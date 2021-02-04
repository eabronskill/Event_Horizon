using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles the updates to the visuals in the Character Select screen.
/// </summary>
public class CharacterSelectView
{
    public GameObject player1Hover, player2Hover, player3Hover, player4Hover, tankIcon, soldierIcon,
        rogueIcon, engineerIcon, selectButton1, selectButton2, selectButton3, selectButton4, upButton1,
        upButton2, upButton3, upButton4, downButton1, downButton2, downButton3, downButton4;

    private Dictionary<int, GameObject> _playerToHover = new Dictionary<int, GameObject>();
    public Dictionary<int, GameObject> _playerToSelectBtn = new Dictionary<int, GameObject>();
    private Dictionary<int, GameObject> _playerToUpBtn = new Dictionary<int, GameObject>();
    private Dictionary<int, GameObject> _playerToDownBtn = new Dictionary<int, GameObject>();

    public bool _player1, _player2, _player3, _player4;

    /// <summary>
    /// Initialize the images and dictionaries used in the View.
    /// </summary>
    public void Initialize()
    {
        // Set all Hover sprites to Tank to start.
        player1Hover.GetComponent<Image>().sprite = tankIcon.GetComponent<Image>().sprite;
        player2Hover.GetComponent<Image>().sprite = tankIcon.GetComponent<Image>().sprite;
        player3Hover.GetComponent<Image>().sprite = tankIcon.GetComponent<Image>().sprite;
        player4Hover.GetComponent<Image>().sprite = tankIcon.GetComponent<Image>().sprite;

        // Initialize dictionary for Hover Images
        _playerToHover.Add(1, player1Hover);
        _playerToHover.Add(2, player2Hover);
        _playerToHover.Add(3, player3Hover);
        _playerToHover.Add(4, player4Hover);

        // Initialize dictionary for Select Buttons
        _playerToSelectBtn.Add(1, selectButton1);
        _playerToSelectBtn.Add(2, selectButton2);
        _playerToSelectBtn.Add(3, selectButton3);
        _playerToSelectBtn.Add(4, selectButton4);

        // Initialzie dictionary for the Up Buttons
        _playerToUpBtn.Add(1, upButton1);
        _playerToUpBtn.Add(2, upButton2);
        _playerToUpBtn.Add(3, upButton3);
        _playerToUpBtn.Add(4, upButton4);

        // Initialize dictionary for the Down Buttons
        _playerToDownBtn.Add(1, downButton1);
        _playerToDownBtn.Add(2, downButton2);
        _playerToDownBtn.Add(3, downButton3);
        _playerToDownBtn.Add(4, downButton4);
    }

    /// <summary>
    /// Fades the selected character Icon for all other players, changes the select button text to "Unselect",
    /// and fades out the Up and Down buttons.
    /// </summary>
    /// <param name="playerID"></param>
    public void CharacterSelected(int playerID)
    {
        _playerToHover.TryGetValue(playerID, out GameObject thisPlayerIcon);
        Sprite thisSprite = thisPlayerIcon.GetComponent<Image>().sprite;

        // Fade this icon for the other players
        foreach (int player in _playerToHover.Keys)
        {
            if (player != playerID)
            {
                _playerToHover.TryGetValue(player, out GameObject otherPlayerIcon);
                
                if (otherPlayerIcon.GetComponent<Image>().sprite == thisSprite)
                {
                    FadeSelectedIcon(player);
                }
            }
        }
        // Change the Selected Button text to Unselect
        _playerToSelectBtn[playerID].GetComponentInChildren<Text>().GetComponent<Text>().text = "Unselect";

        // Fade the Up and Down arrows
        _playerToUpBtn[playerID].GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
        _playerToDownBtn[playerID].GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);

    }

    /// <summary>
    /// Restores the selected character Icon for all other players, changes the select button text to "Select",
    /// and restores the Up and Down buttons.
    /// </summary>
    /// <param name="playerID"></param>
    public void CharacterUnselected(int playerID)
    {
        _playerToHover.TryGetValue(playerID, out GameObject thisPlayerIcon);
        Sprite thisSprite = thisPlayerIcon.GetComponent<Image>().sprite;

        // Restore this icon for the other players
        foreach (int player in _playerToHover.Keys)
        {
            if (player != playerID)
            {
                _playerToHover.TryGetValue(player, out GameObject otherPlayerIcon);

                if (otherPlayerIcon.GetComponent<Image>().sprite == thisSprite)
                {
                    RestoreSelectedIcon(player);
                }
            }
        }
        // Change the Selected Button text to Unselect
        _playerToSelectBtn[playerID].GetComponentInChildren<Text>().GetComponent<Text>().text = "Select";

        // Fade the Up and Down arrows
        _playerToUpBtn[playerID].GetComponent<Image>().color = new Color(1, 1, 1, 1f);
        _playerToDownBtn[playerID].GetComponent<Image>().color = new Color(1, 1, 1, 1f);

    }

    /// <summary>
    /// Changes the Icon this player is hovering to the input character's Icon. Fades out the character's Icon
    /// if the character has already been selected.
    /// </summary>
    /// <param name="playerID"></param>
    /// <param name="character"></param>
    public void NextCharacter(int playerID, CharacterSelectModel.Character character)
    {
        // If the character has a playerID of -1, it means the player has selected this character, and the 
        // Up and Down buttons should do nothing. So, do nothing and return.
        if (character.playerID == -1)
        {
            return;
        }

        _playerToHover.TryGetValue(playerID, out GameObject imageToChange);
        imageToChange.GetComponent<Image>().sprite = character.characterIcon.GetComponent<Image>().sprite;
        
        // If this character has been selected, fade the icon.
        if (character.selected)
        {
            FadeSelectedIcon(playerID);
        }
        else
        {
            RestoreSelectedIcon(playerID);
        }
    }

    /// <summary>
    /// Fades the playerIDs Icon for all other players. Also fades the Select button for all other players
    /// when they are hovering the playerIDs Icon.
    /// </summary>
    /// <param name="playerID"></param>
    private void FadeSelectedIcon(int playerID)
    {
        _playerToHover.TryGetValue(playerID, out GameObject imageToChange);
        imageToChange.GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
        _playerToSelectBtn[playerID].GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
    }

    /// <summary>
    /// Restores the playerIDs Icon for all other players. Also restores the Select button for all other players.
    /// <param name="playerID"></param>
    private void RestoreSelectedIcon(int playerID)
    {
        _playerToHover.TryGetValue(playerID, out GameObject imageToChange);
        imageToChange.GetComponent<Image>().color = new Color(1, 1, 1, 1f);
        _playerToSelectBtn[playerID].GetComponent<Image>().color = new Color(1, 1, 1, 1f);
    }

    public void ToggleGroupOff(GameObject group)
    {
        group.gameObject.SetActive(false);
    }

    public void ToggleGroupOn(GameObject group)
    {
        group.gameObject.SetActive(true);
    }

}
