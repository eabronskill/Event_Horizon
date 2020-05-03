using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    public GameObject explosionPrefab;

    public float explosionDelay = 2f;
    public float range = 8f;
    private float countdown1;
    private float countdown2;
    private bool hasExploded = false;

    //sound
    public AudioSource enemyhit;

    // Start is called before the first frame update
    void Start()
    {
        countdown1 = explosionDelay;
    }

    // Update is called once per frame
    void Update()
    {
        countdown1 -= Time.deltaTime;
        if (countdown1 <= 0f && !hasExploded)
        {
            explode();
            hasExploded = true;
        }
    }

    private void explode()
    {
        // Animation
        Instantiate(explosionPrefab, transform.position, transform.rotation);

        // Get what enemies are in range and deal damage.
        Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, range);
        int i = 0;
        while (i < hitColliders.Length)
        {
            if (hitColliders[i].tag == "Enemy")
            {
                enemyhit.Play();
                hitColliders[i].gameObject.GetComponent<Enemy>().takeDamage(50);
            }
            i++;
        }
        Destroy(gameObject);
    }
}
