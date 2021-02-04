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
        if (ChS_Controller._singlePlayer) text.text = "'E' to Pickup";
    }
}
