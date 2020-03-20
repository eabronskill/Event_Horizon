using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Button_Click : MonoBehaviour
{
    private Button btn;
    public UnityEvent buttonLogic;
    
    void Start()
    {
        btn = gameObject.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick()
    {
        buttonLogic.Invoke();
    }
}
