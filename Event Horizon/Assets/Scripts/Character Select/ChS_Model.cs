using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles all the back-end logic for Character Selection.
/// </summary>
public class ChS_Model : MonoBehaviour
{
    public GameObject tankIcon, soldierIcon, rogueIcon, engineerIcon;

    /// <summary>
    /// Represents a Character in the Character Select screen. Contains the GameObject for the 
    /// characters icon, a bool to tell if this character has been selected, and the id of the 
    /// player that selected it. If the character has not been selected, then selected is False 
    /// and playerID is -1.
    /// </summary>
    public struct Character
    {
        public GameObject characterIcon;
        public bool selected;
        public int playerID;
    }
    Character tankChar, soldierChar, rogueChar, engineerChar;

    /// <summary>
    /// Map the playerID to the ID of the character they are currently hovering. Player IDs are
    /// 1-4.
    /// </summary>
    public static Dictionary<int, int> playerIDToCharacterID = new Dictionary<int, int>()
    {
        // All Players set to hover the Tank at start.
        {1, 0},
        {2, 0},
        {3, 0},
        {4, 0}
    };

    /// <summary>
    /// Map the charcter ID to the corresponding Character struct.
    /// </summary>
    public static Dictionary<int, Character> idToCharacter = new Dictionary<int, Character>();

    /// <summary>
    /// Initialize the character structs and add to the idToCharacter dictionary.
    /// </summary>
    public void intitialize()
    {
        tankChar = new Character()
        {
            characterIcon = tankIcon,
            selected = false,
            playerID = 0
        };
        idToCharacter.Add(0, tankChar);

        soldierChar = new Character()
        {
            characterIcon = soldierIcon,
            selected = false,
            playerID = 0
        };
        idToCharacter.Add(1, soldierChar);

        rogueChar = new Character()
        {
            characterIcon = rogueIcon,
            selected = false,
            playerID = 0
        };
        idToCharacter.Add(2, rogueChar);

        engineerChar = new Character()
        {
            characterIcon = engineerIcon,
            selected = false,
            playerID = 0
        };
        idToCharacter.Add(3, engineerChar);
    }

    /// <summary>
    /// If this character can be selected, assign the player ID to the class and mark as selected. 
    /// Else return false.
    /// </summary>
    /// <param name="playerID"></param>
    /// <param name="classID"></param>
    public bool selectCharacter(int playerID)
    {
        playerIDToCharacterID.TryGetValue(playerID, out int classID);
        if (idToCharacter[classID].selected == true)
        {
            return false;
        }
        else
        {
            idToCharacter[classID] = new Character()
            {
                characterIcon = idToCharacter[classID].characterIcon,
                selected = true,
                playerID = playerID
            };
        }
        return true;
    }

    /// <summary>
    /// Unassign the player ID to the class, amd mark as not selected.
    /// </summary>
    /// <param name="playerID"></param>
    /// <param name="classID"></param>
    public void unselectCharacter(int playerID)
    {
        playerIDToCharacterID.TryGetValue(playerID, out int classID);
        idToCharacter[classID] = new Character()
        {
            characterIcon = idToCharacter[classID].characterIcon,
            selected = false,
            playerID = 0
        };
    }

    /// <summary>
    /// Return the character with the corresponding classID.
    /// </summary>
    /// <param name="characterID"></param>
    public Character getCharacter(int characterID)
    {
        idToCharacter.TryGetValue(characterID, out Character character);
        return character;
    }

    /// <summary>
    /// Return the next Character in the list.
    /// </summary>
    /// <param name="playerID"></param>
    /// <returns></returns>
    public Character nextCharacter(int playerID)
    {
        playerIDToCharacterID.TryGetValue(playerID, out int ID);

        // If this player has selected a character, the they can't use the Up and Down arrows.
        if (idToCharacter[ID].selected == true && idToCharacter[ID].playerID == playerID)
        {
            return new Character()
            {
                characterIcon = null,
                selected = true,
                playerID = -1
            };
        }

        int characterID = ID;
        if (characterID == 3)
        {
            characterID = 0;
        }
        else
        {
            characterID += 1;
        }
        playerIDToCharacterID[playerID] = characterID;

        return getCharacter(characterID);
    }

    /// <summary>
    /// Return the previous Character in the list.
    /// </summary>
    /// <param name="playerID"></param>
    /// <returns></returns>
    public Character previousCharacter(int playerID)
    {
        playerIDToCharacterID.TryGetValue(playerID, out int ID);

        // If this player has selected a character, the they can't use the Up and Down arrows.
        if (idToCharacter[ID].selected == true && idToCharacter[ID].playerID == playerID)
        {
            return new Character()
            {
                characterIcon = null,
                selected = true,
                playerID = -1
            };
        }

        int characterID = ID;
        if (characterID == 0)
        {
            characterID = 3;
        }
        else
        {
            characterID -= 1;
        }
        playerIDToCharacterID[playerID] = characterID;

        return getCharacter(characterID);
    }

    /// <summary>
    /// Gets the playerIDToCharacterID Dict.
    /// </summary>
    /// <returns></returns>
    public Dictionary<int, int> getPIDtoCID()
    {
        return playerIDToCharacterID;
    }

    /// <summary>
    /// Gets the idToCharacter Dict.
    /// </summary>
    /// <returns></returns>
    public Dictionary<int, Character> getCIDtoC()
    {
        return idToCharacter;
    }
}

