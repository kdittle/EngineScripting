using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class TriggerZone : MonoBehaviour
{
	[System.Serializable]
	public class DialogueInfo
	{
		string m_dialogText;
		AudioClip m_dialogAudio;
		bool m_skippable;
	};

	public DialogueInfo[] m_dialogueEnteries;

    public Canvas _worldCanvas;

    public List<string> NPCDialogue;
    public List<string> CharDialogue;

    private Text DialogueTXT;

    private int _curNPCDialogue;
    private int _curCharDialogue;

    private bool _NPCTalking;
    private bool _CharTalking;
    private GameObject m_NPCObject;
    private GameObject m_PlayerObject;
	private bool _bConversationFinished;

    // Use this for initialization
    void Start()
    {
		_bConversationFinished = false;
        //SmartLocalization.LanguageManager.Instance.ChangeLanguage("sv");

        //Add NPC dialogue
        foreach (string s in LocalizationManager.Instance.m_NPCDialogue)
        {
            NPCDialogue.Add(s);
        }

        //Add Char dialogue
        foreach (string s in LocalizationManager.Instance.m_CharDialogue)
        {
            CharDialogue.Add(s);
        }

        //Find the two canvas objects
        //Canvas objects should be turned off by default in editor so they don't appear on screen

        //Find the NPC that's going to be talking
        m_NPCObject = GameObject.FindGameObjectWithTag("AI");

        //Set the current NPCDialogue to 0, so the first sentence may be displayed
        _curNPCDialogue = 0;
        _curCharDialogue = 0;
        _NPCTalking = true;
        _CharTalking = false;

        //Debug.Log("m_NPCObject");
    }

    // Update is called once per frame
    void Update()
    {
        //Rotate the hovering canvas such that the text is always readable 
        //This should probably be changed so it doesn't have to find the object every update.
        if (_worldCanvas.enabled)
            _worldCanvas.gameObject.transform.rotation = GameObject.FindGameObjectWithTag("MainCamera").gameObject.transform.rotation;

        //Get input to change NPCDialogue
        //Should be handled in a player script and use messaging to change NPCDialogue. Maybe?
        if ((_curCharDialogue < CharDialogue.Count && Input.GetKeyDown(KeyCode.Space)) || (_curNPCDialogue < NPCDialogue.Count) && Input.GetKeyDown(KeyCode.Space))
        {
            SwitchDialogue();
            m_NPCObject.gameObject.GetComponent<AudioSource>().Play();
        }

		if (_bConversationFinished)
			m_NPCObject.GetComponent<NPCMovement> ()._NPCState = NPCMovement.NPCState.Walking;
    }

    void OnTriggerEnter(Collider other)
    {

        //Check to see if it's the player that entered the zone
        if (other.gameObject.tag == "Player")
        {
			if(!_bConversationFinished)
			{
            	other.GetComponent<PlayerMovementScript>().m_PlayerState = PlayerMovementScript.PState.Talking;

            	//Spawn the NPCDialogue box above the AI character
            	_worldCanvas.gameObject.transform.position = new Vector3(m_NPCObject.transform.position.x, m_NPCObject.transform.position.y + 2, m_NPCObject.transform.position.z);

            	//enable the proper canvas
            	_worldCanvas.enabled = true;
            	DialogueTXT = _worldCanvas.GetComponentInChildren<Text>();
            	SwitchDialogue();
            	m_NPCObject.gameObject.GetComponent<AudioSource>().Play();
			}
        }
    }

    ////This isn't really being used.
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (!_NPCTalking)
            {
                if (_curNPCDialogue <= NPCDialogue.Count)
                {
                    _worldCanvas.gameObject.transform.position = new Vector3(m_NPCObject.transform.position.x, m_NPCObject.transform.position.y + 2, m_NPCObject.transform.position.z);
                }
            }

            //If the character is talking, display the character text
            if (!_CharTalking)
            {
                if (_curCharDialogue <= CharDialogue.Count)
                {
                    _worldCanvas.gameObject.transform.position = new Vector3(other.transform.position.x, other.transform.position.y + 2, other.transform.position.z);
                }
            }

            //Really bad solution for displaying the final exchange of dialgoue
            //Works for now.
            if (_curNPCDialogue >= NPCDialogue.Count && _curCharDialogue >= CharDialogue.Count)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    if (_worldCanvas.enabled)
                        _worldCanvas.enabled = false;
                }
            }
        }

        if (!_worldCanvas.enabled) 
		{
			other.GetComponent<PlayerMovementScript> ().m_PlayerState = PlayerMovementScript.PState.Idle;
			_bConversationFinished = true;
		}
    }

    //When player exits
    void OnTriggerExit(Collider other)
    {
        //turn off the chat stuff.
        if (other.gameObject.tag == "Player")
        {
            _worldCanvas.enabled = false;
        }
    }

    private void SwitchDialogue()
    {
        //If the NPC is talking, display the NPC text
        if (_NPCTalking)
		{
            if (_curNPCDialogue < NPCDialogue.Count)
            {
                DialogueTXT.text = NPCDialogue[_curNPCDialogue].ToString();
            }

            //Prepare the next string of dialogue to be shown
            _curNPCDialogue++;
        }

        //If the character is talking, display the character text
        if (_CharTalking)
        {
            if (_curCharDialogue < CharDialogue.Count)
            {
                DialogueTXT.text = CharDialogue[_curCharDialogue].ToString();
            }

            //Prepare the next string of dialgoue to be shown
            _curCharDialogue++;
        }

        //Switch between who is talking
        if (_NPCTalking)
        {
            _NPCTalking = false;
            _CharTalking = true;
        }
        else
        {
            _CharTalking = false;
            _NPCTalking = true;
        }
    }
}
