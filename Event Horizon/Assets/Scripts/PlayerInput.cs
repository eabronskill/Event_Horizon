using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PlayerInput : MonoBehaviour
{
    public float m_speed = 1f;

    private Vector3 startPos;

    public int startHP = 100;
    public int currHP;

    public GameObject HPtext;
    public GameObject strap;

    public GameObject projectile;
    public GameObject attackPoint;

    public GameObject wall;


    private Plane plane;
    private bool canJump;

    private float timer;


    public Quaternion strapRot;
    public Quaternion finalRot;

    public AudioSource gunshot;
    public AudioSource music;

    public ParticleSystem gunFlash;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        music.volume = .4f;
        music.Play();

        currHP = startHP;
        HPtext.GetComponent<Text>().text = currHP.ToString() + " HP";

        strap.transform.position.Set(transform.position.x + (float)1.19, transform.position.y + (float)1.59, transform.position.z + (float)1.1);
        //strap.transform.rotation.Set(0f, 0f, 0f, 0f); //should fix recoil
        attackPoint.transform.position.Set(transform.position.x + (float)1.19, transform.position.y + (float)2.101, transform.position.z + (float)3);
        GetComponent<Rigidbody>().freezeRotation = true;

        plane = new Plane(Vector3.up, transform.position);
        canJump = true;

        timer = Time.time;
        strapRot = strap.transform.localRotation;
        finalRot = strapRot;
        finalRot.x -= .6f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        movement();
        aiming();

        if (Input.GetKeyDown("space") && canJump)
            jump();
        if (Input.GetKeyDown("v") && canJump)
            dash();

        if (timer > Time.time)
        {
            strapRecoil(strapRot, finalRot);
        }
        else if (strap.transform.localRotation.x != 0)
        {
            strapRotateBack(strap.transform.localRotation, strapRot);
        }
    }

    private void movement()
    {
        Vector3 movementVec = Vector3.zero;

        movementVec.x = Input.GetAxis("Horizontal");
        movementVec.z = Input.GetAxis("Vertical");
        //movementVec.y = 0.5f * -9.81f;
        GetComponent<Rigidbody>().AddForce(movementVec * m_speed);
    }

    private void jump()
    {
        canJump = false;
        Invoke("jumpTimer", 1.8f);
        for (int i = 0; i < 6; i++)
        {
            Vector3 movementVec = Vector3.zero;
            movementVec.y = 200f;

            GetComponent<Rigidbody>().AddForce(movementVec);
        }
    }

    private void dash()
    {
        canJump = false;
        Invoke("jumpTimer", 1.8f);

        Vector3 movementVec = Vector3.zero;
        movementVec.x = Input.GetAxis("Horizontal");
        movementVec.z = Input.GetAxis("Vertical");

        for (int i = 0; i < 6; i++)
        {
            GetComponent<Rigidbody>().AddForce(movementVec * m_speed * 5);
        }
    }

    private void jumpTimer()
    {
        canJump = true;
    }

    private void aiming()
    {
        if (Input.GetMouseButtonDown(0) && Time.time >= timer)
        {
            timer = Time.time + 0.6f;
            Instantiate(projectile, attackPoint.transform.position, attackPoint.transform.rotation);
            gunshot.Play();
            gunFlash.Play();
        }

        Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        plane.SetNormalAndPosition(Vector3.up, transform.position);

        float intersectionDistance = 0f;

        if (plane.Raycast(cameraRay, out intersectionDistance))
        {
            Vector3 hitPoint = cameraRay.GetPoint(intersectionDistance);
            transform.LookAt(hitPoint);
        }
    }

    private void strapRecoil(Quaternion initRot, Quaternion finalRot)
    {
        
        strap.transform.localRotation = Quaternion.Lerp(initRot, finalRot, timer-Time.time);
    }

    private void strapRotateBack(Quaternion initRot, Quaternion finalRot)
    {
        //strap.transform.localRotation = Quaternion.Lerp(initRot, finalRot, timer - Time.time);
        strap.transform.localRotation = Quaternion.Lerp(initRot, finalRot, .1f);
    }

    private void OnCollisionEnter(Collision coll)
    {
        if (coll.collider.tag == "Button")
            Destroy(wall);

        if (coll.collider.tag == "LevelEnd")
        {
            Invoke("resetCharacter", 2);
        }
    }


    public void resetCharacter()
    {
        //currHP = startHP;
        //transform.position = startPos;
        //rstEvent.Invoke();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void takeDamage()
    {
        currHP -= 10;

        if (currHP <= 0)
            Invoke("resetCharacter", 0.5f);

        HPtext.GetComponent<Text>().text = currHP.ToString() + " HP";
    }
}
