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
        if (playerNumber == Tank.GetComponent<TankInput>().playerID)
        {
            CurHealth = Tank.GetComponent<TankInput>().curHealth;
            MaxHealth = Tank.GetComponent<TankInput>().maxHealth;
            CurAmmo = Tank.GetComponent<TankInput>().curClip;
            MaxAmmo = Tank.GetComponent<TankInput>().curAmmo;
            classImage.GetComponent<Image>().sprite = tankPic;
            abilityImage1.GetComponent<Image>().sprite = shieldImage;
            abilityImage2.GetComponent<Image>().sprite = groundPound;

        }

        if (playerNumber == Soldier.GetComponent<SoldierInput>().playerID)
        {
            CurHealth = Soldier.GetComponent<SoldierInput>().curHealth;
            MaxHealth = Soldier.GetComponent<SoldierInput>().maxHealth;
            CurAmmo = Soldier.GetComponent<SoldierInput>().curClip;
            MaxAmmo = Soldier.GetComponent<SoldierInput>().curAmmo;
            classImage.GetComponent<Image>().sprite = soldierPic;
            abilityImage1.GetComponent<Image>().sprite = rapidFire;
            abilityImage2.GetComponent<Image>().sprite = Grenade;
        }

        /*if (playerNumber == Rogue.GetComponent<PlayerAttributes>().playerID)
        {
            CurHealth = Rogue.GetComponent<RogueInput>().curHealth;
            MaxHealth = Rogue.GetComponent<RogueInput>().maxHealth;
            CurAmmo = Rogue.GetComponent<RogueInput>().curClip;
            MaxAmmo = Rogue.GetComponent<RogueInput>().curAmmo;
            classImage.GetComponent<Image>().sprite = roguePic;
            abilityImage1.GetComponent<Image>().sprite = Caltrops;
            abilityImage2.GetComponent<Image>().sprite = landMine;
        }

        if (playerNumber == Engineer.GetComponent<PlayerAttributes>().playerID)
        {
            CurHealth = Engineer.GetComponent<EngineerInput>().curHealth;
            MaxHealth = Engineer.GetComponent<EngineerInput>().maxHealth;
            CurAmmo = Engineer.GetComponent<EngineerInput>().curClip;
            MaxAmmo = Engineer.GetComponent<EngineerInput>().curAmmo;
            classImage.GetComponent<Image>().sprite = engineerPic;
            abilityImage1.GetComponent<Image>().sprite = turretImage;
            abilityImage2.GetComponent<Image>().sprite = repairImage;
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
            AbilityOne.SetFloat("_Fillpercentage", Tank.GetComponent<TankInput>().shieldCD);
            AbilityTwo.SetFloat("_Fillpercentage", Tank.GetComponent<TankInput>().groundPoundCD / Tank.GetComponent<TankAbilities>().groundPoundCD); 

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
            AbilityOne.SetFloat("_Fillpercentage", Soldier.GetComponent<SoldierInput>().rapidFireCD / Soldier.GetComponent<SoldierAbilities>().rapidFireCD);
            AbilityTwo.SetFloat("_Fillpercentage", Soldier.GetComponent<SoldierInput>().grenadeCD / Soldier.GetComponent<SoldierAbilities>().grenadeCD);

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
            AbilityOne.SetFloat("_Fillpercentage", Rogue.GetComponent<RogueAbilities>().spikeCooldown / Rogue.GetComponent<PlayerAttributes>().maxCooldown1);
            AbilityTwo.SetFloat("_Fillpercentage", Rogue.GetComponent<RogueAbilities>().mineCooldowns / Rogue.GetComponent<PlayerAttributes>().maxCooldown2);

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
            AbilityOne.SetFloat("_Fillpercentage", Engineer.GetComponent<PlayerAttributes>().turretCooldown / Engineer.GetComponent<PlayerAttributes>().maxCooldown1);
            AbilityTwo.SetFloat("_Fillpercentage", Engineer.GetComponent<PlayerAttributes>().repairCooldown / Engineer.GetComponent<PlayerAttributes>().maxCooldown2);
            
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
