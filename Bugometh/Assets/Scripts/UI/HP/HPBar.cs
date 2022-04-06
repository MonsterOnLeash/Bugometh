using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{

	private Slider slider;
	//public Gradient gradient;
	//public Image fill;

	public void SetMaxHealth(int health)
	{
		slider.maxValue = health;

		//fill.color = gradient.Evaluate(1f);
	}

	public void SetHealth(int health)
	{
		slider.value = health;

		//fill.color = gradient.Evaluate(slider.normalizedValue);
	}

	void Awake()
    {
		slider = GetComponent<Slider>();
    }

}