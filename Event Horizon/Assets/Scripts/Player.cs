using UnityEngine;

public class Player : MonoBehaviour
{
    // Stat vars
    public float movementSpeed;
    public float armor;
    public float curHealth;
    public float maxHealth;
    public float curAmmo;
    public float maxAmmo;

    // Aiming vars
    private Plane mousePlane;
    private Ray cameraRay;
    private float intersectionDistance = 0f;

    // Item vars
    [HideInInspector]
    public Ammo ammoItem;
    [HideInInspector]
    public Healing healingItem;
    [HideInInspector]
    public bool hasAmmo = false;
    [HideInInspector]
    public bool hasHealing = false;
    private float bufferTimer = 0f;


    public void FixedUpdate()
    {
        // Movement
        Vector3 movementVec = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        GetComponent<Rigidbody>().AddForce(movementVec * movementSpeed);

        // Aiming 
        cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        mousePlane.SetNormalAndPosition(Vector3.up, transform.position);
        if (mousePlane.Raycast(cameraRay, out intersectionDistance))
        {
            Vector3 hitPoint = cameraRay.GetPoint(intersectionDistance);
            transform.LookAt(hitPoint);
        }

        // Items
        if (hasAmmo && Input.GetKey(KeyCode.F))
        {
            ammoItem.use();
            hasAmmo = false;
        }
        if (hasHealing && Input.GetKey(KeyCode.F))
        {
            healingItem.use();
            hasHealing = false;
        }
    }

    public void OnTriggerStay(Collider other)
    {
        if (bufferTimer < Time.time)
        {
            if (other.tag == "Ammo")
            {
                print("Ammo can be picked up");
                if (Input.GetKey(KeyCode.E) && !(hasAmmo || hasHealing))
                {
                    ammoItem = other.GetComponent<Ammo>();
                    ammoItem.player = this;
                    ammoItem.gameObject.SetActive(false);
                    hasAmmo = true;
                    bufferTimer = Time.time + 1f;
                }
                else if (Input.GetKey(KeyCode.E) && hasHealing)
                {
                    healingItem.gameObject.SetActive(true);
                    healingItem.transform.position = new Vector3(transform.position.x, transform.position.y + 5f, transform.position.z);
                    healingItem.player = null;
                    healingItem = null;
                    hasHealing = false;


                    ammoItem = other.GetComponent<Ammo>();
                    ammoItem.player = this;
                    ammoItem.gameObject.SetActive(false);
                    hasAmmo = true;
                    bufferTimer = Time.time + 1f;
                }

            }
            if (other.tag == "Healing")
            {

                if (Input.GetKey(KeyCode.E) && !(hasAmmo || hasHealing))
                {
                    healingItem = other.GetComponent<Healing>();
                    healingItem.player = this;
                    healingItem.gameObject.SetActive(false);
                    hasHealing = true;
                    bufferTimer = Time.time + 1f;
                }
                else if (Input.GetKey(KeyCode.E) && hasAmmo)
                {
                    ammoItem.gameObject.SetActive(true);
                    ammoItem.transform.position = new Vector3(transform.position.x, transform.position.y + 5f, transform.position.z);
                    ammoItem.player = null;
                    ammoItem = null;
                    hasAmmo = false;

                    healingItem = other.GetComponent<Healing>();
                    healingItem.player = this;
                    healingItem.gameObject.SetActive(false);
                    hasHealing = true;
                    bufferTimer = Time.time + 1f;
                }
            }
        }
    }
}
