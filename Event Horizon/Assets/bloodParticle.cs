using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bloodParticle : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("selfDestruct", 3.0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void selfDestruct()
    {
        Destroy(this.gameObject);
    }
}
