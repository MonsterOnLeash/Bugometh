using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBasic : LivingThing
{
    public override void OnDeath()
    {
        GameMaster.KillPlayer(this);
    }
    // Start is called before the first frame update
    void Start()
    {
        able_to_move = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
