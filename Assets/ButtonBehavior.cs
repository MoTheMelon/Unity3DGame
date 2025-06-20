using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonBehavior : MonoBehaviour
{
    public bool pressed = false;
    public float buttonDuration = 0.5f;
    private float buttonTimer = 0f;

    public void Press()
    {
        if (!pressed)
        {
            pressed = true;
            transform.localPosition -= new Vector3(0, 0.05f, 0);
            buttonTimer = buttonDuration;
        }
    }
    public void ResetButton()
    {
        pressed = false;
        transform.localPosition += new Vector3(0, 0.05f, 0);
    }
    void Update()
    {
        if (buttonTimer > 0f)
        {
            buttonTimer -= Time.deltaTime;
        }
        else
        {
            if (pressed)
            {
                ResetButton();
            }
        }
    }
}