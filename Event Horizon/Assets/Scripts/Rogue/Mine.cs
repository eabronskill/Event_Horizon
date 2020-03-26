using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour
{
    public GameObject grenadePrefab;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.tag == "Enemy")
        {
            print("enemy detected");
            GameObject grenade = Instantiate(grenadePrefab, transform.position, transform.rotation);
            Rigidbody rb = grenade.GetComponent<Rigidbody>();
            rb.AddForce(transform.up, ForceMode.VelocityChange);
        }
    }

    public void explode()
    {
        GameObject grenadeObj = Instantiate(grenadePrefab, transform.position, transform.rotation);
        Rigidbody rb = grenadeObj.GetComponent<Rigidbody>();
        Vector3 upwardForce = transform.up;
        upwardForce.y *= 15f;
        rb.AddForce(upwardForce, ForceMode.VelocityChange);
    }
}
