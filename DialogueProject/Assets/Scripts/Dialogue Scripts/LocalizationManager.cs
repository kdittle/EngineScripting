using UnityEngine;
using System.Collections;
using System.Globalization;
using System.Collections.Generic;
using SmartLocalization;


public class LocalizationManager : MonoBehaviour
{
    private CultureInfo p_SystemCultureInfo;
    public List<string> m_NPCDialogue = new List<string>();
    public List<string> m_CharDialogue = new List<string>();
    private System.IO.StreamReader file;
    private string line;

	// Use this for initialization
	void Start ()
    {
        //get the current culture
        p_SystemCultureInfo = CultureInfo.CurrentCulture;

        //LanguageManager.Instance.ChangeLanguage("sv");

        //Load the dialogue
        //Start with NPC Dialogue file
        file = new System.IO.StreamReader(@"C:\Users\Kyle\Desktop\EngineScripting\DialogueProject\Assets\NPCDialogue.txt");

        while((line = file.ReadLine()) != null)
        {
            m_NPCDialogue.Add(LanguageManager.Instance.GetTextValue(line));
        }

        //Next to player dialogue
        file = new System.IO.StreamReader(@"C:\Users\Kyle\Desktop\EngineScripting\DialogueProject\Assets\PlayerDialogue.txt");

        while((line = file.ReadLine()) != null)
        {
            m_CharDialogue.Add(LanguageManager.Instance.GetTextValue(line));
        }

        //SmartCultureInfo swedishCulture = new SmartCultureInfo("sv", "Swedish", "Svenska", false);
        //Debug.Log(LanguageManager.Instance.IsLanguageSupported(swedishCulture));

    }
	
	// Update is called once per frame
	void Update ()
    {
	
	}
}
