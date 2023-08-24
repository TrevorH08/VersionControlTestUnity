
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using FishNet.Connection;


public class PlayerSpawnObject : NetworkBehaviour
{
    public GameObject objToSpawn;
     public GameObject spawnedObject;
    public override void OnStartClient()
    {
        base.OnStartClient();
        if (!base.IsOwner)
        {
            GetComponent<PlayerSpawnObject>().enabled = false;
        }
    }

    private void Update()
    {
        //spawn on 1 down, if no spawned objects
        if(spawnedObject == null && Input.GetKeyDown(KeyCode.Alpha1))
        {
            SpawnObject(objToSpawn, transform, this);
        }
        //despawn on key 2 down if spawned objects exist
        if(spawnedObject != null && Input.GetKeyDown(KeyCode.Alpha2))
        {
            DespawnObject(spawnedObject);
        }
    }

    //we always want to spawn objects on server
    [ServerRpc]
    public void SpawnObject(GameObject obj, Transform player, PlayerSpawnObject script) //params: The object to spawn, location of player (to spawn on the player), the PlayerSpawnObject script, which has data about if the player has already spawned an object (to read/write)
    {
        //spawns the object locally
        GameObject spawned = Instantiate(obj, player.position + player.forward, Quaternion.identity);
        //spawns the object for every other player
        ServerManager.Spawn(spawned);
        SetSpawnedObject(spawned, script);
    }

    [ObserversRpc]
    public void SetSpawnedObject(GameObject spawned, PlayerSpawnObject script)
    {
        script.spawnedObject = spawned;
    }

    [ServerRpc(RequireOwnership = false)]
    public void DespawnObject(GameObject obj)
    {
        ServerManager.Despawn(obj);
    }
}
