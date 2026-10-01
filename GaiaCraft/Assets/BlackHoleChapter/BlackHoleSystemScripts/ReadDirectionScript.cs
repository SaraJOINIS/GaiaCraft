using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ReadDirectionScript : MonoBehaviour
{
    // Animation globale de la scène
    public WholeBlackHoleSystemAnimation animationsControlScript;

    // Méthode appelée lorsque le toggle qui détermine le sens de lecture est cliqué
    public void UIToggleReadDirection()
    {
        animationsControlScript.ToggleReadDirection();

        if (animationsControlScript.GetIsForward())
            UIForward();
        else
            UIBackward();
    }

    private void UIForward()
    {
        TextMeshProUGUI buttonText = GetComponentInChildren<TextMeshProUGUI>();
        buttonText.text = "Forward";
    }

    private void UIBackward()
    {
        TextMeshProUGUI buttonText = GetComponentInChildren<TextMeshProUGUI>();
        buttonText.text = "Backward";
    }

    public void ResetToggle()
    {
        Slider slider = GetComponent<Slider>();

        if (slider != null)
        {
            slider.value = 1;
            UIForward();
        }
        else
            Debug.LogError("Slider NULL");
    }
}