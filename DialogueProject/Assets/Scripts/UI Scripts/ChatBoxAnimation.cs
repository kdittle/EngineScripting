using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class ChatBoxAnimation : MonoBehaviour
{
    private bool _firstTimeOpening;
    private Canvas _canvas;

	// Use this for initialization
	void Start ()
    {
        _firstTimeOpening = true;
        _canvas = gameObject.GetComponent<Canvas>();
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if(_firstTimeOpening && gameObject.GetComponent<Canvas>().enabled)
        {
            //Animate the piece in
            _canvas.GetComponentInChildren<Animator>().Play(0);
            _firstTimeOpening = false;
        }
	}
}
