using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class BulletSpawnerScript : NetworkBehaviour
{
    public GameObject bulletPrefab, bulleType2Prefab;
    public NetworkVariable<int> PlayerA_bulletType2Capacity = new NetworkVariable<int>(3, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    public NetworkVariable<int> PlayerB_bulletType2Capacity = new NetworkVariable<int>(3, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    private List<GameObject> spawnedBullet = new List<GameObject>();
    public GameObject firePoint;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner) return;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpawnBulletServerRpc();
        }
        if (IsOwner)
        {
            if (Input.GetKeyDown(KeyCode.X) && PlayerA_bulletType2Capacity.Value != 0)
            {
                SpawnBulletType2ServerRpc();
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.X) && PlayerB_bulletType2Capacity.Value != 0)
            {
                SpawnBulletType2ServerRpc();
            }
        }


    }

    [ServerRpc]
    void SpawnBulletServerRpc()
    {
        Vector3 spawnPos = transform.position + (transform.forward * -1.5f) + (transform.up * 0.8f);
        Quaternion spawnRot = transform.rotation;
        GameObject bullet = Instantiate(bulletPrefab, firePoint.transform.position, bulletPrefab.transform.rotation);
        spawnedBullet.Add(bullet);
        bullet.GetComponent<BulletScript>().bulletSpawner = this;
        bullet.GetComponent<NetworkObject>().Spawn();
    }

    [ServerRpc]
    void SpawnBulletType2ServerRpc()
    {
        Vector3 spawnPos = transform.position + (transform.forward * -1.5f) + (transform.up * 0.8f);
        Quaternion spawnRot = transform.rotation;
        GameObject bullet = Instantiate(bulleType2Prefab, firePoint.transform.position, bulletPrefab.transform.rotation);
        spawnedBullet.Add(bullet);
        bullet.GetComponent<BulletScript>().bulletSpawner = this;
        bullet.GetComponent<NetworkObject>().Spawn();
        if (IsOwner)
        {
            if (PlayerA_bulletType2Capacity.Value != 0)
            {
                PlayerA_bulletType2Capacity.Value -= 1;
            }
        }
        else
        {
            if (PlayerB_bulletType2Capacity.Value != 0)
            {
                PlayerB_bulletType2Capacity.Value -= 1;
            }
        }
        
    }

    [ServerRpc (RequireOwnership = false)]
    public void DestroyServerRpc(ulong networkObjectId)
    {
        GameObject toDestroy = FindBombFromNetworkId(networkObjectId);
        if (toDestroy == null) return;
        toDestroy.GetComponent<NetworkObject>().Despawn();
        spawnedBullet.Remove(toDestroy);
        Destroy(toDestroy);
    }

    private GameObject FindBombFromNetworkId(ulong networkObjectId)
    {
        foreach(GameObject bullet in spawnedBullet)
        {
            ulong bulletId = bullet.GetComponent<NetworkObject>().NetworkObjectId;
            if(bulletId == networkObjectId) { return bullet; }
        }
        return null;
    }
}
