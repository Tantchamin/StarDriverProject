using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class EnemyBulletScript : NetworkBehaviour
{
    private Vector2 moveDirection;
    public float speed = 5;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void FixedUpdate()
    {
        transform.Translate(moveDirection * speed * Time.deltaTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "DeathZone" || other.gameObject.tag == "Player")
        {
            DestroyServerRpc();
        }
    }

    public void SetMoveDirection(Vector2 dir)
    {
        moveDirection = dir;
    }

    [ServerRpc(RequireOwnership = false)]
    public void DestroyServerRpc()
    {

        GetComponent<NetworkObject>().Despawn();
        Destroy(gameObject);
    }

}
