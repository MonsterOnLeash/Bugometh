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

	public void SetMaxHealth(int health)
	{
		if (health > maxPossibleHP)
        {
			health = maxPossibleHP;

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
		if (health > maxPossibleHP)
		{
			health = maxPossibleHP;

		}
		for (int i = 0; i < health; ++i)
		{
			array[i].sprite = fullHeart;
		}
		for (int i = health; i < maxPossibleHP; ++i)
		{
			array[i].sprite = emptyHeart;
		}
	}

	void Awake()
    {
		maxPossibleHP = array.Length;
    }
}