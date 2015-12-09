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

	private static LocalizationManager s_Instance = null;
	public static LocalizationManager Instance
	{
		get
		{
			if(s_Instance == null)
			{
				s_Instance = FindObjectOfType(typeof(LocalizationManager)) as LocalizationManager;
			}
			return s_Instance;
		}
	}

	// Use this for initialization
	void Start ()
    {
        //get the current culture
        p_SystemCultureInfo = CultureInfo.CurrentCulture;

		//Automatically set the language to the default system language
        //LanguageManager.Instance.ChangeLanguage(p_SystemCultureInfo.ToString());

        //Load the dialogue
        //Start with NPC Dialogue file
		file = new System.IO.StreamReader (@"Assets/NPCDialogue.txt");	//Yay, no long file path

        while((line = file.ReadLine()) != null)
        {
            m_NPCDialogue.Add(LanguageManager.Instance.GetTextValue(line));
        }

        //Next to player dialogue
		file = new System.IO.StreamReader(@"Assets/PlayerDialogue.txt");	//Yay, another not so long file path

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
