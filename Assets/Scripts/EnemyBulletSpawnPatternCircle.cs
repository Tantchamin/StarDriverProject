using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
public class EnemyBulletSpawnPatternCircle : MonoBehaviour
{
    public int bulletSpawnAmount = 10;
    public GameObject bullet;

    public float startAngle = 90f;
    public float EndAngle = 270f;
    public float fireRate = 1f;

    private Vector2 bulletMoveDirection;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("Fire", 1f, fireRate);   
    }

    private void Fire()
    {
        float angleStep = (EndAngle - startAngle) / bulletSpawnAmount;
        float angle = startAngle;

        for(int i = 0; i < bulletSpawnAmount + 1; i++)
        {
            float bulDirX = transform.position.x + Mathf.Sin((angle * Mathf.PI) / 180);
            float bulDirY = transform.position.y + Mathf.Cos((angle * Mathf.PI) / 180);

            Vector3 bulMoveVector = new Vector3(bulDirX, bulDirY, 0f);
            Vector2 bulDir = (bulMoveVector - transform.position).normalized;

            GameObject bul = Instantiate(bullet, transform.position, transform.rotation);
            bul.GetComponent<EnemyBulletScript>().SetMoveDirection(bulDir);
            bul.GetComponent<NetworkObject>().Spawn();

            angle += angleStep;

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
