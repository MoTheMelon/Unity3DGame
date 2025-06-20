using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonBehavior : MonoBehaviour
{
    public bool pressed = false;

    public void Press()
    {
        if (!pressed)
        {
            pressed = true;
            Debug.Log(gameObject.name + " was pressed!");
            // Do stuff: animate, sound, etc.
        }
    }
}