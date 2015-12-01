using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class TriggerZone : MonoBehaviour
{
    private Canvas _worldCanvas;

    public List<string> NPCDialogue;
    public List<string> CharDialogue;

    private LocalizationManager LM;

    private Text DialogueTXT;

    private int _curNPCDialogue;
    private int _curCharDialogue;

    private bool _NPCTalking;
    private bool _CharTalking;
    private GameObject m_NPCObject;
    private GameObject m_PlayerObject;


    // Use this for initialization
    void Start()
    {
        SmartLocalization.LanguageManager.Instance.ChangeLanguage("sv");

        //Get the LocalizationManager
        LM = GameObject.FindGameObjectWithTag("Manager").GetComponent<LocalizationManager>();

        //Add NPC dialogue
        foreach (string s in LM.m_NPCDialogue)
        {
            NPCDialogue.Add(s);
        }

        //Add Char dialogue
        foreach (string s in LM.m_CharDialogue)
        {
            CharDialogue.Add(s);
        }

        //Find the two canvas objects
        //Canvas objects should be turned off by default in editor so they don't appear on screen
        _worldCanvas = GameObject.FindGameObjectWithTag("WPDialogueCanvas").GetComponent<Canvas>();

        //Find the NPC that's going to be talking
        m_NPCObject = GameObject.FindGameObjectWithTag("AI");

        //Find the player
        m_PlayerObject = GameObject.FindGameObjectWithTag("Player");

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
        if ((_curCharDialogue < CharDialogue.Capacity && Input.GetKeyDown(KeyCode.Space)) || (_curNPCDialogue < NPCDialogue.Capacity) && Input.GetKeyDown(KeyCode.Space))
        {
            SwitchDialogue();
            m_NPCObject.gameObject.GetComponent<AudioSource>().Play();
        }
    }

    void OnTriggerEnter(Collider other)
    {

        //Check to see if it's the player that entered the zone
        if (other.gameObject.tag == "Player")
        {
            m_PlayerObject.GetComponent<PlayerMovementScript>().m_PlayerState = PlayerMovementScript.PState.Talking;

            _curNPCDialogue = 0;
            _curCharDialogue = 0;

            //Spawn the NPCDialogue box above the AI character
            _worldCanvas.gameObject.transform.position = new Vector3(m_NPCObject.transform.position.x, m_NPCObject.transform.position.y + 2, m_NPCObject.transform.position.z);

            //enable the proper canvas
            _worldCanvas.enabled = true;
            DialogueTXT = _worldCanvas.GetComponentInChildren<Text>();
            SwitchDialogue();
            m_NPCObject.gameObject.GetComponent<AudioSource>().Play();
        }
    }

    ////This isn't really being used.
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (!_NPCTalking)
            {
                if (_curNPCDialogue <= NPCDialogue.Capacity)
                {
                    _worldCanvas.gameObject.transform.position = new Vector3(m_NPCObject.transform.position.x, m_NPCObject.transform.position.y + 2, m_NPCObject.transform.position.z);
                }
            }

            //If the character is talking, display the character text
            if (!_CharTalking)
            {
                if (_curCharDialogue <= CharDialogue.Capacity)
                {
                    _worldCanvas.gameObject.transform.position = new Vector3(m_PlayerObject.transform.position.x, m_PlayerObject.transform.position.y + 2, m_PlayerObject.transform.position.z);
                }
            }

            //Really bad solution for displaying the final exchange of dialgoue
            //Works for now.
            if (_curNPCDialogue >= NPCDialogue.Capacity && _curCharDialogue >= CharDialogue.Capacity)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    if (_worldCanvas.enabled)
                        _worldCanvas.enabled = false;
                }
            }
        }

        if(!_worldCanvas.enabled)
            m_PlayerObject.GetComponent<PlayerMovementScript>().m_PlayerState = PlayerMovementScript.PState.Idle;
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
            if (_curNPCDialogue < NPCDialogue.Capacity)
            {
                DialogueTXT.text = NPCDialogue[_curNPCDialogue].ToString();
            }

            //Prepare the next string of dialogue to be shown
            _curNPCDialogue++;
        }

        //If the character is talking, display the character text
        if (_CharTalking)
        {
            if (_curCharDialogue < CharDialogue.Capacity)
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
