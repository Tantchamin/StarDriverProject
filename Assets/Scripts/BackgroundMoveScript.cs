using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class BackgroundMoveScript : MonoBehaviour
{
    public float speed = 10f;
    public float resetRate = 4f;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("ResetPosition", resetRate, resetRate);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position -= transform.up * speed * Time.deltaTime;
        
    }

    private void ResetPosition()
    {
        transform.position = new Vector3(0, 0, 0);
    }
}
