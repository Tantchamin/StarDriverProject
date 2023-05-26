using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class BulletSpawnerScript : NetworkBehaviour
{
    public GameObject bulletPrefab, bulleType2Prefab;
    private int bulletType2Capacity = 3;
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
        if (Input.GetKeyDown(KeyCode.X) && bulletType2Capacity != 0)
        {
            SpawnBulletType2ServerRpc();
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
        if(bulletType2Capacity != 0)
        {
            bulletType2Capacity -= 1;

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
