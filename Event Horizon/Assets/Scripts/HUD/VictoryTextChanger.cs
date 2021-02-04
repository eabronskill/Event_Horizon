using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VictoryTextChanger : MonoBehaviour
{
    public GameObject text;
    public string newText;
    void Update()
    {
        if (CharacterSelectController._singlePlayer) text.GetComponent<Text>().text = newText;   
    }
}
