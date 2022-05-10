using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBasic : LivingThing
{
    private GameObject player;
    private PlayerControls controls;

    public HPBar hpBar;
    public override void OnDeath()
    {
        GameMaster.KillPlayer(player);
        GameStateManager.Instance.OnGameStateChanged -= OnGameStateGhanged;
        Debug.Log("player got killed");
    }
    // Start is called before the first frame update

    public override void OnGameStateGhanged(GameState gameState)
    {
        Debug.Log("player's pause state changed");
        able_to_move = gameState == GameState.Gameplay;
        controls.is_gameplay = gameState == GameState.Gameplay;
    }
    void Awake()
    {
        GameStateManager.Instance.OnGameStateChanged += OnGameStateGhanged;
        player = GameObject.FindGameObjectWithTag("Player");
        controls = GetComponent<PlayerControls>();
        hpBar = GameObject.FindGameObjectWithTag("HPBar").GetComponent<HPBar>();
        Debug.Log("player awake");

    }

    private void Start()
    {
        LoadSettings();
        CurrentHP = MaxHP;
        able_to_move = true;
        Debug.Log(MaxHP);
        hpBar.SetMaxHealth(MaxHP);
        hpBar.SetHealth(MaxHP);
        Debug.Log("player start");
    }

    public override void DamageFixed(int damage_value, bool pierce = false)
    {
        CurrentHP -= damage_value;
        hpBar.SetHealth(CurrentHP > 0 ? CurrentHP : 0);
        if (CurrentHP <= 0)
        {
            OnDeath();
        }
        Debug.Log("HP left: " + CurrentHP.ToString());
    }

    public override void DamageRelative(float damage_percent)
    {
        int damage_value = Mathf.Max(1, (int)(CurrentHP * damage_percent));
        CurrentHP -= damage_value;
        hpBar.SetHealth(CurrentHP > 0 ? CurrentHP : 0);
        if (CurrentHP <= 0)
        {
            OnDeath();
        }
    }

    public void LoadSettings()
    {
        MaxHP = PlayerPrefs.GetInt("maxHP", 5);
    }

    public void SaveSettings()
    {
        PlayerPrefs.SetInt("maxHP", MaxHP);
    }

    public void SetMaxHP(int health)
    {
        hpBar.SetMaxHealth(health);
        MaxHP = health;
    }
    public void SetCurrentHP(int health)
    {
        hpBar.SetHealth(health);
        CurrentHP = health;
    }
    //takes both negative and positive values
    public void IncreaseMaxHP(int health)
    {
        hpBar.IncreaseMaxHealth(health);
        MaxHP += health;
    }
    //takes both negative and positive values
    public void IncreaseCurrentHP(int health)
    {
        hpBar.IncreaseHealth(health);
        CurrentHP += health;
    }
}
