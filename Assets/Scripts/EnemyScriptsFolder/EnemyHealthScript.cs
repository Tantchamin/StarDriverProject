using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;

public class EnemyHealthScript : NetworkBehaviour
{
    public NetworkVariable<int> healthPointNetwork = new NetworkVariable<int>(100, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    public int setHealthPoint = 100;
    GameObject LeaveButton;
    LoginManagerScript loginManager;
    //public Slider healthBar;
    //private Slider hpBar;

    // Start is called before the first frame update
    void Start()
    {
        GameObject canvas = GameObject.FindWithTag("MainCanvas");
        loginManager = GameObject.FindGameObjectWithTag("LoginManager").GetComponent<LoginManagerScript>();
        LeaveButton = GameObject.FindGameObjectWithTag("LeaveButton");
        healthPointNetwork.Value = setHealthPoint;
        LeaveButton.SetActive(false);
        Debug.Log("BossHP: " + healthPointNetwork.Value);
        //hpBar = Instantiate(healthBar, Vector3.zero, Quaternion.identity) as Slider;
        //hpBar.transform.SetParent(canvas.transform);

    }

    // Update is called once per frame
    void Update()
    {
       // Vector3 hpBarPos = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, 1f, 0));
       // hpBar.transform.position = hpBarPos;

        if (healthPointNetwork.Value == 0)
        {
            
            DestroyServerRpc();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "PlayerBullet")
        {
            BulletScript bulletScript = other.GetComponent<BulletScript>();
            int getDamage = bulletScript.damage;
            ChangeHealthServerRpc(getDamage);
            Debug.Log("BossHP: " + healthPointNetwork.Value);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void ChangeHealthServerRpc(int damage)
    {
        healthPointNetwork.Value -= damage;
    }

    [ServerRpc(RequireOwnership = false)]
    private void DestroyServerRpc()
    {

        GetComponent<NetworkObject>().Despawn();
        Destroy(gameObject);
    }

    private void OnDisable()
    {
        loginManager.winButton.SetActive(true);
        LeaveButton.SetActive(true);
        //healthBar.enabled = false;
    }


}
