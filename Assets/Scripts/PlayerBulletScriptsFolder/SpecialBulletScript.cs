using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class SpecialBulletScript : NetworkBehaviour
{
    public SpawnRandomSpecialBulletScript specialSpawner;
    public float speed = 2;

    private void OnTriggerEnter(Collider other)
    {
        if (!IsOwner) return;

        //if(collision.gameObject.tag == "Player")
        //{
        //    ulong networkObkjectId = GetComponent<NetworkObject>().NetworkObjectId;
        //    //SpawnBulletEffect();
        //    bulletSpawner.DestroyServerRpc(networkObkjectId);
        //}

        //if (other.gameObject.tag == "Player" || other.gameObject.tag == "DeathZone")
        //{
        //    ulong networkObkjectId = GetComponent<NetworkObject>().NetworkObjectId;
        //    //SpawnBulletEffect();
        //    specialSpawner.DestroyServerRpc(networkObkjectId);
        //    Debug.Log("GET");
        //}
    }

    private void FixedUpdate()
    {
        transform.position -= transform.up * speed * Time.deltaTime;
    }
}
