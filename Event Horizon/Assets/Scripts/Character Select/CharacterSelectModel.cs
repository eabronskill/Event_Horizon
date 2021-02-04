using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles all the back-end logic for Character Selection.
/// </summary>
public class CharacterSelectModel
{
    public GameObject _tankIcon, _soldierIcon, _rogueIcon, _engineerIcon;

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
    Character _tankChar, _soldierChar, _rogueChar, _engineerChar;

    /// <summary>
    /// Map the playerID to the ID of the character they are currently hovering. Player IDs are
    /// 1-4.
    /// </summary>
    public static Dictionary<int, int> _playerIDToCharacterID = new Dictionary<int, int>()
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
    public static Dictionary<int, Character> _idToCharacter = new Dictionary<int, Character>();

    /// <summary>
    /// Initialize the character structs and add to the idToCharacter dictionary.
    /// </summary>
    public void Intitialize()
    {
        _tankChar = new Character()
        {
            characterIcon = _tankIcon,
            selected = false,
            playerID = 0
        };
        _idToCharacter.Add(0, _tankChar);

        _soldierChar = new Character()
        {
            characterIcon = _soldierIcon,
            selected = false,
            playerID = 0
        };
        _idToCharacter.Add(1, _soldierChar);

        _rogueChar = new Character()
        {
            characterIcon = _rogueIcon,
            selected = false,
            playerID = 0
        };
        _idToCharacter.Add(2, _rogueChar);

        _engineerChar = new Character()
        {
            characterIcon = _engineerIcon,
            selected = false,
            playerID = 0
        };
        _idToCharacter.Add(3, _engineerChar);
    }

    /// <summary>
    /// If this character can be selected, assign the player ID to the class and mark as selected. 
    /// Else return false.
    /// </summary>
    /// <param name="playerID"></param>
    /// <param name="classID"></param>
    public bool SelectCharacter(int playerID)
    {
        _playerIDToCharacterID.TryGetValue(playerID, out int classID);
        if (_idToCharacter[classID].selected == true)
        {
            return false;
        }
        else
        {
            _idToCharacter[classID] = new Character()
            {
                characterIcon = _idToCharacter[classID].characterIcon,
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
    public void UnselectCharacter(int playerID)
    {
        _playerIDToCharacterID.TryGetValue(playerID, out int classID);
        _idToCharacter[classID] = new Character()
        {
            characterIcon = _idToCharacter[classID].characterIcon,
            selected = false,
            playerID = 0
        };
    }

    /// <summary>
    /// Return the character with the corresponding classID.
    /// </summary>
    /// <param name="characterID"></param>
    public Character GetCharacter(int characterID)
    {
        _idToCharacter.TryGetValue(characterID, out Character character);
        return character;
    }

    /// <summary>
    /// Return the next Character in the list.
    /// </summary>
    /// <param name="playerID"></param>
    /// <returns></returns>
    public Character NextCharacter(int playerID)
    {
        _playerIDToCharacterID.TryGetValue(playerID, out int ID);

        // If this player has selected a character, the they can't use the Up and Down arrows.
        if (_idToCharacter[ID].selected == true && _idToCharacter[ID].playerID == playerID)
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
        _playerIDToCharacterID[playerID] = characterID;

        return GetCharacter(characterID);
    }

    /// <summary>
    /// Return the previous Character in the list.
    /// </summary>
    /// <param name="playerID"></param>
    /// <returns></returns>
    public Character PreviousCharacter(int playerID)
    {
        _playerIDToCharacterID.TryGetValue(playerID, out int ID);

        // If this player has selected a character, the they can't use the Up and Down arrows.
        if (_idToCharacter[ID].selected == true && _idToCharacter[ID].playerID == playerID)
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
        _playerIDToCharacterID[playerID] = characterID;

        return GetCharacter(characterID);
    }

    /// <summary>
    /// Gets the playerIDToCharacterID Dict.
    /// </summary>
    /// <returns></returns>
    public Dictionary<int, int> GetPIDtoCID()
    {
        return _playerIDToCharacterID;
    }

    /// <summary>
    /// Gets the idToCharacter Dict.
    /// </summary>
    /// <returns></returns>
    public Dictionary<int, Character> GetCIDtoC()
    {
        return _idToCharacter;
    }
}

