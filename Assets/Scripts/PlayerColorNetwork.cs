
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Connection;
using FishNet.Object;

public class PlayerColorNetwork : NetworkBehaviour
{
        public GameObject body;
        public Color endColor;
    // Start is called before the first frame update
    public override void OnStartClient()
    {
        base.OnStartClient();
        if (!base.IsOwner)
        {
                        //disables the component if we are not the owner
            GetComponent<PlayerColorNetwork>().enabled = false;
        }

    }

    private void Update() 
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            ChangeColorServer(gameObject, endColor);
        }
    }

    //Tells the server what we want to do, and to send that out to each player
    [ServerRpc]
    public void ChangeColorServer(GameObject player, Color color)
    {
        ChangeColor(player, color);
    }

    //lets all the observers know we run the function
    [ObserversRpc]
    public void ChangeColor(GameObject player, Color color)
    {
        player.GetComponent<PlayerColorNetwork>().body.GetComponent<Renderer>().material.color = color;
    }
}
