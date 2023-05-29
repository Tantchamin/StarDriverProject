using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System.Diagnostics;

public class BulletEffectScript : NetworkBehaviour
{
    public GameObject bulletEffectPrefab;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!IsOwner) return;

        //if(collision.gameObject.tag == "Player")
        //{
        //    ulong networkObkjectId = GetComponent<NetworkObject>().NetworkObjectId;
        //    //SpawnBulletEffect();
        //    bulletSpawner.DestroyServerRpc(networkObkjectId);
        //}

        if (other.gameObject.tag == "Enemy" || other.gameObject.tag == "DeathZone")
        {
            ulong networkObkjectId = GetComponent<NetworkObject>().NetworkObjectId;
            SpawnBulletEffect();
            deletebullet();
        }
    }

    private void SpawnBulletEffect()
    {
        GameObject bulleteffect = Instantiate(bulletEffectPrefab, transform.position, Quaternion.identity);
        bulleteffect.GetComponent<NetworkObject>().Spawn();
    }

    private void deletebullet()
    {
        Destroy(bulletEffectPrefab, 3); //you have 15 seconds before the objects deletes itself
    }
}
