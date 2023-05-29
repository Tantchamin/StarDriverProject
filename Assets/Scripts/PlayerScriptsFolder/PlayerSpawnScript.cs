using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Collections;

public class PlayerSpawnScript : NetworkBehaviour
{
    //int healthPoint = 3;
    bool isPlayer1;
    MainPlayerScript mainPlayer;
    public BoxCollider boxCollider; 
    private BoxCollider sprite;
    public Behaviour[] scripts;
    private Renderer[] renderers;
    BulletSpawnerScript bulletSpawner;
    LoginManagerScript loginManager;
    public SpecialBulletUIScript specialBulletUIScript;
    public BulletSpawnerScript bulletSpawnerScript;

    public NetworkVariable<int> healthPointA = new NetworkVariable<int>(3,
        NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<int> healthPointB = new NetworkVariable<int>(3);

    private void Start()
    {
        mainPlayer = GetComponent<MainPlayerScript>();
        bulletSpawner = GetComponent<BulletSpawnerScript>();
        renderers = GetComponentsInChildren<Renderer>();
        loginManager = GameObject.FindGameObjectWithTag("LoginManager").GetComponent<LoginManagerScript>();
        specialBulletUIScript = GameObject.FindGameObjectWithTag("BulletUI").GetComponent<SpecialBulletUIScript>();
        sprite = boxCollider.GetComponent<BoxCollider>();

        healthPointA.Value = 3;
        healthPointB.Value = 3;

    }

    private void OnTriggerEnter(Collider other)
    {
        if (IsOwner)
        {
            if (other.CompareTag("EnemyBullet") && healthPointA.Value > 0)
            {
                mainPlayer.enabled = false;
                bulletSpawner.enabled = false;
                Respawn();
                healthPointA.Value -= 1;
                Debug.Log("A: " + healthPointA.Value + " B: " + healthPointB.Value);
            }
            else if (other.CompareTag("EnemyBullet") && healthPointA.Value <= 0)
            {
                mainPlayer.enabled = false;
                bulletSpawner.enabled = false;
                SetPlayerState(false);
                Debug.Log("A: " + healthPointA.Value + " B: " + healthPointB.Value);
            }
            else if (other.CompareTag("Item"))
            {
                if(specialBulletUIScript.PlayerA_bullet.Value < 3)
                {
                    specialBulletUIScript.PlayerA_bullet.Value += 1;
                    bulletSpawnerScript.PlayerA_bulletType2Capacity.Value += 1;
                }
                Debug.Log("A Bullet UI: " + specialBulletUIScript.PlayerA_bullet.Value + " A Bullet Capacity: " + bulletSpawnerScript.PlayerA_bulletType2Capacity.Value);
            }
        }
        else
        {
            if (other.CompareTag("EnemyBullet") && healthPointB.Value > 0)
            {
                mainPlayer.enabled = false;
                bulletSpawner.enabled = false;
                Respawn();
                ChangeHealthBServerRpc();
                Debug.Log("A: " + healthPointA.Value + " B: " + healthPointB.Value);
            }
            else if (other.CompareTag("EnemyBullet") && healthPointB.Value <= 0)
            {
                mainPlayer.enabled = false;
                bulletSpawner.enabled = false;
                SetPlayerState(false);
                Debug.Log("A: " + healthPointA.Value + " B: " + healthPointB.Value);
            }
            else if (other.CompareTag("Item"))
            {
                if (specialBulletUIScript.PlayerB_bullet.Value < 3)
                {
                    specialBulletUIScript.PlayerB_bullet.Value += 1;
                    bulletSpawnerScript.PlayerB_bulletType2Capacity.Value += 1;
                }
                Debug.Log("B Bullet UI: " + specialBulletUIScript.PlayerB_bullet.Value + " B Bullet Capacity: " + bulletSpawnerScript.PlayerB_bulletType2Capacity.Value);
            }
        }

        if (healthPointA.Value <= 0 && healthPointB.Value <= 0)
        {
            loginManager.leaveButton.SetActive(true);
        }

    }

    private Vector3 GetRandPos()
    {
        Vector3 randPos;
        if (IsOwner)
        {
            randPos = new Vector3(loginManager.SpawnPoints[0].transform.position.x, loginManager.SpawnPoints[0].transform.position.y, loginManager.SpawnPoints[0].transform.position.z);
        }
        else
        {
            randPos = new Vector3(loginManager.SpawnPoints[1].transform.position.x, loginManager.SpawnPoints[1].transform.position.y, loginManager.SpawnPoints[1].transform.position.z);

        }

        return randPos;
    }

    public void Respawn()
    {
        RespawnServerRpc();
    }

    [ServerRpc (RequireOwnership = false)]
    private void ChangeHealthBServerRpc()
    {
        healthPointB.Value -= 1;
    }
    

    [ServerRpc (RequireOwnership = false)]
    private void RespawnServerRpc()
    {
        RespawnClientRpc(GetRandPos());
    }

    [ClientRpc]
    private void RespawnClientRpc(Vector3 spawnPos)
    {
        StartCoroutine(RespawnCoroutine(spawnPos));
    }

    private void SetPlayerState(bool state)
    {
        foreach (var script in scripts) { script.enabled = state; }
        foreach (var renderer in renderers) { renderer.enabled = state; }
        sprite.enabled = state;
    }

    IEnumerator RespawnCoroutine(Vector3 spawnPos)
    {
        SetPlayerState(false);
        transform.position = spawnPos;
        yield return new WaitForSeconds(3);
        mainPlayer.enabled = true;
        bulletSpawner.enabled = true;
        SetPlayerState(true);
    }
}
