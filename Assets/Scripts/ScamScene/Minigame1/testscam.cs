using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class testscam : MonoBehaviour
{

    public void OnClickkk()
    {
        Debug.Log("boom");

        //text.text = "clicked";
    }

    public void OnMouseOver()
    {
        Debug.Log("rat");
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("mouse");
        }
    }

}
