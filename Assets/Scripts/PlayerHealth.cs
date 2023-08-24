using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using FishNet.Object.Synchronizing;
 
//Made by Bobsi Unity - Youtube
public class PlayerHealth : NetworkBehaviour
{
    //make public var synchronized w SyncVar (uses ...Synchronizing)
    [SyncVar] public int health = 10;
 
    //Makes sure this only applies to owner/player
    public override void OnStartClient()
    {
        base.OnStartClient();
        if (!base.IsOwner)
            GetComponent<PlayerHealth>().enabled = false;
    }
 
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            UpdateHealth(this, -1);
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            UpdateHealth(this, 300);
        }
    }
 
    [ServerRpc]
    public void UpdateHealth(PlayerHealth script, int amountToChange)
    {
        script.health += amountToChange;
    }

}