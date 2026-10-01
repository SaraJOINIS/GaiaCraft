using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayPauseButtonScript : MonoBehaviour
{
    // Animation globale de la scène
    public WholeBlackHoleSystemAnimation animationsControlScript;

    // Méthode appelée lorsque le bouton Play/Pause est cliqué
    public void UITogglePlayPause()
    {
        animationsControlScript.TogglePlayPause();

        if (animationsControlScript.GetIsPlaying())
            UIPlay();
        else
            UIPause();
    }

    private void UIPlay()
    {
        TextMeshProUGUI buttonText = GetComponentInChildren<TextMeshProUGUI>();
        buttonText.text = "Play";
    }

    private void UIPause()
    {
        TextMeshProUGUI buttonText = GetComponentInChildren<TextMeshProUGUI>();
        buttonText.text = "Pause";
    }

}