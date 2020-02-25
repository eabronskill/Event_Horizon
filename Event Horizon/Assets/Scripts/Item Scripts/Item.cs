using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Item : MonoBehaviour
{
    public UnityEvent use;
    private Player player;
    public string name;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void useItem()
    {
        use.Invoke();
    }

    public void pickupItem()
    {
        gameObject.SetActive(false);
    }

    public void dropItem()
    {
        //TODO
    }

    void OnTriggerStay()
    {

    }

    void OnTriggerEnter()
    {

    }

    void OnTriggerExit()
    {

    }
}
