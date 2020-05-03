using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour
{
    public GameObject mineExplodyPrefab;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.tag == "Enemy")
        {
            print("enemy detected");
            GameObject grenade = Instantiate(mineExplodyPrefab, transform.position, transform.rotation);
            Rigidbody rb = grenade.GetComponent<Rigidbody>();
            rb.AddForce(transform.up, ForceMode.VelocityChange);
        }
    }

    public void explode()
    {
        GameObject grenadeObj = Instantiate(mineExplodyPrefab, transform.position, transform.rotation);
        Rigidbody rb = grenadeObj.GetComponent<Rigidbody>();
        Vector3 upwardForce = transform.up;
        upwardForce.y *= 15f;
        rb.AddForce(upwardForce, ForceMode.VelocityChange);
    }
}
