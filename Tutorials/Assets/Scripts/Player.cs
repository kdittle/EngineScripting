using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class Player : MonoBehaviour
{
    public ReactiveProperty<long> health = new ReactiveProperty<long>(100);
    public ReactiveProperty<long> mana = new ReactiveProperty<long>(100);
    public ReactiveProperty<long> exp = new ReactiveProperty<long>(0);
    public ReactiveProperty<long> expToNextLvl = new ReactiveProperty<long>(250);
    public ReactiveProperty<int> level = new ReactiveProperty<int>(1);

    private int expToNextLevel = 250;
    private float expToLevelScaler = 1.37f;
    private long maxHealth;
    private long maxMana;


	// Use this for initialization
	void Start ()
    {
        level.Value = 1;
        expToNextLevel = 250;
        maxHealth = health.Value;
        maxMana = mana.Value;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            SubtractFromMana(10);
        else if (Input.GetKeyDown(KeyCode.M))
            ApplyManaEffect(10);

        if (Input.GetKeyDown(KeyCode.UpArrow))
            ApplyHealthEffect(10);
        else if (Input.GetKeyDown(KeyCode.DownArrow))
            ApplyDamage(10);

        if (Input.GetKeyDown(KeyCode.Keypad0))
            AddExperience(25);

        if (Input.GetKeyDown(KeyCode.Keypad1))
            AddExperience(250);

        if (Input.GetKeyDown(KeyCode.Keypad5))
            AddExperience(1000);
    }

    public void AddExperience(int expToAdd)
    {
        exp.Value += expToAdd;

        CheckLevelIncrease();
    }

    private void CheckLevelIncrease()
    {
        long overflowExp = 0;

        if (exp.Value >= expToNextLevel)
        {
            level.Value++;
            overflowExp = (exp.Value - expToNextLevel);

            exp.Value = 0;

            IncreaseExpToNextLevel();

            if(overflowExp > 0)
            {
                exp.Value = overflowExp;
                CheckLevelIncrease();
            }
        }
    }

    private void IncreaseExpToNextLevel()
    {
        expToNextLevel = (int)Mathf.Floor(Mathf.Log(expToNextLevel) + (expToNextLevel * expToLevelScaler));
        expToNextLvl.Value = expToNextLevel;
    }

    public void ApplyDamage(int damage)
    {
        if ((health.Value - damage) >= 0)
            health.Value -= damage;
        else if (health.Value - damage <= 0)
            health.Value = 0;
    }

    public void IncreaseHealth(int healthIncrase)
    {
        
    }

    public void ApplyHealthEffect(int effectIncrease)
    {
        if ((health.Value + effectIncrease) < maxHealth)
            health.Value += effectIncrease;
        else if ((health.Value + effectIncrease >= maxHealth))
            health.Value = maxHealth;
    }

    public void SubtractFromMana(int manaUsed)
    {
        if ((mana.Value - manaUsed) >= 0)
            mana.Value -= manaUsed;
        else if (mana.Value - manaUsed <= 0)
            mana.Value = 0;
    }

    public void IncreaseMana(int manaIncrease)
    {

    }

    public void ApplyManaEffect(int effectIncrease)
    {
        if ((mana.Value + effectIncrease) < maxMana)
            mana.Value += effectIncrease;
        else if ((mana.Value + effectIncrease) >= maxMana)
            mana.Value = maxMana;
    }
}