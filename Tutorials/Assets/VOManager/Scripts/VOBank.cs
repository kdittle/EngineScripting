using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// ClipProperties - Description:
/// Holds the different properties of a single audio clip.
/// </summary>

[System.Serializable]
public class ClipProperties
{
    // audio clip properties
    // also known as variables
    public int id;                      // keeps track of the clip's current id (uniquely assigned)
    public string strName;              // the name of the current clip
    public string strSubtitle;          // the subtitle tied into the current clip (if any)
    public AudioClip audClpDialogue;    // the current dialogue clip
    public bool blnExpandClipPnl;       // boolean to keep track of custom editor panel
}

/// <summary>
/// VOBank - Description:
/// An audio bank that holds a collection of VO (voice-over) clips.
/// </summary>

// automatically assign the VOManager.cs script and 
// audio source component to the object if none
// is present
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(VOManager))]
public class VOBank : MonoBehaviour
{
    // a list of audio clip properties
    public List<ClipProperties> bank = new List<ClipProperties>();

    // add a new clip to the bank
    public void AddClip()
    {
        bank.Add(new ClipProperties());
    }

    // remove a clip from the bank
    public void RemoveClip(int _id)
    {
        bank.RemoveAt(_id);
    }
}