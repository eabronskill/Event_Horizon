using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RogueAbilities : MonoBehaviour
{
    // Start is called before the first frame update
    public bool canSetSpikes;
    private float spikeCooldown;

    public bool canSetMine;
    private float mineCooldown;

    public GameObject spikes;
    public GameObject mine;
    public GameObject grenade;
    Enemy enemy;

    void Start()
    {
        canSetSpikes = true;
        enemy = new Enemy();
        enemy.mineExplosionEvent += detonate;
        canSetMine = true;
        spikeCooldown = 3.0f;
        mineCooldown = 3.0f;
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void setSpikes()
    {
        canSetSpikes = false;
        Invoke("resetSpikes", spikeCooldown);

        //actually put in spikes
        Instantiate(spikes, transform.position, transform.rotation);
    }

    private void resetSpikes()
    {
        canSetSpikes = true;
    }

    public void setMine()
    {
        canSetMine = false;
        Invoke("resetMine", mineCooldown);

        //actually put in spikes
        Instantiate(mine, transform.position, transform.rotation);
    }

    private void detonate()
    {
        print("boom!");
        GameObject grenadeObj = Instantiate(grenade, mine.transform);
    }

    private void resetMine()
    {
        canSetMine = true;
    }
}
