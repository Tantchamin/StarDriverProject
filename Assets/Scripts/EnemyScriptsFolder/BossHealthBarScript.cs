using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthBarScript : MonoBehaviour
{
    EnemyHealthScript enemyHealth;
    public Slider slider;

    // Start is called before the first frame update
    void Start()
    {
        enemyHealth = GameObject.FindGameObjectWithTag("Enemy").GetComponent<EnemyHealthScript>();
        slider.maxValue = enemyHealth.setHealthPoint;
    }

    // Update is called once per frame
    void Update()
    {
        slider.value = enemyHealth.healthPointNetwork.Value;
    }

}
