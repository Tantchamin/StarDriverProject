using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;


public class EnemyBulletSpawnPatternSpiral : MonoBehaviour
{
    public GameObject bullet;
    public float fireRate = 0.2f;

    public float angle = 0f;
    public int anglePlus = 20;

    private Vector2 bulletMoveDirection;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("Fire", 0f, fireRate);
    }

    private void Fire()
    {
        float bulDirX = transform.position.x + Mathf.Sin((angle * Mathf.PI) / 180);
        float bulDirY = transform.position.y + Mathf.Cos((angle * Mathf.PI) / 180);

        Vector3 bulMoveVector = new Vector3(bulDirX, bulDirY, 0f);
        Vector2 bulDir = (bulMoveVector - transform.position).normalized;

        GameObject bul = Instantiate(bullet, transform.position, transform.rotation);
        bul.transform.position = transform.position;
        bul.transform.rotation = transform.rotation;
        bul.GetComponent<EnemyBulletScript>().SetMoveDirection(bulDir);
        bul.GetComponent<NetworkObject>().Spawn();

        angle += anglePlus;
        if(angle >= 360)
        {
            angle = 0;
        }
    }
}
