using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialBulletUIScript : MonoBehaviour
{
    public GameObject[] specialBulletUI;
    public SpecialBulletCapacityScript specialBulletCapacityScript;

    // Start is called before the first frame update
    void Start()
    {
        specialBulletCapacityScript = GameObject.FindGameObjectWithTag("BulletCapacity").GetComponent<SpecialBulletCapacityScript>();
    }

    // Update is called once per frame
    void Update()
    {
        playerABulletCheck();
    }

    void playerABulletCheck()
    {
        if (specialBulletCapacityScript.GetPlayerABulletCapacity() == 3)
        {
            specialBulletUI[0].SetActive(true);
            specialBulletUI[1].SetActive(true);
            specialBulletUI[2].SetActive(true);
        }
        else if (specialBulletCapacityScript.GetPlayerABulletCapacity() == 2)
        {
            specialBulletUI[0].SetActive(true);
            specialBulletUI[1].SetActive(true);
            specialBulletUI[2].SetActive(false);
        }
        else if (specialBulletCapacityScript.GetPlayerABulletCapacity() == 1)
        {
            specialBulletUI[0].SetActive(true);
            specialBulletUI[1].SetActive(false);
            specialBulletUI[2].SetActive(false);
        }
        else
        {
            specialBulletUI[0].SetActive(false);
            specialBulletUI[1].SetActive(false);
            specialBulletUI[2].SetActive(false);
        }
    }

    void playerBBulletCheck()
    {
        if (specialBulletCapacityScript.GetPlayerBBulletCapacity() == 3)
        {
            specialBulletUI[3].SetActive(true);
            specialBulletUI[4].SetActive(true);
            specialBulletUI[5].SetActive(true);
        }
        else if (specialBulletCapacityScript.GetPlayerBBulletCapacity() == 2)
        {
            specialBulletUI[3].SetActive(true);
            specialBulletUI[4].SetActive(true);
            specialBulletUI[5].SetActive(false);
        }
        else if (specialBulletCapacityScript.GetPlayerBBulletCapacity() == 1)
        {
            specialBulletUI[3].SetActive(true);
            specialBulletUI[4].SetActive(false);
            specialBulletUI[5].SetActive(false);
        }
        else
        {
            specialBulletUI[3].SetActive(false);
            specialBulletUI[4].SetActive(false);
            specialBulletUI[5].SetActive(false);
        }
    }

}
