using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sendPos : MonoBehaviour
{
    // Start is called before the first frame update
    private float lastSend;
    private BaseClient client;

    void Start()
    {
        client = FindObjectOfType<BaseClient>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - lastSend > 1.0f) //more than 1 sec
        {
            Net_PlayerPosition ps = new Net_PlayerPosition(1, transform.position.x, transform.position.y, transform.position.z);
            client.SendToServer(ps);
            lastSend = Time.time;
        }
    }
}
