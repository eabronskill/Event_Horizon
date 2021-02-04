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
        if (ChS_Controller._singlePlayer) text.GetComponent<Text>().text = newText;   
    }
}
