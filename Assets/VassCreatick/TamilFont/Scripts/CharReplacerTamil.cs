using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using System.IO;
using Newtonsoft.Json;

public class CharReplacerTamil : MonoBehaviour
{
   
    List<CharacterAttributes> _CharacterAttributes = new List<CharacterAttributes>();
    private TMP_Text _Text;
    private TMP_InputField _InputField;
    private TMP_Dropdown _DropDown;
    private string FontFile = "TamilReplacement";
    public string Convertedvalue = "";
    public string OriginalText = "";
    bool FirstTime;
    void Start()
    {
        LoadAttributes();
       _Text = GetComponent<TMP_Text>();
       _InputField = GetComponent<TMP_InputField>();
        _DropDown = GetComponent<TMP_Dropdown>();
        UpdateMe();
    }

    void OnEnable()
    {
        if (_Text == null) _Text = GetComponent<TMP_Text>();
        if (_InputField == null) _InputField = GetComponent<TMP_InputField>();
        if (_DropDown == null) _DropDown = GetComponent<TMP_Dropdown>();
    }

    void Update()
    {
       
    }

    

   
    void LoadAttributes()
    {
        TextAsset textAsset = Resources.Load<TextAsset>(FontFile);
        if (textAsset != null)
        {
            _CharacterAttributes = JsonConvert.DeserializeObject<List<CharacterAttributes>>(textAsset.text);

        }

    }


    public void UpdateMe()
    {
        string Value="";
       
        if (_Text != null)
        {
            Value = _Text.text;
            OriginalText = _Text.text;
            //Debug.Log("Original Text " + OriginalText);
        }
        else if(_InputField!=null)
        {
          Value = _InputField.text;
        }
        else if (_DropDown != null)
        {
            Value = _DropDown.captionText.text;
           
        }

        for (int i = 0; i < _CharacterAttributes.Count; i++)
        {
            if (_CharacterAttributes.Count > 0)
            {
                if (_CharacterAttributes[i].CharacterHexValue == "")
                {
                   
                    for (int j = 0; j < _CharacterAttributes[i].CharacterCombinations.Count; j++)
                    {
                        if (Value.Contains(_CharacterAttributes[i].CharacterCombinations[j]))
                        {
                            Value = Value.Replace(_CharacterAttributes[i].CharacterCombinations[j], _CharacterAttributes[i].CharName);
                          
                        }
                    }
                }
                else
                {
                  
                    int decValue = Convert.ToInt32(_CharacterAttributes[i].CharacterHexValue, 16);
                    string Converted = Convert.ToChar(decValue).ToString();
                    for (int j = 0; j < _CharacterAttributes[i].CharacterCombinations.Count; j++)
                    {
                        
                        Value = Value.Replace(_CharacterAttributes[i].CharacterCombinations[j], @Converted);
                       
                    }
                }
            }
           
        }
        if (_Text != null)
        {
            _Text.text=Value;
         //   Debug.Log(" _Text.text " + _Text.text);
            Convertedvalue = Value;

        }
        if (_InputField != null)
        {
            _InputField.text = Value;
             Convertedvalue = Value;
        }

        if (_DropDown != null)
        {
            _DropDown.captionText.text = Value;
            Convertedvalue = Value;
        }


    }
   
    
    }

[Serializable]
public class CharacterAttributes
{
    public string Character;
    public List<string> CharacterCombinations;
    public string CharacterHexValue;
    public string CharName;
 }
