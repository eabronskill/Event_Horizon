using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TankShieldTextSwitcher : MonoBehaviour
{
    public TextMeshPro text;
    // Start is called before the first frame update
    void Start()
    {
        if (CharacterSelectController._singlePlayer) text.text = "'E' to Pickup";
    }
}
