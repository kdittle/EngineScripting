using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class TriggerZone : MonoBehaviour
{

    private Canvas _cameraCanvas;
    private Canvas _worldCanvas;

    public List<string> NPCDialogue;
    public List<string> CharDialogue;
    private Text DialogueTXT;

    private int _curNPCDialogue;
    private int _curCharDialogue;

    private bool _NPCTalking;
    private bool _CharTalking;

    private Animator DialogueAnim;


    // Use this for initialization
    void Start()
    {

        //Find the two canvas objects
        //Canvas objects should be turned off by default in editor so they don't appear on screen
        _cameraCanvas = GameObject.FindGameObjectWithTag("CPDialogueCanvas").GetComponent<Canvas>();
        _worldCanvas = GameObject.FindGameObjectWithTag("WPDialogueCanvas").GetComponent<Canvas>();

        //Set the current NPCDialogue to 0, so the first sentence may be displayed
        _curNPCDialogue = 0;
        _curCharDialogue = 0;
        _NPCTalking = true;
        _CharTalking = false;

        DialogueAnim = GameObject.FindGameObjectWithTag("CPDialogueCanvas").GetComponent<Animator>();
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
            gameObject.GetComponent<AudioSource>().Play();

            //Fade in the dialogue box.
            DialogueAnim.Play("FadeIn");
        }
    }

    void OnTriggerEnter(Collider other)
    {

        //Check to see if it's the player that entered the zone
        if (other.gameObject.tag == "Player")
        {
            //Check which AI trigger zone the player has entered
            if (this.gameObject.tag == "AI1")
            {

                _curNPCDialogue = 0;
                _curCharDialogue = 0;

                //Enable the proper canvas
                _cameraCanvas.enabled = true;
                DialogueTXT = _cameraCanvas.GetComponentInChildren<Text>();
                SwitchDialogue();
                gameObject.GetComponent<AudioSource>().Play();
            }

            //Check which AI trigger zone the player has entered
            if (this.gameObject.tag == "AI2")
            {

                _curNPCDialogue = 0;
                _curCharDialogue = 0;

                //Spawn the NPCDialogue box above the AI character
                _worldCanvas.gameObject.transform.position = new Vector3(transform.localPosition.x, transform.localPosition.y + 2, transform.localPosition.z);

                //enable the proper canvas
                _worldCanvas.enabled = true;
                DialogueTXT = _worldCanvas.GetComponentInChildren<Text>();
                SwitchDialogue();
                gameObject.GetComponent<AudioSource>().Play();
            }
        }
    }

    ////This isn't really being used.
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {

            ////Check which AI trigger zone the player has entered
            if (this.gameObject.tag == "AI2")
            {
                if (!_NPCTalking)
                {
                    if (_curNPCDialogue <= NPCDialogue.Capacity)
                    {
                        _worldCanvas.gameObject.transform.position = new Vector3(transform.position.x, transform.position.y + 2, transform.position.z);
                    }
                }

                //If the character is talking, display the character text
                if (!_CharTalking)
                {
                    if (_curCharDialogue <= CharDialogue.Capacity)
                    {
                        _worldCanvas.gameObject.transform.position = new Vector3(GameObject.FindGameObjectWithTag("Player").gameObject.transform.position.x,
                            GameObject.FindGameObjectWithTag("Player").gameObject.transform.position.y + 2, GameObject.FindGameObjectWithTag("Player").gameObject.transform.position.z);
                    }
                }

                //Really bad solution for displaying the final exchange of dialgoue
                //Works for now.
                if (_curNPCDialogue >= NPCDialogue.Capacity && _curCharDialogue >= CharDialogue.Capacity)
                {
                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        if (_cameraCanvas.enabled)
                            _cameraCanvas.enabled = false;
                        if (_worldCanvas.enabled)
                            _worldCanvas.enabled = false;
                    }
                }
            }
        }
    }

    //When player exits
    void OnTriggerExit(Collider other)
    {
        //turn off the chat stuff.
        if (other.gameObject.tag == "Player")
        {
            if (this.gameObject.tag == "AI1")
            {
                _cameraCanvas.enabled = false;
            }

            if (this.gameObject.tag == "AI2")
            {
                _worldCanvas.enabled = false;
            }
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
