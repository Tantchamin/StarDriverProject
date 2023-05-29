using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class SpecialItemScript : NetworkBehaviour
{
    public SpawnItemScript specialSpawner;
    public float speed = 5;

    private void OnTriggerEnter(Collider other)
    {
        if (!IsOwner) return;

        if (other.gameObject.tag == "Player" || other.gameObject.tag == "DeathZone")
        {
            Debug.Log("GET");
            ulong networkObkjectId = GetComponent<NetworkObject>().NetworkObjectId;
            specialSpawner.DestroyServerRpc(networkObkjectId);
        }
    }

    private void FixedUpdate()
    {
        transform.position -= transform.up * speed * Time.deltaTime;
    }
}