using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class SpecialBulletUIScript : NetworkBehaviour
{
    public NetworkVariable<int> PlayerA_bullet = new NetworkVariable<int>(3, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    public NetworkVariable<int> PlayerB_bullet = new NetworkVariable<int>(3, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    public GameObject[] bulletUI;

    private void Update()
    {
        if (IsOwner)
        {
            if (Input.GetKeyDown(KeyCode.X) && PlayerA_bullet.Value != 0)
            {
                PlayerA_bullet.Value -= 1;
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.X) && PlayerB_bullet.Value != 0)
            {
                PlayerB_BulletAdjustServerRpc();
            }
        }

        playerABulletCheck();
        playerBBulletCheck();
    }

    void playerABulletCheck()
    {
        if (PlayerA_bullet.Value == 3)
        {
            bulletUI[0].SetActive(true);
            bulletUI[1].SetActive(true);
            bulletUI[2].SetActive(true);
        }
        else if (PlayerA_bullet.Value == 2)
        {
            bulletUI[0].SetActive(true);
            bulletUI[1].SetActive(true);
            bulletUI[2].SetActive(false);
        }
        else if (PlayerA_bullet.Value == 1)
        {
            bulletUI[0].SetActive(true);
            bulletUI[1].SetActive(false);
            bulletUI[2].SetActive(false);
        }
        else
        {
            bulletUI[0].SetActive(false);
            bulletUI[1].SetActive(false);
            bulletUI[2].SetActive(false);
        }
    }

    void playerBBulletCheck()
    {
        if (PlayerB_bullet.Value == 3)
        {
            bulletUI[3].SetActive(true);
            bulletUI[4].SetActive(true);
            bulletUI[5].SetActive(true);
        }
        else if (PlayerB_bullet.Value == 2)
        {
            bulletUI[3].SetActive(true);
            bulletUI[4].SetActive(true);
            bulletUI[5].SetActive(false);
        }
        else if (PlayerB_bullet.Value == 1)
        {
            bulletUI[3].SetActive(true);
            bulletUI[4].SetActive(false);
            bulletUI[5].SetActive(false);
        }
        else
        {
            bulletUI[3].SetActive(false);
            bulletUI[4].SetActive(false);
            bulletUI[5].SetActive(false);
        }
    }

    [ServerRpc]
    void PlayerB_BulletAdjustServerRpc()
    {
        PlayerB_bullet.Value -= 1;
    }

}
