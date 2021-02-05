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

    [HideInInspector] public float turretDestroyedTime;
    [HideInInspector] public float lastRepairTime;

    public float turretAliveTime;

    public GameObject turretGObj;
    public GameObject turretPrefab;
    Turret turret;
    //Enemy enemy;

    void Start()
    {
        canSetTurret = true;
        canRepair = true;
        turretSet = false;
    }

    // Update is called once per frame
    void Update()
    {
        turretTimeRemaining = turretDestroyedTime - Time.time;
        repairTimeRemaining = lastRepairTime - Time.time;
        if (turretTimeRemaining < 0)
        {
            turretTimeRemaining = 0;
        }

        if (repairTimeRemaining < 0)
        {
            repairTimeRemaining = 0;
        }
    }

    public void setTurret()
    {
        canSetTurret = false;
        turretDestroyedTime = Time.time + turretCD;

        //place turret in game
        Vector3 turretLoc = transform.position;
        turretLoc.y += .3f;
        turretLoc += transform.forward * 2;

        if (turret)
        {
            turret.Kill();
            turret = null;
        } 
        turretGObj = Instantiate(turretPrefab, turretLoc, transform.rotation);
        turret = turretGObj.GetComponent<Turret>();
        turret._aliveTime = turretAliveTime;
        Invoke("resetTurret", turretCD);
        Invoke("destroyTurret", turretAliveTime);
        turretSet = true;
    }

    private void destroyTurret()
    {
        Destroy(turretGObj);
    }
    private void resetTurret()
    {
        canSetTurret = true;
    }

    public void repair()
    {
        canRepair = false;
        lastRepairTime = Time.time + repairCD;

        SendMessage("engineerHeal");
        Invoke("resetRepair", repairCD);
        
    }


    private void resetRepair()
    {
        canRepair = true;
    }
}
