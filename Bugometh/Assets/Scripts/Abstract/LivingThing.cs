using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingThing : MonoBehaviour
{
    public int MaxHP;
    protected int CurrentHP;
    public bool able_to_move = false;
    public float speed = 1; // TODO check if it's actually needed here
    
    public virtual void DamageFixed(int damage_value)
    {
        CurrentHP -= damage_value;
        if (CurrentHP <= 0)
        {
            OnDeath();
        }
        Debug.Log("HP left: " + CurrentHP.ToString());
    }

    public virtual void DamageRelative(float damage_percent)
    {
        int damage_value = Mathf.Max(1, (int)(CurrentHP * damage_percent));
        CurrentHP -= damage_value;
        if (CurrentHP <= 0)
        {
            OnDeath();
        }
    }
    public virtual void OnDeath()
    {
        Debug.Log("creature got killed");
        Destroy(gameObject);
    }

    public virtual void OnGameStateGhanged(GameState gameState)
    {
        able_to_move = gameState == GameState.Gameplay;
    }

    public virtual Vector2Int GetHealthStats()
    {
        return new Vector2Int(CurrentHP, MaxHP);
    }
}
