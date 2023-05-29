using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;


public class BulletEffectScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("DestroyServerRpc", 0.85f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [ServerRpc(RequireOwnership = false)]
    public void DestroyServerRpc()
    {

        GetComponent<NetworkObject>().Despawn();
        Destroy(gameObject);
    }
}
