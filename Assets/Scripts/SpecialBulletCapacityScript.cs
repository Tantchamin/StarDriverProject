using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;


public class SpecialBulletCapacityScript : MonoBehaviour
{
    public NetworkVariable<int> PlayerA_bulletType2Capacity = new NetworkVariable<int>(3, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    public NetworkVariable<int> PlayerB_bulletType2Capacity = new NetworkVariable<int>(3, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    public int GetPlayerABulletCapacity()
    {
        return PlayerA_bulletType2Capacity.Value;
    }

    public int GetPlayerBBulletCapacity()
    {
        return PlayerB_bulletType2Capacity.Value;
    }

    public void PlayerACapacityAdjust(int amount)
    {
        PlayerA_bulletType2Capacity.Value += amount;
    }

    public void PlayerBCapacityAdjust(int amount)
    {
        PlayerB_bulletType2Capacity.Value += amount;
    }


}
