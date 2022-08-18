using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class DemoTamil : MonoBehaviour
{
    // Start is called before the first frame update
    public TMP_Text Tamil;
    public  CharReplacerTamil TamilInput = new CharReplacerTamil();
    public CharReplacerTamil _Text = new CharReplacerTamil();//Use this for Getting text from One text to another
    public TMP_Text GetTexts; //Display the text from _Text to this text;

   


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //Getting Text from InputField to Text
    public void GetInputText()
    {
        Tamil.text = TamilInput.Convertedvalue;
        
    }
      

    //Getting OriginalText from TextComponent attached in CharReplacerTamil Script
    public void GetOriginalText()
    {
        _Text.UpdateMe();
        string originaltext = _Text.OriginalText;
       
    }
    //Getting Texts From One Text(Tmp) to Another Text (Tmp)
    public void GetTextToAnother()
    {
        _Text.UpdateMe();
        GetTexts.text = _Text.Convertedvalue;
    }
}
