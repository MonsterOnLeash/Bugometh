using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
	public Image[] array;
	public Sprite fullHeart;
	public Sprite emptyHeart;
	private int maxPossibleHP;
	private int currentMaxHP;
	private int currentHP;

	public void SetMaxHealth(int health)
	{
		if (health > maxPossibleHP)
        {
			health = maxPossibleHP;

		}
		currentMaxHP = health;
		if (currentHP > currentMaxHP)
        {
			currentHP = currentMaxHP;
        }
		for (int i = 0; i < health; ++i)
        {
			array[i].enabled = true;
        }
		for (int i = health; i < maxPossibleHP; ++i)
        {
			array[i].enabled = false;
        }
	}

	public void SetHealth(int health)
	{
		if (health > currentMaxHP)
		{
			health = currentMaxHP;

		}
		currentHP = health;
		for (int i = 0; i < health; ++i)
		{
			array[i].sprite = fullHeart;
		}
		for (int i = health; i < maxPossibleHP; ++i)
		{
			array[i].sprite = emptyHeart;
		}
	}

	public void IncreaseMaxHealth(int value)
    {
		int new_max_HP = currentMaxHP + value;
		SetMaxHealth(new_max_HP);
    }
	public void IncreaseHealth(int value)
	{
		int new_HP = currentHP + value;
		SetHealth(new_HP);
	}

	void Awake()
    {
		maxPossibleHP = array.Length;
		currentMaxHP = array.Length;
    }
}