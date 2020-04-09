using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles the updates to the visuals in the Character Select screen.
/// </summary>
public class ChS_View : MonoBehaviour
{
    public GameObject player1Hover, player2Hover, player3Hover, player4Hover, tankIcon, soldierIcon,
        rogueIcon, engineerIcon, selectButton1, selectButton2, selectButton3, selectButton4, upButton1,
        upButton2, upButton3, upButton4, downButton1, downButton2, downButton3, downButton4;

    private Dictionary<int, GameObject> playerToHover = new Dictionary<int, GameObject>();
    public Dictionary<int, GameObject> playerToSelectBtn = new Dictionary<int, GameObject>();
    private Dictionary<int, GameObject> playerToUpBtn = new Dictionary<int, GameObject>();
    private Dictionary<int, GameObject> playerToDownBtn = new Dictionary<int, GameObject>();

    public bool player1, player2, player3, player4;

    /// <summary>
    /// Initialize the images and dictionaries used in the View.
    /// </summary>
    public void initialize()
    {
        // Set all Hover sprites to Tank to start.
        player1Hover.GetComponent<Image>().sprite = tankIcon.GetComponent<Image>().sprite;
        player2Hover.GetComponent<Image>().sprite = tankIcon.GetComponent<Image>().sprite;
        player3Hover.GetComponent<Image>().sprite = tankIcon.GetComponent<Image>().sprite;
        player4Hover.GetComponent<Image>().sprite = tankIcon.GetComponent<Image>().sprite;

        // Initialize dictionary for Hover Images
        playerToHover.Add(1, player1Hover);
        playerToHover.Add(2, player2Hover);
        playerToHover.Add(3, player3Hover);
        playerToHover.Add(4, player4Hover);

        // Initialize dictionary for Select Buttons
        playerToSelectBtn.Add(1, selectButton1);
        playerToSelectBtn.Add(2, selectButton2);
        playerToSelectBtn.Add(3, selectButton3);
        playerToSelectBtn.Add(4, selectButton4);

        // Initialzie dictionary for the Up Buttons
        playerToUpBtn.Add(1, upButton1);
        playerToUpBtn.Add(2, upButton2);
        playerToUpBtn.Add(3, upButton3);
        playerToUpBtn.Add(4, upButton4);

        // Initialize dictionary for the Down Buttons
        playerToDownBtn.Add(1, downButton1);
        playerToDownBtn.Add(2, downButton2);
        playerToDownBtn.Add(3, downButton3);
        playerToDownBtn.Add(4, downButton4);
    }

    /// <summary>
    /// Fades the selected character Icon for all other players, changes the select button text to "Unselect",
    /// and fades out the Up and Down buttons.
    /// </summary>
    /// <param name="playerID"></param>
    public void characterSelected(int playerID)
    {
        playerToHover.TryGetValue(playerID, out GameObject thisPlayerIcon);
        Sprite thisSprite = thisPlayerIcon.GetComponent<Image>().sprite;

        // Fade this icon for the other players
        foreach (int player in playerToHover.Keys)
        {
            if (player != playerID)
            {
                playerToHover.TryGetValue(player, out GameObject otherPlayerIcon);
                
                if (otherPlayerIcon.GetComponent<Image>().sprite == thisSprite)
                {
                    fade(player);
                }
            }
        }
        // Change the Selected Button text to Unselect
        playerToSelectBtn[playerID].GetComponentInChildren<Text>().GetComponent<Text>().text = "Unselect";

        // Fade the Up and Down arrows
        playerToUpBtn[playerID].GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
        playerToDownBtn[playerID].GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);

    }

    /// <summary>
    /// Restores the selected character Icon for all other players, changes the select button text to "Select",
    /// and restores the Up and Down buttons.
    /// </summary>
    /// <param name="playerID"></param>
    public void characterUnselected(int playerID)
    {
        playerToHover.TryGetValue(playerID, out GameObject thisPlayerIcon);
        Sprite thisSprite = thisPlayerIcon.GetComponent<Image>().sprite;

        // Restore this icon for the other players
        foreach (int player in playerToHover.Keys)
        {
            if (player != playerID)
            {
                playerToHover.TryGetValue(player, out GameObject otherPlayerIcon);

                if (otherPlayerIcon.GetComponent<Image>().sprite == thisSprite)
                {
                    restore(player);
                }
            }
        }
        // Change the Selected Button text to Unselect
        playerToSelectBtn[playerID].GetComponentInChildren<Text>().GetComponent<Text>().text = "Select";

        // Fade the Up and Down arrows
        playerToUpBtn[playerID].GetComponent<Image>().color = new Color(1, 1, 1, 1f);
        playerToDownBtn[playerID].GetComponent<Image>().color = new Color(1, 1, 1, 1f);

    }

    /// <summary>
    /// Changes the Icon this player is hovering to the input character's Icon. Fades out the character's Icon
    /// if the character has already been selected.
    /// </summary>
    /// <param name="playerID"></param>
    /// <param name="character"></param>
    public void nextCharacter(int playerID, ChS_Model.Character character)
    {
        // If the character has a playerID of -1, it means the player has selected this character, and the 
        // Up and Down buttons should do nothing. So, do nothing and return.
        if (character.playerID == -1)
        {
            return;
        }

        playerToHover.TryGetValue(playerID, out GameObject imageToChange);
        imageToChange.GetComponent<Image>().sprite = character.characterIcon.GetComponent<Image>().sprite;
        
        // If this character has been selected, fade the icon.
        if (character.selected)
        {
            fade(playerID);
        }
        else
        {
            restore(playerID);
        }
    }

    /// <summary>
    /// Fades the playerIDs Icon for all other players. Also fades the Select button for all other players
    /// when they are hovering the playerIDs Icon.
    /// </summary>
    /// <param name="playerID"></param>
    private void fade(int playerID)
    {
        playerToHover.TryGetValue(playerID, out GameObject imageToChange);
        imageToChange.GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
        playerToSelectBtn[playerID].GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
    }

    /// <summary>
    /// Restores the playerIDs Icon for all other players. Also restores the Select button for all other players.
    /// <param name="playerID"></param>
    private void restore(int playerID)
    {
        playerToHover.TryGetValue(playerID, out GameObject imageToChange);
        imageToChange.GetComponent<Image>().color = new Color(1, 1, 1, 1f);
        playerToSelectBtn[playerID].GetComponent<Image>().color = new Color(1, 1, 1, 1f);
    }

    public void toggleGroupOff(GameObject group)
    {
        group.gameObject.SetActive(false);
    }

    public void toggleGroupOn(GameObject group)
    {
        group.gameObject.SetActive(true);
    }

}
