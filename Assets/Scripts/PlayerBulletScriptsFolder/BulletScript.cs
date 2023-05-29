using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class BulletScript : NetworkBehaviour
{
    public BulletSpawnerScript bulletSpawner;
    public float speed = 2;
    public int damage = 1;
    public GameObject bulletEffectPrefab;

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (!IsOwner) return;

    //    //if(collision.gameObject.tag == "Player")
    //    //{
    //    //    ulong networkObkjectId = GetComponent<NetworkObject>().NetworkObjectId;
    //    //    //SpawnBulletEffect();
    //    //    bulletSpawner.DestroyServerRpc(networkObkjectId);
    //    //}

    //    if (collision.gameObject.tag == "Enemy")
    //    {
    //        ulong networkObkjectId = GetComponent<NetworkObject>().NetworkObjectId;
    //        //SpawnBulletEffect();
    //        bulletSpawner.DestroyServerRpc(networkObkjectId);
    //    }
    //}

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
            bulletSpawner.DestroyServerRpc(networkObkjectId);
            Debug.Log("Oras");
        }
    }

    private void FixedUpdate()
    {
        transform.position += transform.up * speed * Time.deltaTime;
    }

    private void SpawnBulletEffect()
    {
        GameObject bulleteffect = Instantiate(bulletEffectPrefab, transform.position, Quaternion.identity);
        bulleteffect.GetComponent<NetworkObject>().Spawn();
    }


}
