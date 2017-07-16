using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// VOManager - Description:
/// Manages and handles how the clips 
/// from the VOBank are played.
/// </summary>

// automatically assign the VOBank.cs script and 
// audio source component to the object if none
// is present
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(VOBank))]
public class VOManager : MonoBehaviour
{
    // global variables
    public static VOManager Instance;       // make VOManager a global instance so we can use for reference

    // public variables
    public Text txtSubtitle;                // the text component that will dispaly the subtitles

    // private variables
    private AudioSource m_audSrc;           // the object's audio source component
    private VOBank voBank;                  // the VOBank reference
    private bool m_blnDialogueTriggered;    // boolean to check if a dialogue has been triggered

    // on enabled
    void OnEnable()
    {
        m_audSrc = this.GetComponent<AudioSource>();    // get the object's audio source component
        voBank = this.GetComponent<VOBank>();           // get the VOBank reference
    }

    // on loaded
    void Awake()
    {
        Instance = this;    // set the Instance to this object reference

        // try and run the following
        try
        {
            txtSubtitle.text = "";              // clear out any subtitle text
        }
        catch
        {
            // throw a warning
            Debug.LogError("No UI Text component is found. Please set the subtitle Text component.");
        }

        m_blnDialogueTriggered = false;     // set dialogue to not triggered
    }

    // on initialization
    void Start ()
    {
        m_audSrc.playOnAwake = false;   // disable play on awake
        m_audSrc.spatialBlend = 0;      // set sound settings to 2D
        m_audSrc.loop = false;          // turn off sound looping
    }

    // once per frame
    void Update()
    {
        // if a dialogue was triggered
        if (m_blnDialogueTriggered)
            // if the current audio clip is done playing
            if (!m_audSrc.isPlaying)
            {
                Stop();                             // call the stop function
                m_blnDialogueTriggered = false;     // set the dialogue trigger to false
            }
    }

    // play the desired clip only if no other clips are playing
    public void Play(int _id)
    {
        // if there is an audio clip playing
        if (m_audSrc.isPlaying)
            return;
        else
        {
            // try and run the following
            try
            {
                txtSubtitle.text = voBank.bank[_id].strSubtitle;   // set the subtitle text on the screen (display)
                m_audSrc.clip = voBank.bank[_id].audClpDialogue;   // set the audio source's clip to the new clip
                m_audSrc.Play();                            // play the new audio clip
                m_blnDialogueTriggered = true;              // set the dialogue trigger to true
            }
            catch
            {
                // throw a warning
                Debug.LogError("Index is out of range. Please check if the audio bank is the correct length.");
            }
        }
    }

    // play the desired clip and stop other audio clips that are playing
    public void PlayInterrupt(int _id)
    {
        // if there is an audio clip playing
        if (m_audSrc.isPlaying)
            // then stop playing the current clip
            m_audSrc.Stop();

        // try and run the following
        try
        {
            txtSubtitle.text = voBank.bank[_id].strSubtitle;   // set the subtitle text on the screen (display)
            m_audSrc.clip = voBank.bank[_id].audClpDialogue;   // set the audio source's clip to the new clip
            m_audSrc.Play();                                   // play the new audio clip
            m_blnDialogueTriggered = true;                     // set the dialogue trigger to true
        }
        catch
        {
            // throw a warning
            Debug.LogError("Index is out of range. Please check if the audio bank is the correct length.");
        }
    }

    // stop and clear all dialogue components
    public void Stop()
    {
        txtSubtitle.text = "";  // clear the subtitle text 
        m_audSrc.Stop();        // stop audio from playing
        m_audSrc.clip = null;   // clear the current clip
    }
}