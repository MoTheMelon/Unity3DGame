using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumEditor : MonoBehaviour
{
    public static GameObject incButton;
    public static GameObject decButton;

    private ButtonBehavior incB = incButton.GetComponent<ButtonBehavior>();
    private ButtonBehavior decB = decButton.GetComponent<ButtonBehavior>();
    
    void Start()
    {
        
    }

    void Update()
    {
        Debug.Log(incB.pressed);
    }
}
