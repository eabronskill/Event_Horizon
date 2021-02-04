using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    private float CurAmmo;
    private float MaxAmmo;
    private float CurHealth;
    private float MaxHealth;
    private bool hasAmmo;
    private bool hasFAK;
    public Text AmmoCounter;
    public Text AmmoReserve;
    public Material health;
    public Material AbilityOne;
    public Material AbilityTwo;
    public GameObject ammoPickup;
    public GameObject FAK;
    //Player GameObjects
    public GameObject Tank;
    public GameObject Soldier;
    public GameObject Rogue;
    public GameObject Engineer;
    public GameObject classImage;
    public GameObject abilityImage1;
    public GameObject abilityImage2;
    public int playerNumber;
    //Class Images
    public Sprite tankPic;
    public Sprite soldierPic;
    public Sprite roguePic;
    public Sprite engineerPic;
    //Ability Images
    public Sprite groundPound;
    public Sprite shieldImage;
    public Sprite turretImage;
    public Sprite repairImage;
    public Sprite rapidFire;
    public Sprite Grenade;
    public Sprite Caltrops;
    public Sprite landMine;



    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LateStart(0.01f));

        
        
        
    }

    IEnumerator LateStart(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        if (playerNumber == Tank.GetComponent<TankInput>()._playerID)
        {
            CurHealth = Tank.GetComponent<TankInput>()._curHealth;
            MaxHealth = Tank.GetComponent<TankInput>()._maxHealth;
            CurAmmo = Tank.GetComponent<TankInput>()._curClip;
            MaxAmmo = Tank.GetComponent<TankInput>()._curAmmo;
            classImage.GetComponent<Image>().sprite = tankPic;
            abilityImage1.GetComponent<Image>().sprite = shieldImage;
            abilityImage2.GetComponent<Image>().sprite = groundPound;
            classImage.GetComponent<Image>().color = new Color(0f, 1f, 0.98f);
            //print("Player " + playerNumber + "is assigned Tank.");

        }
        else if (playerNumber == Soldier.GetComponent<SoldierInput>()._playerID)
        {
            CurHealth = Soldier.GetComponent<SoldierInput>()._curHealth;
            MaxHealth = Soldier.GetComponent<SoldierInput>()._maxHealth;
            CurAmmo = Soldier.GetComponent<SoldierInput>()._curClip;
            MaxAmmo = Soldier.GetComponent<SoldierInput>()._curAmmo;
            classImage.GetComponent<Image>().sprite = soldierPic;
            abilityImage1.GetComponent<Image>().sprite = rapidFire;
            abilityImage2.GetComponent<Image>().sprite = Grenade;
            classImage.GetComponent<Image>().color = new Color(0f, 1f, 0.2f);
            //print("Player " + playerNumber + "is assigned Soldier.");
        }
        else if (playerNumber == Rogue.GetComponent<rogueInput>()._playerID)
        {
            CurHealth = Rogue.GetComponent<rogueInput>()._curHealth;
            MaxHealth = Rogue.GetComponent<rogueInput>()._maxHealth;
            CurAmmo = Rogue.GetComponent<rogueInput>()._curClip;
            MaxAmmo = Rogue.GetComponent<rogueInput>()._curAmmo;
            classImage.GetComponent<Image>().sprite = roguePic;
            abilityImage1.GetComponent<Image>().sprite = Caltrops;
            abilityImage2.GetComponent<Image>().sprite = landMine;
            classImage.GetComponent<Image>().color = new Color(0.986f, 0f, 1f);
            //print("Player " + playerNumber + "is assigned Rogue.");
        }
        else if (playerNumber == Engineer.GetComponent<TechnicianInput>()._playerID)
        {
            CurHealth = Engineer.GetComponent<TechnicianInput>()._curHealth;
            MaxHealth = Engineer.GetComponent<TechnicianInput>()._maxHealth;
            CurAmmo = Engineer.GetComponent<TechnicianInput>()._curClip;
            MaxAmmo = Engineer.GetComponent<TechnicianInput>()._curAmmo;
            classImage.GetComponent<Image>().sprite = engineerPic;
            abilityImage1.GetComponent<Image>().sprite = turretImage;
            abilityImage2.GetComponent<Image>().sprite = repairImage;
            classImage.GetComponent<Image>().color = new Color(1f, 0.1529f, 0f);
            //print("Player " + playerNumber + "is assigned Technician.");
        }
        else
            this.gameObject.SetActive(false);

        health.SetFloat("_Fillpercentage", CurHealth / MaxHealth);
    }
    // Update is called once per frame
    void Update()
    {

        if (playerNumber == Tank.GetComponent<TankInput>()._playerID)
        {
            CurHealth = Tank.GetComponent<TankInput>()._curHealth;
            MaxHealth = Tank.GetComponent<TankInput>()._maxHealth;
            CurAmmo = Tank.GetComponent<TankInput>()._curClip;
            MaxAmmo = Tank.GetComponent<TankInput>()._curAmmo;
            AbilityOne.SetFloat("_Fillpercentage", Tank.GetComponent<TankInput>()._shieldCD);
            AbilityTwo.SetFloat("_Fillpercentage", Tank.GetComponent<TankInput>()._groundPoundCD / Tank.GetComponent<TankAbilities>()._shockwaveCD);
            
            if (Tank.GetComponent<TankInput>()._hasAmmo ^ Tank.GetComponent<TankInput>()._hasHealing)
            {
                if (Tank.GetComponent<TankInput>()._hasAmmo)
                {
                    ammoPickup.SetActive(true);
                    FAK.SetActive(false);
                }

                if (Tank.GetComponent<TankInput>()._hasHealing)
                {
                    FAK.SetActive(true);
                    ammoPickup.SetActive(false);
                }
            }
            else
            {
                FAK.SetActive(false);
                ammoPickup.SetActive(false);
            }
        }

        if (playerNumber == Soldier.GetComponent<SoldierInput>()._playerID)
        {
            CurHealth = Soldier.GetComponent<SoldierInput>()._curHealth;
            MaxHealth = Soldier.GetComponent<SoldierInput>()._maxHealth;
            CurAmmo = Soldier.GetComponent<SoldierInput>()._curClip;
            MaxAmmo = Soldier.GetComponent<SoldierInput>()._curAmmo;
            AbilityOne.SetFloat("_Fillpercentage", Soldier.GetComponent<SoldierInput>()._rapidFireCD / Soldier.GetComponent<SoldierAbilities>()._rapidFireCD);
            AbilityTwo.SetFloat("_Fillpercentage", Soldier.GetComponent<SoldierInput>()._grenadeCD / Soldier.GetComponent<SoldierAbilities>()._grenadeCD);
            
            if (Soldier.GetComponent<SoldierInput>()._hasAmmo ^ Soldier.GetComponent<SoldierInput>()._hasHealing)
            {
                if (Soldier.GetComponent<SoldierInput>()._hasAmmo)
                {
                    ammoPickup.SetActive(true);
                    FAK.SetActive(false);
                }

                if (Soldier.GetComponent<SoldierInput>()._hasHealing)
                {
                    FAK.SetActive(true);
                    ammoPickup.SetActive(false);
                }
            }
            else
            {
                FAK.SetActive(false);
                ammoPickup.SetActive(false);
            }
        }

        if (playerNumber == Rogue.GetComponent<rogueInput>()._playerID)
        {
            CurHealth = Rogue.GetComponent<rogueInput>()._curHealth;
            MaxHealth = Rogue.GetComponent<rogueInput>()._maxHealth;
            CurAmmo = Rogue.GetComponent<rogueInput>()._curClip;
            MaxAmmo = Rogue.GetComponent<rogueInput>()._curAmmo;
            AbilityOne.SetFloat("_Fillpercentage", Rogue.GetComponent<RogueAbilities>().spikeTimeRemaining / Rogue.GetComponent<RogueAbilities>().spikeCD);
            AbilityTwo.SetFloat("_Fillpercentage", Rogue.GetComponent<RogueAbilities>().mineTimeRemaining / Rogue.GetComponent<RogueAbilities>().mineCD);
            
            if (Rogue.GetComponent<rogueInput>()._hasAmmo ^ Rogue.GetComponent<rogueInput>()._hasHealing)
            {
                if (Rogue.GetComponent<rogueInput>()._hasAmmo)
                {
                    ammoPickup.SetActive(true);
                    FAK.SetActive(false);
                }

                if (Rogue.GetComponent<rogueInput>()._hasHealing)
                {
                    FAK.SetActive(true);
                    ammoPickup.SetActive(false);
                }
            }
            else
            {
                FAK.SetActive(false);
                ammoPickup.SetActive(false);
            }
        }

        if (playerNumber == Engineer.GetComponent<TechnicianInput>()._playerID)
        {
            CurHealth = Engineer.GetComponent<TechnicianInput>()._curHealth;
            MaxHealth = Engineer.GetComponent<TechnicianInput>()._maxHealth;
            CurAmmo = Engineer.GetComponent<TechnicianInput>()._curClip;
            MaxAmmo = Engineer.GetComponent<TechnicianInput>()._curAmmo;
            AbilityOne.SetFloat("_Fillpercentage", Engineer.GetComponent<TechnicianAbilities>().turretTimeRemaining / Engineer.GetComponent<TechnicianAbilities>().turretCD);
            AbilityTwo.SetFloat("_Fillpercentage", Engineer.GetComponent<TechnicianAbilities>().repairTimeRemaining / Engineer.GetComponent<TechnicianAbilities>().repairCD);
            
            if (Engineer.GetComponent<TechnicianInput>()._hasAmmo ^ Engineer.GetComponent<TechnicianInput>()._hasHealing)
            {
                if (Engineer.GetComponent<TechnicianInput>()._hasAmmo)
                {
                    ammoPickup.SetActive(true);
                    FAK.SetActive(false);
                }

                if (Engineer.GetComponent<TechnicianInput>()._hasHealing)
                {
                    FAK.SetActive(true);
                    ammoPickup.SetActive(false);
                }
            }
            else
            {
                FAK.SetActive(false);
                ammoPickup.SetActive(false);
            }
        }

        AmmoCounter.text = CurAmmo.ToString();
        AmmoReserve.text = MaxAmmo.ToString();

        health.SetFloat("_Fillpercentage", CurHealth / MaxHealth);
       
        if(CurHealth == 0)
        {
            this.gameObject.SetActive(false);
        }
        
    }
}
