using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class BossSpawnScript : MonoBehaviour
{
    public GameObject spawnPoint;
    public GameObject Boss1;

    private void Start()
    {
        
    }

    public void SpawnBoss()
    {
        GameObject Boss = Instantiate(Boss1, spawnPoint.transform.position, Boss1.transform.rotation);
        Boss.GetComponent<NetworkObject>().Spawn();
        Boss.SetActive(false);
    }
}
