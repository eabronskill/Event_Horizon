using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class greenLights : MonoBehaviour
{

    public List<Light> roomLights;
    public List<GameObject> enemies;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        bool donezo = true;
        foreach(GameObject enemy in enemies)
        {
            if (enemy != null)
                donezo = false;
        }

        if (donezo)
            foreach (Light light in roomLights)
                light.color = Color.white;
    }
}
