using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetButtonScript : MonoBehaviour
{
    // Animation globale de la scène
    public WholeBlackHoleSystemAnimation animationsControlScript;

    // Méthode appelée lorsque le bouton Reset est cliqué
    public void UIReset()
    {
        animationsControlScript.Reset();
    }

}