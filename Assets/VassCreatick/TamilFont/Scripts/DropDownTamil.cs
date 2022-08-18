using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DropDownTamil : MonoBehaviour
{
    // Start is called before the first frame update
    public TMP_Text Tamiltext;
    public CharReplacerTamil _TamilDropdown = new CharReplacerTamil();

     
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void getTamiltext()
    {
        Tamiltext.text = _TamilDropdown.Convertedvalue;
    }

   

   
}
