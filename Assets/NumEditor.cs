using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NumEditor : MonoBehaviour
{
    public ButtonBehavior increaseButton;
    public ButtonBehavior decreaseButton;
    public TextMeshProUGUI numberText;

    private int currentValue = 0;

    private bool increaseHandled = false;
    private bool decreaseHandled = false;

    void Update()
    {
        if (increaseButton != null && decreaseButton != null)
        {
            // Increase logic
            if (increaseButton.pressed && !increaseHandled)
            {
                currentValue++;
                UpdateText();
                increaseHandled = true;
            }
            else if (!increaseButton.pressed)
            {
                increaseHandled = false;
            }

            // Decrease logic
            if (decreaseButton.pressed && !decreaseHandled)
            {
                currentValue--;
                UpdateText();
                decreaseHandled = true;
            }
            else if (!decreaseButton.pressed)
            {
                decreaseHandled = false;
            }
        }
    }

    void UpdateText()
    {
        numberText.text = currentValue.ToString();
    }
}
