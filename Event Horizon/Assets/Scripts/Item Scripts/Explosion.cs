using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{

    public AudioSource explosion;
    // Start is called before the first frame update
    void Start()
    {

        if (!explosion.isPlaying)
        {
            explosion.Play();
        }
        Destroy(gameObject, 2f);
    }
}
