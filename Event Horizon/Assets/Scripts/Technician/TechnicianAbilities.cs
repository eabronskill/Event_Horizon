using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TechnicianAbilities : MonoBehaviour
{
    // Start is called before the first frame update
    public bool canSetTurret;
    public float turretCD;
    public bool turretSet;

    public bool canRepair;
    public float repairCD;

    public float turretTimeRemaining;
    public float repairTimeRemaining;

    private float turretDestroyedTime;
    private float lastRepairTime;

    public GameObject turret;
    //Enemy enemy;

    void Start()
    {
        canSetTurret = true;
        canRepair = true;
        turretSet = false;
        turretCD = 120.0f;
        repairCD = 90.0f;

    }

    // Update is called once per frame
    void Update()
    {
        if (!canSetTurret && !turretSet)
        {
            turretTimeRemaining = turretDestroyedTime + turretCD - Time.time;
        }

        if (!canRepair)
        {
            repairTimeRemaining = lastRepairTime + repairCD - Time.time;
        }
    }

    public void setTurret()
    {
        canSetTurret = false;

        //place turret in game
        Vector3 turretLoc = transform.position;
        turretLoc.y += .3f;
        turretLoc += transform.forward * 2;

        turret = Instantiate(turret, turretLoc, transform.rotation); //TODO: TURRET LOGIC
        Invoke("resetTurret", turretCD);
    }

    private void resetTurret()
    {
        canSetTurret = true;
    }

    public void repair()
    {
        canRepair = false;
        lastRepairTime = Time.time;
        Invoke("resetRepair", repairCD);

        //TODO: REPAIR TURRET OR TANK
    }


    private void resetRepair()
    {
        canRepair = true;
    }
}
