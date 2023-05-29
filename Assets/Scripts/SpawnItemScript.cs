using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class SpawnItemScript : NetworkBehaviour
{
    [SerializeField] private GameObject specialBullet;
    [SerializeField] private float fireRate = 2f;
    private List<GameObject> spawnedSpecial = new List<GameObject>();


    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnSpecialServerRpc", 1f, fireRate);
    }

    // Update is called once per frame
    void Update()
    {

    }

    [ServerRpc(RequireOwnership = false)]
    void SpawnSpecialServerRpc()
    {
        Vector3 position = new Vector3(Random.Range(-8, 8), 4, 0);
        GameObject special = Instantiate(specialBullet, position, specialBullet.transform.rotation);
        spawnedSpecial.Add(special);
        special.GetComponent<SpecialItemScript>().specialSpawner = this;
        special.GetComponent<NetworkObject>().Spawn();
    }

    [ServerRpc(RequireOwnership = false)]
    public void DestroyServerRpc(ulong networkObjectId)
    {
        GameObject toDestroy = FindBombFromNetworkId(networkObjectId);
        if (toDestroy == null) return;
        toDestroy.GetComponent<NetworkObject>().Despawn();
        spawnedSpecial.Remove(toDestroy);
        Destroy(toDestroy);
    }

    private GameObject FindBombFromNetworkId(ulong networkObjectId)
    {
        foreach (GameObject bullet in spawnedSpecial)
        {
            ulong bulletId = bullet.GetComponent<NetworkObject>().NetworkObjectId;
            if (bulletId == networkObjectId) { return bullet; }
        }
        return null;
    }

}
