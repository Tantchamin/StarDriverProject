using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class BossAnimationScript : NetworkBehaviour
{
    EnemyHealthScript enemyHealth;
    EnemyBulletSpawnPatternCircle pattern1;
    EnemyBulletSpawnPatternSpiral pattern2;
    BossMoveScript moveScript;
    NetworkVariable<int> healthPoint = new NetworkVariable<int>(100, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    Animator anim;
    private bool isStage2 = true, isStage3 = true;

    // Start is called before the first frame update
    void Start()
    {
        enemyHealth = GetComponent<EnemyHealthScript>();
        pattern1 = GetComponent<EnemyBulletSpawnPatternCircle>();
        pattern2 = GetComponent<EnemyBulletSpawnPatternSpiral>();
        pattern2.enabled = false;
        moveScript = GetComponent<BossMoveScript>();
        moveScript.enabled = false;
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        healthPoint.Value = enemyHealth.healthPointNetwork.Value;
        //Debug.Log("AnimHP: " + healthPoint.Value);


        if (healthPoint.Value <= enemyHealth.setHealthPoint && healthPoint.Value > (enemyHealth.setHealthPoint * 60) / 100)
        {

        }
        else if (healthPoint.Value <= (enemyHealth.setHealthPoint * 60) / 100 && healthPoint.Value > (enemyHealth.setHealthPoint * 30) / 100)
        {
            AnimationSetBool("isStage2", isStage2);
            pattern2.enabled = true;
        }
        else if (healthPoint.Value <= (enemyHealth.setHealthPoint * 30) / 100) 
        {
            AnimationSetBool("isStage3", isStage3);
            moveScript.enabled = true;
        }
        
    }

    void AnimationSetBool(string parameterName, bool parameterBool)
    {
        anim.SetBool(parameterName, parameterBool);
    }

}
