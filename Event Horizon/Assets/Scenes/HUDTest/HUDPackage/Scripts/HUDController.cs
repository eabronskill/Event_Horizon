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
    public int playerNumber;
    //Class Images
    public Sprite tankPic;
    public Sprite soldierPic;
    public Sprite roguePic;
    public Sprite engineerPic;


    // Start is called before the first frame update
    void Start()
    {
        if (playerNumber == Tank.GetComponent<TankInput>().playerID)
        {
            CurHealth = Tank.GetComponent<TankInput>().curHealth;
            MaxHealth = Tank.GetComponent<TankInput>().maxHealth;
            CurAmmo = Tank.GetComponent<TankInput>().curClip;
            MaxAmmo = Tank.GetComponent<TankInput>().curAmmo;
            classImage.GetComponent<Image>().sprite = tankPic;

        }

        if (playerNumber == Soldier.GetComponent<SoldierInput>().playerID)
        {
            CurHealth = Soldier.GetComponent<SoldierInput>().curHealth;
            MaxHealth = Soldier.GetComponent<SoldierInput>().maxHealth;
            CurAmmo = Soldier.GetComponent<SoldierInput>().curClip;
            MaxAmmo = Soldier.GetComponent<SoldierInput>().curAmmo;
            classImage.GetComponent<Image>().sprite = soldierPic;
        }

        /*if (playerNumber == Rogue.GetComponent<PlayerAttributes>().playerID)
        {
            CurHealth = Rogue.GetComponent<RogueInput>().curHealth;
            MaxHealth = Rogue.GetComponent<RogueInput>().maxHealth;
            CurAmmo = Rogue.GetComponent<RogueInput>().curClip;
            MaxAmmo = Rogue.GetComponent<RogueInput>().curAmmo;
            classImage.GetComponent<Image>().sprite = roguePic;
        }

        if (playerNumber == Engineer.GetComponent<PlayerAttributes>().playerID)
        {
            CurHealth = Engineer.GetComponent<EngineerInput>().curHealth;
            MaxHealth = Engineer.GetComponent<EngineerInput>().maxHealth;
            CurAmmo = Engineer.GetComponent<EngineerInput>().curClip;
            MaxAmmo = Engineer.GetComponent<EngineerInput>().curAmmo;
            classImage.GetComponent<Image>().sprite = engineerPic; 
        }*/
        
        health.SetFloat("_Fillpercentage", CurHealth / MaxHealth);
        
    }

    // Update is called once per frame
    void Update()
    {

        if (playerNumber == Tank.GetComponent<TankInput>().playerID)
        {
            CurHealth = Tank.GetComponent<TankInput>().curHealth;
            MaxHealth = Tank.GetComponent<TankInput>().maxHealth;
            CurAmmo = Tank.GetComponent<TankInput>().curClip;
            MaxAmmo = Tank.GetComponent<TankInput>().curAmmo;
            /*AbilityOne.SetFloat("_Fillpercentage", Tank.GetComponent<PlayerAttributes>().abilityCooldown1 / Tank.GetComponent<PlayerAttributes>().maxCooldown1);
            AbilityTwo.SetFloat("_Fillpercentage", Tank.GetComponent<PlayerAttributes>().abilityCooldown2 / Tank.GetComponent<PlayerAttributes>().maxCooldown2); */

            if (Tank.GetComponent<TankInput>().hasAmmo ^ Tank.GetComponent<TankInput>().hasHealing)
            {
                if (Tank.GetComponent<TankInput>().hasAmmo)
                {
                    ammoPickup.SetActive(true);
                    FAK.SetActive(false);
                }

                if (Tank.GetComponent<TankInput>().hasHealing)
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

        if (playerNumber == Soldier.GetComponent<SoldierInput>().playerID)
        {
            CurHealth = Soldier.GetComponent<SoldierInput>().curHealth;
            MaxHealth = Soldier.GetComponent<SoldierInput>().maxHealth;
            CurAmmo = Soldier.GetComponent<SoldierInput>().curClip;
            MaxAmmo = Soldier.GetComponent<SoldierInput>().curAmmo;
            /*AbilityOne.SetFloat("_Fillpercentage", Soldier.GetComponent<PlayerAttributes>().abilityCooldown1 / Soldier.GetComponent<PlayerAttributes>().maxCooldown1);
            AbilityTwo.SetFloat("_Fillpercentage", Soldier.GetComponent<PlayerAttributes>().abilityCooldown2 / Soldier.GetComponent<PlayerAttributes>().maxCooldown2);*/

            if (Soldier.GetComponent<SoldierInput>().hasAmmo ^ Soldier.GetComponent<SoldierInput>().hasHealing)
            {
                if (Soldier.GetComponent<SoldierInput>().hasAmmo)
                {
                    ammoPickup.SetActive(true);
                    FAK.SetActive(false);
                }

                if (Soldier.GetComponent<SoldierInput>().hasHealing)
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

        /*if (playerNumber == Rogue.GetComponent<PlayerAttributes>().playerID)
        {
            CurHealth = Rogue.GetComponent<RogueInput>().curHealth;
            MaxHealth = Rogue.GetComponent<RogueInput>().maxHealth;
            CurAmmo = Rogue.GetComponent<RogueInput>().curClip;
            MaxAmmo = Rogue.GetComponent<RogueInput>().curAmmo;
            AbilityOne.SetFloat("_Fillpercentage", Rogue.GetComponent<PlayerAttributes>().abilityCooldown1 / Rogue.GetComponent<PlayerAttributes>().maxCooldown1);
            AbilityTwo.SetFloat("_Fillpercentage", Rogue.GetComponent<PlayerAttributes>().abilityCooldown2 / Rogue.GetComponent<PlayerAttributes>().maxCooldown2);

            if (Rogue.GetComponent<RogueInput>().hasAmmo ^ Rogue.GetComponent<RogueInput>().hasHealing)
            {
                if (Rogue.GetComponent<RogueInput>().hasAmmo)
                {
                    ammoPickup.SetActive(true);
                    FAK.SetActive(false);
                }

                if (Rogue.GetComponent<RogueInput>().hasHealing)
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
        }*/

        /*if (playerNumber == Engineer.GetComponent<EngineerInput>().playerID)
        {
            CurHealth = Engineer.GetComponent<EngineerInput>().curHealth;
            MaxHealth = Engineer.GetComponent<EngineerInput>().maxHealth;
            CurAmmo = Engineer.GetComponent<EngineerInput>().curClip;
            MaxAmmo = Engineer.GetComponent<EngineerInput>().curAmmo;
            AbilityOne.SetFloat("_Fillpercentage", Engineer.GetComponent<PlayerAttributes>().abilityCooldown1 / Engineer.GetComponent<PlayerAttributes>().maxCooldown1);
            AbilityTwo.SetFloat("_Fillpercentage", Engineer.GetComponent<PlayerAttributes>().abilityCooldown2 / Engineer.GetComponent<PlayerAttributes>().maxCooldown2);
            
            if (Engineer.GetComponent<EngineerInput>().hasAmmo ^ Engineer.GetComponent<EngineerInput>().hasHealing)
            {
                if (Engineer.GetComponent<EngineerInput>().hasAmmo)
                {
                    ammoPickup.SetActive(true);
                    FAK.SetActive(false);
                }

                if (Engineer.GetComponent<EngineerInput>().Healing)
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
        }*/

        AmmoCounter.text = CurAmmo.ToString();
        AmmoReserve.text = MaxAmmo.ToString();

        health.SetFloat("_Fillpercentage", CurHealth / MaxHealth);
       
        
    }
}
