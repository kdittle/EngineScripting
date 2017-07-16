using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class HUDManager : MonoBehaviour
{
    public Player player;
    public Text HealthText;
    public Text ManaText;
    public Text Level;
    public Text ExpText;
    public Text ExpToNextLevel;

	// Use this for initialization
	void Start ()
    {
        player.health.Select(h => string.Format("{0}", h)).Subscribe(text => HealthText.text = text);
        player.mana.Select(m => string.Format("{0}", m)).Subscribe(text => ManaText.text = text);
        player.level.Select(l => string.Format("{0}", l)).Subscribe(text => Level.text = text);
        player.exp.Select(e => string.Format("{0}", e)).Subscribe(text => ExpText.text = text);
        player.expToNextLvl.Select(ex => string.Format("/" + "{0}", ex)).Subscribe(text => ExpToNextLevel.text = text);
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}