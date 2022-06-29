using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ScamInstructions : MonoBehaviour
{
    [SerializeField]
    public Text instructionScoreText;

    private bool toggleText = false;

    public void ToggleText()
    {
        if (toggleText)
        {
            toggleText = !toggleText;
            instructionScoreText.text = "- 10";
        }
        else
        {
            toggleText = !toggleText;
            instructionScoreText.text = "+ 10";
        }
    }
}
